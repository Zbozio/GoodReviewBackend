using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodReviewBackend.Models;
using System.Linq;
using System.Threading.Tasks;

namespace GoodReviewBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatunkowoscController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public GatunkowoscController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("books-with-genres")]
        public async Task<IActionResult> GetBooksWithGenres()
        {
            var booksWithGenres = await _context.Ksiazkas
                .Include(k => k.IdGatunkus) 
                .Select(k => new
                {
                    k.IdKsiazka,
                    k.Tytul,
                    k.Opis,
                    Genres = k.IdGatunkus.Select(g => new
                    {
                        g.IdGatunku,          
                        g.NazwaGatunku       
                    }).ToList()
                })
                .ToListAsync();

            return Ok(booksWithGenres);
        }

        
        [HttpGet("genres-with-books")]
        public async Task<IActionResult> GetGenresWithBooks()
        {
            var genresWithBooks = await _context.Gatuneks
                .Include(g => g.IdKsiazkas) 
                .Select(g => new
                {
                    g.IdGatunku,                
                    g.NazwaGatunku,
                    Books = g.IdKsiazkas.Select(k => new
                    {   
                        k.IdKsiazka,
                        k.Tytul                   
                    }).ToList()
                })
                .ToListAsync();

            return Ok(genresWithBooks);
        }

        [HttpGet("books/{id}/genres")]
        public async Task<IActionResult> GetGenresForBook(int id)
        {
            var bookWithGenres = await _context.Ksiazkas
                .Where(k => k.IdKsiazka == id)
                .Include(k => k.IdGatunkus)  
                .Select(k => new
                {
                    k.IdKsiazka,
                    k.Tytul,
                    k.Opis,
                    Genres = k.IdGatunkus.Select(g => new
                    {
                        g.IdGatunku,          
                        g.NazwaGatunku      
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (bookWithGenres == null)
            {
                return NotFound(new { message = "Book not found." });
            }

            return Ok(bookWithGenres);
        }

        [HttpGet("genres/{id}/books")]
        public async Task<IActionResult> GetBooksForGenre(int id)
        {
            var genreWithBooks = await _context.Gatuneks
                .Where(g => g.IdGatunku == id)
                .Include(g => g.IdKsiazkas)  
                .Select(g => new
                {
                    g.NazwaGatunku,
                    Books = g.IdKsiazkas.Select(k => new
                    {
                        k.IdKsiazka,
                        k.Tytul,
                        k.Okladka 
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (genreWithBooks == null)
            {
                return NotFound(new { message = "Genre not found." });
            }

            return Ok(genreWithBooks);
        }
    }
}
