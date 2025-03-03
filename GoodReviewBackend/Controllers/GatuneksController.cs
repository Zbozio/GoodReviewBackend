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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gatunek>>> GetGatuneks()
        {
            return await _context.Gatuneks.ToListAsync();
        }

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

       
        [HttpPost]
        public async Task<ActionResult<Gatunek>> PostGatunek(Gatunek gatunek)
        {
            _context.Gatuneks.Add(gatunek);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGatunek", new { id = gatunek.IdGatunku }, gatunek);
        }

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
                .Include(g => g.IdKsiazkas) 
                .FirstOrDefaultAsync(g => g.IdGatunku == id);

            if (gatunek == null)
            {
                return NotFound();
            }

            return Ok(gatunek.IdKsiazkas); 
        }

        private bool GatunekExists(int id)
        {
            return _context.Gatuneks.Any(e => e.IdGatunku == id);
        }
    }
}
