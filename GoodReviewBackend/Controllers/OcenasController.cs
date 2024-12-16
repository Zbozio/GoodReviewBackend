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
        public async Task<ActionResult<IEnumerable<Ocena>>> GetOcenas()
        {
            return await _context.Ocenas.ToListAsync();
        }

        // GET: api/Ocenas/5
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

        // PUT: api/Ocenas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Ocenas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ocena>> PostOcena(Ocena ocena)
        {
            _context.Ocenas.Add(ocena);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOcena", new { id = ocena.IdOceny }, ocena);
        }

        // DELETE: api/Ocenas/5
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
