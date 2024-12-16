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
    public class UdzialsController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public UdzialsController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Udzials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Udzial>>> GetUdzials()
        {
            return await _context.Udzials.ToListAsync();
        }

        // GET: api/Udzials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Udzial>> GetUdzial(int id)
        {
            var udzial = await _context.Udzials.FindAsync(id);

            if (udzial == null)
            {
                return NotFound();
            }

            return udzial;
        }

        // PUT: api/Udzials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUdzial(int id, Udzial udzial)
        {
            if (id != udzial.IdUdzialu)
            {
                return BadRequest();
            }

            _context.Entry(udzial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UdzialExists(id))
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

        // POST: api/Udzials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Udzial>> PostUdzial(Udzial udzial)
        {
            _context.Udzials.Add(udzial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUdzial", new { id = udzial.IdUdzialu }, udzial);
        }

        // DELETE: api/Udzials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUdzial(int id)
        {
            var udzial = await _context.Udzials.FindAsync(id);
            if (udzial == null)
            {
                return NotFound();
            }

            _context.Udzials.Remove(udzial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UdzialExists(int id)
        {
            return _context.Udzials.Any(e => e.IdUdzialu == id);
        }
    }
}
