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
    public class TypAutorstwasController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public TypAutorstwasController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypAutorstwa>>> GetTypAutorstwas()
        {
            return await _context.TypAutorstwas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TypAutorstwa>> GetTypAutorstwa(int id)
        {
            var typAutorstwa = await _context.TypAutorstwas.FindAsync(id);

            if (typAutorstwa == null)
            {
                return NotFound();
            }

            return typAutorstwa;
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypAutorstwa(int id, TypAutorstwa typAutorstwa)
        {
            if (id != typAutorstwa.IdTypu)
            {
                return BadRequest();
            }

            _context.Entry(typAutorstwa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypAutorstwaExists(id))
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
        public async Task<ActionResult<TypAutorstwa>> PostTypAutorstwa(TypAutorstwa typAutorstwa)
        {
            _context.TypAutorstwas.Add(typAutorstwa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypAutorstwa", new { id = typAutorstwa.IdTypu }, typAutorstwa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypAutorstwa(int id)
        {
            var typAutorstwa = await _context.TypAutorstwas.FindAsync(id);
            if (typAutorstwa == null)
            {
                return NotFound();
            }

            _context.TypAutorstwas.Remove(typAutorstwa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypAutorstwaExists(int id)
        {
            return _context.TypAutorstwas.Any(e => e.IdTypu == id);
        }
    }
}
