using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodReviewBackend.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GoodReviewBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddingBookController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public AddingBookController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Sprawdzenie, czy książka już istnieje na podstawie ISBN
                var existingBook = await _context.Ksiazkas
                    .FirstOrDefaultAsync(b => b.Isbn == bookDto.Isbn);
                if (existingBook != null)
                {
                    return Conflict(new { Message = "Book with this ISBN already exists." });
                }

                // Sprawdzenie, czy wydawnictwo istnieje
                var publisher = await _context.Wydawnictwos.FindAsync(bookDto.PublisherId);
                if (publisher == null)
                {
                    return BadRequest(new { Message = $"Publisher with ID {bookDto.PublisherId} does not exist." });
                }

                // Tworzenie nowej książki
                var newBook = new Ksiazka
                {
                    Tytul = bookDto.Title,
                    Opis = bookDto.Description,
                    RokWydania = bookDto.ReleaseYear,
                    IloscStron = bookDto.Pages,
                    Okladka = bookDto.Cover,
                    Isbn = bookDto.Isbn,
                    IdWydawnictwa = publisher.IdWydawnictwa
                };
                _context.Ksiazkas.Add(newBook);
                await _context.SaveChangesAsync();

                // Dodanie gatunków do książki
                foreach (var genreId in bookDto.GenreIds)
                {
                    var genre = await _context.Gatuneks.FindAsync(genreId);
                    if (genre != null)
                    {
                        newBook.IdGatunkus.Add(genre);
                    }
                }

                // Dodanie autorów i ról
                foreach (var authorDto in bookDto.Authors)
                {
                    // Sprawdzanie, czy autor istnieje po ID
                    var author = await _context.Autors
                        .FirstOrDefaultAsync(a => a.IdAutora == authorDto.AuthorId);
                    if (author == null)
                    {
                        return BadRequest(new { Message = $"Author with ID {authorDto.AuthorId} does not exist." });
                    }

                    var authorshipType = await _context.TypAutorstwas
                        .FirstOrDefaultAsync(t => t.IdTypu == authorDto.AuthorshipTypeId);
                    if (authorshipType == null)
                    {
                        return BadRequest(new { Message = $"Invalid authorship type ID: {authorDto.AuthorshipTypeId}" });
                    }

                    var participation = new Udzial
                    {
                        IdKsiazka = newBook.IdKsiazka,
                        IdAutora = author.IdAutora,
                        IdTypu = authorshipType.IdTypu,
                        
                    };
                    _context.Udzials.Add(participation);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Pobranie pełnych danych książki z autorami
                var addedBook = await _context.Ksiazkas
                    .Include(b => b.IdWydawnictwaNavigation)
                    .Include(b => b.IdGatunkus)
                    .Include(b => b.Udzials)
                        .ThenInclude(u => u.IdAutoraNavigation)
                    .Include(b => b.Udzials)
                        .ThenInclude(u => u.IdTypuNavigation)
                    .FirstOrDefaultAsync(b => b.IdKsiazka == newBook.IdKsiazka);

                // Przygotowanie odpowiedzi z pełnymi danymi książki i autorami
                var bookResponse = new
                {
                    addedBook.IdKsiazka,
                    addedBook.Tytul,
                    addedBook.Opis,
                    addedBook.RokWydania,
                    addedBook.IloscStron,
                    addedBook.Okladka,
                    addedBook.Isbn,
                    Publisher = addedBook.IdWydawnictwaNavigation?.Nazwa,
                    Genres = addedBook.IdGatunkus.Select(g => g.NazwaGatunku),
                    Authors = addedBook.Udzials.Select(u => new
                    {
                        AuthorId = u.IdAutora,
                        FirstName = u.IdAutoraNavigation.ImieAutora,
                        LastName = u.IdAutoraNavigation.NazwiskoAutora,
                        AuthorshipType = u.IdTypuNavigation.NazwaTypu,
                        ContributionValue = u.WartoscUdzialu
                    })
                };

                return CreatedAtAction(nameof(AddBook), new { id = addedBook.IdKsiazka }, bookResponse);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "Error adding book.", Details = ex.Message });
            }
        }

        // GET: api/AddingBook/roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.TypAutorstwas
                .Select(r => new { r.IdTypu, r.NazwaTypu })
                .ToListAsync();

            return Ok(roles);
        }
    }



// DTO do przesyłania danych książki
// DTO do przesyłania danych książki
public class BookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseYear { get; set; }
        public int Pages { get; set; }
        public string Cover { get; set; }
        public string Isbn { get; set; }
        public int PublisherId { get; set; } // Zmienione z PublisherName na PublisherId
        public List<int> GenreIds { get; set; } = new List<int>();
        public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();

        public class AuthorDto
        {
            public int AuthorId { get; set; } // ID autora
            public int AuthorshipTypeId { get; set; } // ID typu autorstwa
            public int ContributionValue { get; set; } // Wartość udziału
        }
    }



}
