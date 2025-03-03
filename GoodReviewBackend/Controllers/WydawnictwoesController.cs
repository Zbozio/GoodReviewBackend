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
    public class WydawnictwoesController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public WydawnictwoesController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wydawnictwo>>> GetWydawnictwos()
        {
            return await _context.Wydawnictwos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Wydawnictwo>> GetWydawnictwo(int id)
        {
            var wydawnictwo = await _context.Wydawnictwos.FindAsync(id);

            if (wydawnictwo == null)
            {
                return NotFound();
            }

            return wydawnictwo;
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWydawnictwo(int id, Wydawnictwo wydawnictwo)
        {
            if (id != wydawnictwo.IdWydawnictwa)
            {
                return BadRequest();
            }

            _context.Entry(wydawnictwo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WydawnictwoExists(id))
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
        public async Task<ActionResult<Wydawnictwo>> PostWydawnictwo(Wydawnictwo wydawnictwo)
        {
            _context.Wydawnictwos.Add(wydawnictwo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWydawnictwo", new { id = wydawnictwo.IdWydawnictwa }, wydawnictwo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWydawnictwo(int id)
        {
            var wydawnictwo = await _context.Wydawnictwos.FindAsync(id);
            if (wydawnictwo == null)
            {
                return NotFound();
            }

            _context.Wydawnictwos.Remove(wydawnictwo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WydawnictwoExists(int id)
        {
            return _context.Wydawnictwos.Any(e => e.IdWydawnictwa == id);
        }
    }
}
