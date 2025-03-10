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
    public class RecenzjasController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public RecenzjasController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRecenzjas()
        {
            var recenzje = await _context.Recenzjas
                .Include(r => r.IdUzytkownikNavigation) 
                .Include(r => r.IdKsiazkaNavigation)   
                .Select(r => new
                {
                    r.IdRecenzji,
                    r.TrescRecenzji,
                    r.DataRecenzji,
                    r.PolubieniaRecenzji,
                    UzytkownikImie = r.IdUzytkownikNavigation.Imie,
                    UzytkownikNazwisko = r.IdUzytkownikNavigation.Nazwisko,
                    UzytkownikZdjecie = r.IdUzytkownikNavigation.Zdjecie,
                    KsiazkaTytul = r.IdKsiazkaNavigation.Tytul,
                    KsiazkaOkladka = r.IdKsiazkaNavigation.Okladka
                })
                .ToListAsync();

            if (!recenzje.Any())
            {
                return NotFound("No reviews found.");
            }

            return Ok(recenzje);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRecenzja(int id)
        {
            var recenzja = await _context.Recenzjas
                .Include(r => r.IdUzytkownikNavigation) 
                .Include(r => r.IdKsiazkaNavigation)   
                .Where(r => r.IdRecenzji == id)
                .Select(r => new
                {
                    r.IdRecenzji,
                    r.TrescRecenzji,
                    r.DataRecenzji,
                    r.PolubieniaRecenzji,
                    UzytkownikImie = r.IdUzytkownikNavigation.Imie,
                    UzytkownikNazwisko = r.IdUzytkownikNavigation.Nazwisko,
                    UzytkownikZdjecie = r.IdUzytkownikNavigation.Zdjecie,
                    KsiazkaTytul = r.IdKsiazkaNavigation.Tytul,
                    KsiazkaOkladka = r.IdKsiazkaNavigation.Okladka
                })
                .FirstOrDefaultAsync();

            if (recenzja == null)
            {
                return NotFound($"Recenzja z ID {id} nie została znaleziona.");
            }

            return Ok(recenzja);
        }

        [HttpGet("Uzytkownik/{idUzytkownik}")]
        public async Task<ActionResult<IEnumerable<object>>> GetRecenzjeByUzytkownik(int idUzytkownik)
        {
            var recenzje = await _context.Recenzjas
    .Include(r => r.IdUzytkownikNavigation) 
    .Include(r => r.IdKsiazkaNavigation) 
    .Include(r => r.Komentarzs)
    .ThenInclude(k => k.IdUzytkownikNavigation) 
    .Where(r => r.IdUzytkownik == idUzytkownik)
    .Select(r => new
    {
        r.IdUzytkownik,
        r.IdKsiazka,
        r.IdRecenzji,
        r.TrescRecenzji,
        r.DataRecenzji,
        r.PolubieniaRecenzji,
        UzytkownikImie = r.IdUzytkownikNavigation.Imie,
        UzytkownikNazwisko = r.IdUzytkownikNavigation.Nazwisko,
        UzytkownikZdjecie = r.IdUzytkownikNavigation.Zdjecie,
        KsiazkaTytul = r.IdKsiazkaNavigation.Tytul,
        KsiazkaOkladka = r.IdKsiazkaNavigation.Okladka,
        Komentarze = r.Komentarzs.Select(k => new
        {
            k.TrescKomentarza,
            k.IdUzytkownik,
            UserImie = k.IdUzytkownikNavigation.Imie,
            UserNazwisko = k.IdUzytkownikNavigation.Nazwisko,
            UserZdjecie = k.IdUzytkownikNavigation.Zdjecie
        }).ToList() 
    }).ToListAsync();


            if (!recenzje.Any())
            {
                return NotFound($"No reviews found for user with ID {idUzytkownik}");
            }

            return Ok(recenzje);
        }

        [HttpGet("Ksiazka/{idKsiazka}")]
        public async Task<ActionResult<IEnumerable<object>>> GetRecenzjeByKsiazka(int idKsiazka)
        {
            var recenzje = await _context.Recenzjas
                .Include(r => r.IdUzytkownikNavigation) 
                .Include(r => r.IdKsiazkaNavigation)   
                .Where(r => r.IdKsiazka == idKsiazka)
                .Select(r => new
                {
                    r.IdRecenzji,
                    r.IdKsiazka,
                    r.IdUzytkownik,
                    r.TrescRecenzji,
                    r.DataRecenzji,
                    r.PolubieniaRecenzji,
                    UzytkownikImie = r.IdUzytkownikNavigation.Imie,
                    UzytkownikNazwisko = r.IdUzytkownikNavigation.Nazwisko,
                    UzytkownikZdjecie = r.IdUzytkownikNavigation.Zdjecie,
                    KsiazkaTytul = r.IdKsiazkaNavigation.Tytul,
                    KsiazkaOkladka = r.IdKsiazkaNavigation.Okladka
                })
                .ToListAsync();

            if (!recenzje.Any())
            {
                return NotFound($"No reviews found for book with ID {idKsiazka}");
            }

            return Ok(recenzje);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecenzja(int id, Recenzja recenzja)
        {
            if (id != recenzja.IdRecenzji)
            {
                return BadRequest();
            }

            _context.Entry(recenzja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecenzjaExists(id))
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
        public async Task<ActionResult<Recenzja>> PostRecenzja(Recenzja recenzja)
        {
            var existingReview = await _context.Recenzjas
                .FirstOrDefaultAsync(r => r.IdUzytkownik == recenzja.IdUzytkownik && r.IdKsiazka == recenzja.IdKsiazka);

            if (existingReview != null)
            {
                return BadRequest("Użytkownik już dodał recenzję dla tej książki.");
            }

            _context.Recenzjas.Add(recenzja);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecenzja", new { id = recenzja.IdRecenzji }, recenzja);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecenzja(int id)
        {
            var recenzja = await _context.Recenzjas.FindAsync(id);
            if (recenzja == null)
            {
                return NotFound();
            }

            _context.Recenzjas.Remove(recenzja);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecenzjaExists(int id)
        {
            return _context.Recenzjas.Any(e => e.IdRecenzji == id);
        }
    }
}