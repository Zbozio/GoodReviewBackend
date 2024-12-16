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
    public class ZnajomisController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public ZnajomisController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Znajomis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Znajomi>>> GetZnajomis()
        {
            return await _context.Znajomis.ToListAsync();
        }

        // GET: api/Znajomis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Znajomi>> GetZnajomi(int id)
        {
            var znajomi = await _context.Znajomis.FindAsync(id);

            if (znajomi == null)
            {
                return NotFound();
            }

            return znajomi;
        }

        // PUT: api/Znajomis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZnajomi(int id, Znajomi znajomi)
        {
            if (id != znajomi.IdZnajomosci)
            {
                return BadRequest();
            }

            _context.Entry(znajomi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZnajomiExists(id))
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

        // POST: api/Znajomis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Znajomi>> PostZnajomi(Znajomi znajomi)
        {
            _context.Znajomis.Add(znajomi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetZnajomi", new { id = znajomi.IdZnajomosci }, znajomi);
        }

        // DELETE: api/Znajomis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZnajomi(int id)
        {
            var znajomi = await _context.Znajomis.FindAsync(id);
            if (znajomi == null)
            {
                return NotFound();
            }

            _context.Znajomis.Remove(znajomi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ZnajomiExists(int id)
        {
            return _context.Znajomis.Any(e => e.IdZnajomosci == id);
        }
    }
}
