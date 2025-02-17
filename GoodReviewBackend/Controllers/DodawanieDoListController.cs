using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using GoodReviewBackend.Models;

namespace GoodReviewBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DodawanieDoListController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public DodawanieDoListController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // Endpoint zwracający listy z przypisanymi książkami dla konkretnego użytkownika
        [HttpGet("user/{userId}/lists-with-books")]
        public async Task<IActionResult> GetListsWithBooksForUser(int userId)
        {
            var listsWithBooks = await _context.Lista
                .Where(l => l.IdUzytkownik == userId) // Filtruj listy po ID użytkownika
                .Include(l => l.IdKsiazkas) // Ładowanie książek przypisanych do list
                .Select(l => new
                {
                    l.IdListy,
                    l.NazwaListy,
                    l.IdUzytkownik, // Zwróć ID użytkownika
                    Books = l.IdKsiazkas.Select(k => new
                    {
                        k.IdKsiazka,
                        k.Tytul,
                        k.Opis
                    }).ToList()
                })
                .ToListAsync();

            return Ok(listsWithBooks);
        }
        // Endpoint zwracający książki dla konkretnej listy
        [HttpGet("list/{listId}/books")]
        public async Task<IActionResult> GetBooksInList(int listId)
        {
            var listWithBooks = await _context.Lista
                .Where(l => l.IdListy == listId)  // Filtrujemy po ID listy
                .Include(l => l.IdKsiazkas)  // Ładujemy książki przypisane do listy
                .Select(l => new
                {
                    l.IdListy,
                    l.NazwaListy,
                    Books = l.IdKsiazkas.Select(k => new
                    {
                        k.IdKsiazka,
                        k.Tytul,
                        k.Opis
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (listWithBooks == null)
            {
                return NotFound(new { message = "List not found." });
            }

            return Ok(listWithBooks);
        }


        // Endpoint dodający książkę do listy użytkownika
        [HttpPost("add-book-to-list")]
        public async Task<IActionResult> AddBookToList([FromBody] AddBookToListRequest request)
        {
            var list = await _context.Lista
                .FirstOrDefaultAsync(l => l.IdListy == request.ListId && l.IdUzytkownik == request.UserId);

            if (list == null)
            {
                return NotFound(new { message = "List not found or does not belong to the user." });
            }

            var book = await _context.Ksiazkas.FindAsync(request.BookId);
            if (book == null)
            {
                return NotFound(new { message = "Book not found." });
            }

            // Sprawdzenie, czy książka już jest na liście
            if (list.IdKsiazkas.Any(b => b.IdKsiazka == request.BookId))
            {
                return Conflict(new { message = "Book already exists in the list." });
            }

            list.IdKsiazkas.Add(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Book added to the list." });
        }

        // Endpoint usuwający książkę z listy użytkownika
        [HttpDelete("remove-book-from-list/{userId}/{listId}/{bookId}")]
        public async Task<IActionResult> RemoveBookFromList(int userId, int listId, int bookId)
        {
            var list = await _context.Lista
                .Include(l => l.IdKsiazkas)
                .FirstOrDefaultAsync(l => l.IdListy == listId && l.IdUzytkownik == userId);

            if (list == null)
            {
                return NotFound(new { message = "List not found or does not belong to the user." });
            }

            var book = list.IdKsiazkas.FirstOrDefault(b => b.IdKsiazka == bookId);
            if (book == null)
            {
                return NotFound(new { message = "Book not found in the list." });
            }

            list.IdKsiazkas.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Book removed from the list." });
        }
    }

    // Model pomocniczy do dodawania książki do listy
    public class AddBookToListRequest
    {
        public int UserId { get; set; }  // ID użytkownika
        public int ListId { get; set; } // ID listy
        public int BookId { get; set; } // ID książki
    }
}
