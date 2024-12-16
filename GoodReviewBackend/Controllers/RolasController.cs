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
    public class RolasController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public RolasController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Rolas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rola>>> GetRolas()
        {
            return await _context.Rolas.ToListAsync();
        }

        // GET: api/Rolas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rola>> GetRola(int id)
        {
            var rola = await _context.Rolas.FindAsync(id);

            if (rola == null)
            {
                return NotFound();
            }

            return rola;
        }

        // PUT: api/Rolas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRola(int id, Rola rola)
        {
            if (id != rola.IdOceny2)
            {
                return BadRequest();
            }

            _context.Entry(rola).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolaExists(id))
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

        // POST: api/Rolas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rola>> PostRola(Rola rola)
        {
            _context.Rolas.Add(rola);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRola", new { id = rola.IdOceny2 }, rola);
        }

        // DELETE: api/Rolas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRola(int id)
        {
            var rola = await _context.Rolas.FindAsync(id);
            if (rola == null)
            {
                return NotFound();
            }

            _context.Rolas.Remove(rola);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolaExists(int id)
        {
            return _context.Rolas.Any(e => e.IdOceny2 == id);
        }
    }
}
