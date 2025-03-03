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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tagi>>> GetTagis()
        {
            return await _context.Tagis.ToListAsync();
        }

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

        [HttpGet("{id}/books")]
        public async Task<ActionResult<object>> GetBooksByTag(int id)
        {
            var tag = await _context.Tagis.FindAsync(id);
            if (tag == null)
            {
                return NotFound(); 
            }

            var books = await _context.Ksiazkas
                .Where(k => k.IdOceny5s.Any(kt => kt.IdOceny5 == id)) 
                .Select(k => new { k.IdKsiazka, k.Tytul }) 
                .ToListAsync();

            if (!books.Any())  
            {
                return NoContent(); 
            }

            return Ok(new
            {
                TagName = tag.NazwaTagu,
                Books = books
            });
        }


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

        [HttpPost]
        public async Task<ActionResult<Tagi>> PostTagi(Tagi tagi)
        {
            _context.Tagis.Add(tagi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTagi", new { id = tagi.IdOceny5 }, tagi);
        }

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
        [HttpPost("{bookId}/assign-tags")]
        public async Task<IActionResult> AssignTagsToBook(int bookId, [FromBody] List<int> tagIds)
        {
            var ksiazka = await _context.Ksiazkas.FindAsync(bookId);
            if (ksiazka == null)
            {
                return NotFound(); 
            }

            var tags = await _context.Tagis.Where(t => tagIds.Contains(t.IdOceny5)).ToListAsync();
            if (tags.Count != tagIds.Count)
            {
                return NotFound("Some tags not found");
            }

            foreach (var tag in tags)
            {
                if (!ksiazka.IdOceny5s.Contains(tag))
                {
                    ksiazka.IdOceny5s.Add(tag);
                }
            }

            try
            {
                await _context.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                return BadRequest("Error assigning tags: " + ex.Message);
            }

            var result = ksiazka.IdOceny5s.Select(t => new { t.IdOceny5, t.NazwaTagu }).ToList();
            return Ok(result); 
        }
        [HttpGet("{bookId}/tags")]
        public async Task<ActionResult<IEnumerable<object>>> GetTagsForBook(int bookId)
        {
            
            var ksiazka = await _context.Ksiazkas
                .Include(k => k.IdOceny5s)
                .FirstOrDefaultAsync(k => k.IdKsiazka == bookId);

            if (ksiazka == null)
            {
                return NotFound(); 
            }

            var tags = ksiazka.IdOceny5s.Select(t => new { t.IdOceny5, t.NazwaTagu }).ToList();

            if (!tags.Any())
            {
                return NoContent(); 
            }

            return Ok(tags);
        }



        private bool TagiExists(int id)
        {
            return _context.Tagis.Any(e => e.IdOceny5 == id);
        }
    }
}
