using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodReviewBackend.Models;
using NuGet.Packaging;

namespace GoodReviewBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagisController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public TagisController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Tagis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tagi>>> GetTagis()
        {
            return await _context.Tagis.ToListAsync();
        }

        // GET: api/Tagis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tagi>> GetTagi(int id)
        {
            var tagi = await _context.Tagis.FindAsync(id);

            if (tagi == null)
            {
                return NotFound();
            }

            return tagi;
        }

        // GET: api/Tagis/{id}/books
        [HttpGet("{id}/books")]
        public async Task<ActionResult<object>> GetBooksByTag(int id)
        {
            // Sprawdzenie, czy tag istnieje
            var tag = await _context.Tagis.FindAsync(id);
            if (tag == null)
            {
                return NotFound(); // Jeśli tag nie istnieje
            }

            // Pobranie książek powiązanych z tym tagiem
            var books = await _context.Ksiazkas
                .Where(k => k.IdOceny5s.Any(kt => kt.IdOceny5 == id)) // Filtrujemy książki powiązane z danym tagiem
                .Select(k => new { k.IdKsiazka, k.Tytul }) // Zwracamy tylko IdKsiazka oraz Tytul
                .ToListAsync();

            if (!books.Any())  // Jeśli brak książek powiązanych z tagiem
            {
                return NoContent(); // Zwróć 204 No Content, jeśli brak książek
            }

            // Zwracamy wynik z tagiem oraz listą książek
            return Ok(new
            {
                TagName = tag.NazwaTagu, // Zwracamy nazwę tagu z modelu Tagi
                Books = books
            });
        }


        // PUT: api/Tagis/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTagi(int id, Tagi tagi)
        {
            if (id != tagi.IdOceny5)
            {
                return BadRequest();
            }

            _context.Entry(tagi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagiExists(id))
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

        // POST: api/Tagis
        [HttpPost]
        public async Task<ActionResult<Tagi>> PostTagi(Tagi tagi)
        {
            _context.Tagis.Add(tagi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTagi", new { id = tagi.IdOceny5 }, tagi);
        }

        // DELETE: api/Tagis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTagi(int id)
        {
            var tagi = await _context.Tagis.FindAsync(id);
            if (tagi == null)
            {
                return NotFound();
            }

            _context.Tagis.Remove(tagi);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // POST: api/Tagis/{bookId}/assign-tags
        [HttpPost("{bookId}/assign-tags")]
        public async Task<IActionResult> AssignTagsToBook(int bookId, [FromBody] List<int> tagIds)
        {
            // Sprawdzenie, czy książka istnieje
            var ksiazka = await _context.Ksiazkas.FindAsync(bookId);
            if (ksiazka == null)
            {
                return NotFound(); // Jeśli książka nie istnieje
            }

            // Sprawdzenie, czy tagi istnieją
            var tags = await _context.Tagis.Where(t => tagIds.Contains(t.IdOceny5)).ToListAsync();
            if (tags.Count != tagIds.Count)
            {
                return NotFound("Some tags not found"); // Jeśli któryś z tagów nie istnieje
            }

            // Przypisanie tagów do książki
            foreach (var tag in tags)
            {
                // Sprawdzenie, czy dany tag nie jest już przypisany do książki
                if (!ksiazka.IdOceny5s.Contains(tag))
                {
                    ksiazka.IdOceny5s.Add(tag); // Dodajemy tag tylko jeśli nie jest już przypisany
                }
            }

            try
            {
                await _context.SaveChangesAsync(); // Zapisz zmiany do bazy danych
            }
            catch (Exception ex)
            {
                return BadRequest("Error assigning tags: " + ex.Message);
            }

            // Zwracamy książkę z przypisanymi tagami
            var result = ksiazka.IdOceny5s.Select(t => new { t.IdOceny5, t.NazwaTagu }).ToList();
            return Ok(result); // Zwracamy listę tagów powiązanych z książką
        }
        // GET: api/Tagis/{bookId}/tags
        [HttpGet("{bookId}/tags")]
        public async Task<ActionResult<IEnumerable<object>>> GetTagsForBook(int bookId)
        {
            // Sprawdzenie, czy książka istnieje
            var ksiazka = await _context.Ksiazkas
                .Include(k => k.IdOceny5s) // Upewniamy się, że tagi są załadowane
                .FirstOrDefaultAsync(k => k.IdKsiazka == bookId);

            if (ksiazka == null)
            {
                return NotFound(); // Jeśli książka nie istnieje
            }

            // Zwracamy tagi przypisane do książki
            var tags = ksiazka.IdOceny5s.Select(t => new { t.IdOceny5, t.NazwaTagu }).ToList();

            if (!tags.Any())
            {
                return NoContent(); // Zwracamy 204 No Content, jeśli brak tagów
            }

            return Ok(tags); // Zwracamy listę tagów
        }



        private bool TagiExists(int id)
        {
            return _context.Tagis.Any(e => e.IdOceny5 == id);
        }
    }
}
