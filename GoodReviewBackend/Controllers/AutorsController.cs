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
    public class AutorsController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public AutorsController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutors()
        {
            return await _context.Autors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAutor(int id)
        {
            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            var booksWithDetails = await _context.Udzials
                .Where(u => u.IdAutora == id)
                .Join(_context.Ksiazkas,
                      u => u.IdKsiazka,
                      k => k.IdKsiazka,
                      (udzial, ksiazka) => new
                      {
                          IdKsiazka = ksiazka.IdKsiazka,
                          Tytul = ksiazka.Tytul,
                          Okladka = ksiazka.Okladka
                      })
                .ToListAsync();

            return Ok(new
            {
                IdAutora = autor.IdAutora,
                ImieAutora = autor.ImieAutora,
                NazwiskoAutora = autor.NazwiskoAutora,
                Wiek = autor.Wiek,
                DataUrodzenia = autor.DataUrodzenia,
                Opis = autor.Opis,
                DataSmierci = autor.DataSmierci,
                Pseudonim = autor.Pseudonim,
                Ksiazki = booksWithDetails
            });
        }



       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutor(int id, Autor autor)
        {
            if (id != autor.IdAutora)
            {
                return BadRequest();
            }

            _context.Entry(autor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorExists(id))
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
        public async Task<ActionResult<Autor>> PostAutor(Autor autor)
        {
            _context.Autors.Add(autor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAutor", new { id = autor.IdAutora }, autor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            _context.Autors.Remove(autor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Autor>>> SearchAuthors(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            var authors = await _context.Autors
                .Where(a => EF.Functions.Like(a.ImieAutora, $"%{query}%"))
                .ToListAsync();

            return Ok(authors);
        }

        private bool AutorExists(int id)
        {
            return _context.Autors.Any(e => e.IdAutora == id);
        }
    }
}
