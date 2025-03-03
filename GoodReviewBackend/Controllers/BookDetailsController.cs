using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodReviewBackend.Models;

namespace GoodReviewBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookDetailsController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public BookDetailsController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetBookDetails(int id)
        {
            var bookDetails = _context.Ksiazkas
                .Include(b => b.IdWydawnictwaNavigation) 
                .Include(b => b.IdGatunkus) 
                .Include(b => b.IdOceny5s) 
                .Include(b => b.Ocenas) 
                .Include(b => b.Udzials)
                    .ThenInclude(u => u.IdAutoraNavigation) 
                .Include(b => b.Udzials)
                    .ThenInclude(u => u.IdTypuNavigation) 
                .Where(b => b.IdKsiazka == id)
                .Select(b => new
                {
                    Id = b.IdKsiazka,
                    Title = b.Tytul,
                    Description = b.Opis,
                    ReleaseYear = b.RokWydania,
                    Pages = b.IloscStron,
                    Cover = b.Okladka,
                    ISBN = b.Isbn,
                    Publisher = b.IdWydawnictwaNavigation != null
                        ? new
                        {
                            Id = b.IdWydawnictwaNavigation.IdWydawnictwa,
                            Name = b.IdWydawnictwaNavigation.Nazwa
                        }
                        : null,
                    Genres = b.IdGatunkus.Select(g => new { Id = g.IdGatunku, Name = g.NazwaGatunku }),
                    Tags = b.IdOceny5s.Select(t => new { Id = t.IdOceny5, Name = t.NazwaTagu }),
                    Authors = b.Udzials.Select(u => new
                    {
                        Id = u.IdAutoraNavigation.IdAutora,
                        FirstName = u.IdAutoraNavigation.ImieAutora,
                        LastName = u.IdAutoraNavigation.NazwiskoAutora,
                     
                        AuthorshipType = u.IdTypuNavigation != null
                            ? new { Id = u.IdTypuNavigation.IdTypu, Name = u.IdTypuNavigation.NazwaTypu }
                            : null
                    }),


                    AverageRating = b.Ocenas.Any() ? Math.Round((decimal)b.Ocenas.Average(o => o.WartoscOceny), 1) : 0,
                    TotalRatings = b.Ocenas.Count()
                })
                .FirstOrDefault();

            if (bookDetails == null)
            {
                return NotFound(new { Message = "Book not found." });
            }

            return Ok(bookDetails);
        }
    }
}
