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
    public class ListumsController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public ListumsController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listum>>> GetLista()
        {
            return await _context.Lista.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Listum>> GetListum(int id)
        {
            var listum = await _context.Lista.FindAsync(id);

            if (listum == null)
            {
                return NotFound();
            }

            return listum;
        }

        // PUT: api/Listums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutListum(int id, Listum listum)
        {
            if (id != listum.IdListy)
            {
                return BadRequest();
            }

            _context.Entry(listum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListumExists(id))
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

        // POST: api/Listums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Listum>> PostListum(Listum listum)
        {
            _context.Lista.Add(listum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetListum", new { id = listum.IdListy }, listum);
        }

        // DELETE: api/Listums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListum(int id)
        {
            var listum = await _context.Lista.FindAsync(id);
            if (listum == null)
            {
                return NotFound();
            }

            _context.Lista.Remove(listum);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // GET: api/Listums/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Listum>>> GetListsForUser(int userId)
        {
            var userLists = await _context.Lista
                                          .Where(l => l.IdUzytkownik == userId)
                                          .ToListAsync();

            if (userLists == null || !userLists.Any())
            {
                return NotFound($"No lists found for user with ID: {userId}");
            }

            return userLists;
        }


        private bool ListumExists(int id)
        {
            return _context.Lista.Any(e => e.IdListy == id);
        }
    }
}
