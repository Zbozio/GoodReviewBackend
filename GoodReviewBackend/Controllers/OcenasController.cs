using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodReviewBackend.Models;

namespace GoodReviewBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcenasController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public OcenasController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Ocenas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOcenas()
        {
            var ocenyWithBooksAndGenres = await _context.Ocenas
                .Include(o => o.IdKsiazkaNavigation)  // Dołączamy książki związane z ocenami
                    .ThenInclude(k => k.IdGatunkus)  // Dołączamy gatunki książek
                .Select(o => new
                {
                    o.IdOceny,
                    o.WartoscOceny,
                    o.DataOceny,
                    Ksiazka = new
                    {
                        o.IdKsiazkaNavigation.Tytul, 
                        Gatunki = o.IdKsiazkaNavigation.IdGatunkus.Select(g => new
                        {
                            g.IdGatunku,    
                            g.NazwaGatunku   
                        }).ToList()
                    }
                })
                .ToListAsync();

            return Ok(ocenyWithBooksAndGenres);
        }
        [HttpGet("UserRatingsSorted/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserRatingsSorted(int userId)
        {
            var userRatingsWithBooksAndGenres = await _context.Ocenas
                .Where(o => o.IdUzytkownik == userId)  // Filtrujemy po IdUzytkownik
                .Include(o => o.IdKsiazkaNavigation)  // Dołączamy książki związane z ocenami
                    .ThenInclude(k => k.IdGatunkus)  // Dołączamy gatunki książek
                .OrderByDescending(o => o.WartoscOceny)  // Sortujemy oceny od najwyższej do najniższej
                .Select(o => new
                {
                    o.IdOceny,
                    o.WartoscOceny,
                    o.DataOceny,
                    Ksiazka = new
                    {
                        o.IdKsiazkaNavigation.Tytul,  
                        Gatunki = o.IdKsiazkaNavigation.IdGatunkus.Select(g => new
                        {
                            g.IdGatunku,    
                            g.NazwaGatunku   
                        }).ToList()
                    }
                })
                .ToListAsync();

            if (userRatingsWithBooksAndGenres == null || userRatingsWithBooksAndGenres.Count == 0)
            {
                return NotFound($"No ratings found for user with ID {userId}");
            }

            return Ok(userRatingsWithBooksAndGenres);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Ocena>> GetOcena(int id)
        {
            var ocena = await _context.Ocenas.FindAsync(id);

            if (ocena == null)
            {
                return NotFound();
            }

            return ocena;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOcena(int id, Ocena ocena)
        {
            if (id != ocena.IdOceny)
            {
                return BadRequest();
            }

            _context.Entry(ocena).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OcenaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Ocena>> PostOcena(Ocena ocena)
        {
            _context.Ocenas.Add(ocena);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOcena", new { id = ocena.IdOceny }, ocena);
        }
        
        
        [HttpGet("UserRatings/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserRatings(int userId)
        {
            // Pobieramy oceny użytkownika, a także powiązane informacje o książkach i użytkownikach
            var userRatings = await _context.Ocenas
                .Where(o => o.IdUzytkownik == userId)
                .Include(o => o.IdKsiazkaNavigation)  // Dołączamy dane o książce przez właściwość nawigacyjną
                    .ThenInclude(k => k.IdGatunkus)  // Dołączamy powiązane gatunki książki
                .Include(o => o.IdUzytkownikNavigation)  // Dołączamy dane o użytkowniku
                .ToListAsync();

            if (userRatings == null || userRatings.Count == 0)
            {
                return NotFound($"No ratings found for user with ID {userId}");
            }

            // Nowa lista, która zwróci tytuł książki, zdjęcie użytkownika, okładkę książki i powiązane gatunki
            var ratingsWithBookData = userRatings.Select(o => new
            {
                o.IdOceny,
                o.IdUzytkownik,
                o.IdKsiazka,
                o.WartoscOceny,
                o.DataOceny,
                Tytul = o.IdKsiazkaNavigation?.Tytul,  
                Imie = o.IdUzytkownikNavigation?.Imie,
                Nazwisko = o.IdUzytkownikNavigation?.Nazwisko,
                Zdjecie = o.IdUzytkownikNavigation?.Zdjecie,  
                Okladka = o.IdKsiazkaNavigation?.Okladka,  
                Gatunki = o.IdKsiazkaNavigation?.IdGatunkus.Select(g => new
                {
                    g.IdGatunku,      
                    g.NazwaGatunku     
                }).ToList()  
            }).ToList();

            return Ok(ratingsWithBookData);
        }



        [HttpGet("UserRecommendations/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserRecommendations(int userId)
        {
            var userRatings = await _context.Ocenas
                .Where(o => o.IdUzytkownik == userId)
                .Include(o => o.IdKsiazkaNavigation)
                    .ThenInclude(k => k.IdGatunkus)
                .Include(o => o.IdKsiazkaNavigation)
                    .ThenInclude(k => k.Udzials)
                        .ThenInclude(u => u.IdAutoraNavigation)
                .ToListAsync();

            if (userRatings == null || userRatings.Count == 0)
            {
                return NotFound($"No ratings found for user with ID {userId}");
            }

            var genrePoints = userRatings
                .SelectMany(r => r.IdKsiazkaNavigation.IdGatunkus.Select(g => g.IdGatunku))
                .GroupBy(g => g)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select((g, index) => new { GenreId = g.Key, Points = 1 - (index * 0.2) }) 
                .ToDictionary(g => g.GenreId, g => g.Points);

            var userAverageRating = userRatings.Average(r => r.WartoscOceny);

            var averagePages = userRatings.Average(r => r.IdKsiazkaNavigation.IloscStron);

            var highRatedAuthors = userRatings
                .Where(r => r.WartoscOceny > 8)
                .SelectMany(r => r.IdKsiazkaNavigation.Udzials.Select(u => u.IdAutora))
                .Distinct()
                .ToList();

            var ratedBookIds = userRatings.Select(r => r.IdKsiazka).ToList();
            var unratedBooks = await _context.Ksiazkas
                .Where(k => !ratedBookIds.Contains(k.IdKsiazka))
                .Include(k => k.IdGatunkus)
                .Include(k => k.Udzials)
                .ToListAsync();

            var recommendations = unratedBooks
                .Select(book =>
                {
                    // Punkty za występujące gatunki
                    var genreScore = book.IdGatunkus
                        .Where(g => genrePoints.ContainsKey(g.IdGatunku))
                        .Sum(g => genrePoints[g.IdGatunku]);

                    // Punkty za zbliżoność do średniej ilości stron
                    var pageScore = book.IloscStron.HasValue ? GetPageScore(book.IloscStron.Value, (double)averagePages) : 0;

                    // Punkty za występowanie autora
                    var authorBonus = book.Udzials.Any(u => highRatedAuthors.Contains(u.IdAutora)) ? 0.2 : 0;

                    // Średnia ocena użytkowników dla danej książki
                    var averageRating = _context.Ocenas
                        .Where(r => r.IdKsiazka == book.IdKsiazka)
                        .Average(r => (double?)r.WartoscOceny) ?? 0;

                    var totalScore = ((genreScore + pageScore) * averageRating/10 + authorBonus);

                    return new
                    {
                        BookId = book.IdKsiazka,
                        Title = book.Tytul,
                        IloscStron=book.IloscStron,
                        genrePoints=genrePoints,
                        pageScore=pageScore,
                        authorBonus=authorBonus,
                        Okladka = book.Okladka,
                        Genres = book.IdGatunkus.Select(g => new { g.IdGatunku, g.NazwaGatunku }),
                        Authors = book.Udzials.Select(a => a.IdAutora),
                        AverageRating = averageRating,
                        Score = totalScore
                    };
                })
                .Where(r => r.Score > 0)
                .OrderByDescending(r => r.Score)
                .Take(6)
                .ToList();

            return Ok(recommendations);
        }


        private double GetPageScore(int bookPages, double averagePages)
        {
            var differencePercentage = Math.Abs(bookPages - averagePages) / averagePages * 100;

            if (differencePercentage <= 10)
                return 0.4;
            else if (differencePercentage <= 30)
                return 0.3;
            else if (differencePercentage <= 60)
                return 0.2;
            else
                return 0.1;
        }

        [HttpGet("UserRecommendationsMatrix/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserRecommendationsMatrix(int userId)
        {
            // Pobierz oceny użytkownika wraz z książkami i ich gatunkami
            var userRatings = await _context.Ocenas
                .Where(o => o.IdUzytkownik == userId)
                .Include(o => o.IdKsiazkaNavigation)
                    .ThenInclude(k => k.IdGatunkus)
                .ToListAsync();

            if (userRatings == null || userRatings.Count == 0)
            {
                return NotFound($"No ratings found for user with ID {userId}");
            }

            var highRatedGenres = userRatings
                .Where(r => r.WartoscOceny >= 8)
                .SelectMany(r => r.IdKsiazkaNavigation.IdGatunkus.Select(g => g.IdGatunku)) 
                .GroupBy(g => g) 
                .OrderByDescending(g => g.Count()) 
                .Take(5) 
                .Select(g => g.Key) 
                .ToList();

            var allBooks = await _context.Ksiazkas
                .Include(k => k.IdGatunkus)
                .ToListAsync();

            var bookFeatureMatrix = allBooks.Select(book => new
            {
                BookId = book.IdKsiazka,
                Features = highRatedGenres.Select(g =>
                    book.IdGatunkus.Any(bGenre => bGenre.IdGatunku == g) ? 1 : 0)
                    .ToArray()
            }).ToDictionary(b => b.BookId, b => b.Features);

            var userFeatureVector = userRatings
                .Where(r => r.WartoscOceny >= 8) 
                .Select(r => bookFeatureMatrix[r.IdKsiazka.GetValueOrDefault()])
                .ToList();

            var userTopGenresVector = new[] { 1, 1, 1, 1, 1 };

            double magnitudeB = Math.Sqrt(userTopGenresVector.Sum(f => f * f));

            var detailedResults = allBooks
                .Where(book => !userRatings.Any(r => r.IdKsiazka == book.IdKsiazka)) 
                .Select(book =>
                {
                    var bookFeatures = bookFeatureMatrix[book.IdKsiazka];

                    double dotProduct = userTopGenresVector.Zip(bookFeatures, (uf, bf) => uf * bf).Sum();

                    double magnitudeA = Math.Sqrt(bookFeatures.Sum(f => f * f));

                    // Obliczanie podobieństwa cosinusowego
                    double similarity = (magnitudeA > 0 && magnitudeB > 0)
                        ? dotProduct / (magnitudeA * magnitudeB)
                        : 0;

                    return new
                    {
                        BookId = book.IdKsiazka,
                        Title = book.Tytul,
                        SimilarityScore = similarity,
                        DotProduct = dotProduct,
                        MagnitudeA = magnitudeA,
                        MagnitudeB = magnitudeB,
                        Okladka=book.Okladka,
                        BookFeatures = string.Join(", ", bookFeatures),
                        Genres = book.IdGatunkus.Select(g => new { g.IdGatunku, g.NazwaGatunku }),
                        Pages = book.IloscStron
                    };
                })
                .OrderByDescending(r => r.SimilarityScore) // Sortuj według podobieństwa
                .Take(6) // Wybierz 5 najlepszych książek
                .ToList();


            return Ok(detailedResults);

        }








        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOcena(int id)
        {
            var ocena = await _context.Ocenas.FindAsync(id);
            if (ocena == null)
            {
                return NotFound();
            }

            _context.Ocenas.Remove(ocena);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool OcenaExists(int id)
        {
            return _context.Ocenas.Any(e => e.IdOceny == id);
        }
    }
}
