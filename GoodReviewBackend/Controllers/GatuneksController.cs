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
    public class GatuneksController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public GatuneksController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Gatuneks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gatunek>>> GetGatuneks()
        {
            return await _context.Gatuneks.ToListAsync();
        }

        // GET: api/Gatuneks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gatunek>> GetGatunek(int id)
        {
            var gatunek = await _context.Gatuneks.FindAsync(id);

            if (gatunek == null)
            {
                return NotFound();
            }

            return gatunek;
        }

        // PUT: api/Gatuneks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGatunek(int id, Gatunek gatunek)
        {
            if (id != gatunek.IdGatunku)
            {
                return BadRequest();
            }

            _context.Entry(gatunek).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GatunekExists(id))
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

        // POST: api/Gatuneks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Gatunek>> PostGatunek(Gatunek gatunek)
        {
            _context.Gatuneks.Add(gatunek);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGatunek", new { id = gatunek.IdGatunku }, gatunek);
        }

        // DELETE: api/Gatuneks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGatunek(int id)
        {
            var gatunek = await _context.Gatuneks.FindAsync(id);
            if (gatunek == null)
            {
                return NotFound();
            }

            _context.Gatuneks.Remove(gatunek);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/ksiazki")]
        public async Task<ActionResult<IEnumerable<Ksiazka>>> GetKsiazkiByGatunek(int id)
        {
            var gatunek = await _context.Gatuneks
                .Include(g => g.IdKsiazkas) // Dołączamy powiązane książki
                .FirstOrDefaultAsync(g => g.IdGatunku == id);

            if (gatunek == null)
            {
                return NotFound();
            }

            return Ok(gatunek.IdKsiazkas); // Zwracamy książki powiązane z gatunkiem
        }

        private bool GatunekExists(int id)
        {
            return _context.Gatuneks.Any(e => e.IdGatunku == id);
        }
    }
}
