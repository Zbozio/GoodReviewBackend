﻿using System;
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
    public class KomentarzsController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public KomentarzsController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Komentarzs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Komentarz>>> GetKomentarzs()
        {
            return await _context.Komentarzs.ToListAsync();
        }

        // GET: api/Komentarzs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Komentarz>> GetKomentarz(int id)
        {
            var komentarz = await _context.Komentarzs.FindAsync(id);

            if (komentarz == null)
            {
                return NotFound();
            }

            return komentarz;
        }

        // PUT: api/Komentarzs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKomentarz(int id, Komentarz komentarz)
        {
            if (id != komentarz.IdOceny3)
            {
                return BadRequest();
            }

            _context.Entry(komentarz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KomentarzExists(id))
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

        // POST: api/Komentarzs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Komentarz>> PostKomentarz(Komentarz komentarz)
        {
            _context.Komentarzs.Add(komentarz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKomentarz", new { id = komentarz.IdOceny3 }, komentarz);
        }

        // DELETE: api/Komentarzs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKomentarz(int id)
        {
            var komentarz = await _context.Komentarzs.FindAsync(id);
            if (komentarz == null)
            {
                return NotFound();
            }

            _context.Komentarzs.Remove(komentarz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KomentarzExists(int id)
        {
            return _context.Komentarzs.Any(e => e.IdOceny3 == id);
        }
    }
}
