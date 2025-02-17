﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodReviewBackend.Models;

namespace GoodReviewBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcenianieController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public OcenianieController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // POST: api/Ocenianie
        [HttpPost]
        public async Task<IActionResult> RateBook([FromBody] Ocena ratingRequest)
        {
            // Walidacja wartości oceny
            if (ratingRequest.WartoscOceny < 1 || ratingRequest.WartoscOceny > 10)
            {
                return BadRequest("Rating value must be between 1 and 10.");
            }

            // Walidacja obecności wymaganych pól
            if (ratingRequest.IdKsiazka == null || ratingRequest.IdUzytkownik == null)
            {
                return BadRequest("Book ID and User ID must be provided.");
            }

            // Sprawdź, czy użytkownik już ocenił tę książkę
            var existingRating = await _context.Ocenas
                .FirstOrDefaultAsync(o => o.IdKsiazka == ratingRequest.IdKsiazka
                                           && o.IdUzytkownik == ratingRequest.IdUzytkownik);

            if (existingRating != null)
            {
                // Aktualizacja istniejącej oceny
                existingRating.WartoscOceny = ratingRequest.WartoscOceny;
                existingRating.DataOceny = DateTime.UtcNow;
                _context.Entry(existingRating).State = EntityState.Modified;
            }
            else
            {
                // Dodanie nowej oceny
                var newRating = new Ocena
                {
                    IdKsiazka = ratingRequest.IdKsiazka,
                    IdUzytkownik = ratingRequest.IdUzytkownik,
                    WartoscOceny = ratingRequest.WartoscOceny,
                    DataOceny = DateTime.UtcNow
                };

                _context.Ocenas.Add(newRating);
            }

            await _context.SaveChangesAsync();
            return Ok("Rating saved successfully.");
        }
        [HttpGet("{idKsiazka}/{idUzytkownik}")]
        public async Task<IActionResult> GetRating(int idKsiazka, int idUzytkownik)
        {
            // Sprawdź, czy ocena dla danej książki i użytkownika istnieje
            var rating = await _context.Ocenas
                .FirstOrDefaultAsync(o => o.IdKsiazka == idKsiazka && o.IdUzytkownik == idUzytkownik);

            if (rating == null)
            {
                return NotFound("No rating found for this user and book.");
            }

            return Ok(new
            {
                IdKsiazka = rating.IdKsiazka,
                IdUzytkownik = rating.IdUzytkownik,
                WartoscOceny = rating.WartoscOceny,
                DataOceny = rating.DataOceny
            });
        }


    }
}
