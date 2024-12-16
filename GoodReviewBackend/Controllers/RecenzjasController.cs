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
    public class RecenzjasController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public RecenzjasController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Recenzjas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recenzja>>> GetRecenzjas()
        {
            return await _context.Recenzjas.ToListAsync();
        }

        // GET: api/Recenzjas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recenzja>> GetRecenzja(int id)
        {
            var recenzja = await _context.Recenzjas.FindAsync(id);

            if (recenzja == null)
            {
                return NotFound();
            }

            return recenzja;
        }

        // PUT: api/Recenzjas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecenzja(int id, Recenzja recenzja)
        {
            if (id != recenzja.IdRecenzji)
            {
                return BadRequest();
            }

            _context.Entry(recenzja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecenzjaExists(id))
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

        // POST: api/Recenzjas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recenzja>> PostRecenzja(Recenzja recenzja)
        {
            _context.Recenzjas.Add(recenzja);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecenzja", new { id = recenzja.IdRecenzji }, recenzja);
        }

        // DELETE: api/Recenzjas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecenzja(int id)
        {
            var recenzja = await _context.Recenzjas.FindAsync(id);
            if (recenzja == null)
            {
                return NotFound();
            }

            _context.Recenzjas.Remove(recenzja);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecenzjaExists(int id)
        {
            return _context.Recenzjas.Any(e => e.IdRecenzji == id);
        }
    }
}
