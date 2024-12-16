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

        // PUT: api/Tagis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        private bool TagiExists(int id)
        {
            return _context.Tagis.Any(e => e.IdOceny5 == id);
        }
    }
}
