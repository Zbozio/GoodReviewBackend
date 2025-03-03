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
    public class PoleceniumsController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public PoleceniumsController(GoodReviewDatabaseContext context)
        {
            _context = context;
        } 

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Polecenium>>> GetPolecenia()
        {
            return await _context.Polecenia.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Polecenium>> GetPolecenium(int id)
        {
            var polecenium = await _context.Polecenia.FindAsync(id);

            if (polecenium == null)
            {
                return NotFound();
            }

            return polecenium;
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPolecenium(int id, Polecenium polecenium)
        {
            if (id != polecenium.IdPolecenia)
            {
                return BadRequest();
            }

            _context.Entry(polecenium).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoleceniumExists(id))
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
        public async Task<ActionResult<Polecenium>> PostPolecenium(Polecenium polecenium)
        {
            _context.Polecenia.Add(polecenium);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPolecenium", new { id = polecenium.IdPolecenia }, polecenium);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolecenium(int id)
        {
            var polecenium = await _context.Polecenia.FindAsync(id);
            if (polecenium == null)
            {
                return NotFound();
            }

            _context.Polecenia.Remove(polecenium);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PoleceniumExists(int id)
        {
            return _context.Polecenia.Any(e => e.IdPolecenia == id);
        }
    }
}
