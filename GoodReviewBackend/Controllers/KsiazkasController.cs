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
    public class KsiazkasController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public KsiazkasController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Ksiazkas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ksiazka>>> GetKsiazkas()
        {
            return await _context.Ksiazkas.ToListAsync();
        }

        // GET: api/Ksiazkas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ksiazka>> GetKsiazka(int id)
        {
            var ksiazka = await _context.Ksiazkas.FindAsync(id);

            if (ksiazka == null)
            {
                return NotFound();
            }

            return ksiazka;
        }

        // PUT: api/Ksiazkas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKsiazka(int id, Ksiazka ksiazka)
        {
            if (id != ksiazka.IdKsiazka)
            {
                return BadRequest();
            }

            _context.Entry(ksiazka).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KsiazkaExists(id))
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

        // POST: api/Ksiazkas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ksiazka>> PostKsiazka(Ksiazka ksiazka)
        {
            _context.Ksiazkas.Add(ksiazka);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKsiazka", new { id = ksiazka.IdKsiazka }, ksiazka);
        }


        // DELETE: api/Ksiazkas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKsiazka(int id)
        {
            var ksiazka = await _context.Ksiazkas.FindAsync(id);
            if (ksiazka == null)
            {
                return NotFound();
            }

            _context.Ksiazkas.Remove(ksiazka);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KsiazkaExists(int id)
        {
            return _context.Ksiazkas.Any(e => e.IdKsiazka == id);
        }
    }
}
