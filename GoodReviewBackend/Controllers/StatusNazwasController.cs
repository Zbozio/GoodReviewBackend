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
    public class StatusNazwasController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public StatusNazwasController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusNazwa>>> GetStatusNazwas()
        {
            return await _context.StatusNazwas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StatusNazwa>> GetStatusNazwa(int id)
        {
            var statusNazwa = await _context.StatusNazwas.FindAsync(id);

            if (statusNazwa == null)
            {
                return NotFound();
            }

            return statusNazwa;
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatusNazwa(int id, StatusNazwa statusNazwa)
        {
            if (id != statusNazwa.IdStatusNazwa)
            {
                return BadRequest();
            }

            _context.Entry(statusNazwa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusNazwaExists(id))
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
        public async Task<ActionResult<StatusNazwa>> PostStatusNazwa(StatusNazwa statusNazwa)
        {
            _context.StatusNazwas.Add(statusNazwa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatusNazwa", new { id = statusNazwa.IdStatusNazwa }, statusNazwa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatusNazwa(int id)
        {
            var statusNazwa = await _context.StatusNazwas.FindAsync(id);
            if (statusNazwa == null)
            {
                return NotFound();
            }

            _context.StatusNazwas.Remove(statusNazwa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatusNazwaExists(int id)
        {
            return _context.StatusNazwas.Any(e => e.IdStatusNazwa == id);
        }
    }
}
