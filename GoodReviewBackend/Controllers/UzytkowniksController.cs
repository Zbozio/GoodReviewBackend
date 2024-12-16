using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodReviewBackend.Models;
using GoodReviewBackend.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodReviewBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UzytkowniksController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public UzytkowniksController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Uzytkowniks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UzytkownikDTO>>> GetUzytkowniks()
        {
            var uzytkownicy = await _context.Uzytkowniks
                .Select(u => new UzytkownikDTO
                {
                    
                    Imie = u.Imie,
                    Nazwisko = u.Nazwisko,
                    EMail = u.EMail,
                    DataRejestracji = u.DataRejestracji,
                    IloscOcen = u.IloscOcen,
                    IloscRecenzji = u.IloscRecenzji,
                    OstaniaAktywnosc = u.OstaniaAktywnosc,
                    Zdjecie = u.Zdjecie,
                    Znajomi = u.Znajomi,
                    Opis = u.Opis,
                    DataUrodzenia=u.DataUrodzenia
                })
                .ToListAsync();

            return Ok(uzytkownicy);
        }

        // GET: api/Uzytkowniks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UzytkownikDTO>> GetUzytkownik(int id)
        {
            var uzytkownik = await _context.Uzytkowniks
                .Where(u => u.IdUzytkownik == id)
                .Select(u => new UzytkownikDTO
                {
                    
                    Imie = u.Imie,
                    Nazwisko = u.Nazwisko,
                    EMail = u.EMail,
                    DataRejestracji = u.DataRejestracji,
                    IloscOcen = u.IloscOcen,
                    IloscRecenzji = u.IloscRecenzji,
                    OstaniaAktywnosc = u.OstaniaAktywnosc,
                    Zdjecie = u.Zdjecie,
                    Znajomi = u.Znajomi,
                    Opis = u.Opis,
                    DataUrodzenia=u.DataUrodzenia,
                })
                .FirstOrDefaultAsync();

            if (uzytkownik == null)
            {
                return NotFound();
            }

            return Ok(uzytkownik);
        }

        // POST: api/Uzytkowniks
        [HttpPost]
        public async Task<ActionResult<UzytkownikDTO>> PostUzytkownik(UzytkownikDTO uzytkownikDto)
        {
            // Tworzymy nowy obiekt Uzytkownika z DTO
            var uzytkownik = new Uzytkownik
            {
                Imie = uzytkownikDto.Imie,
                Nazwisko = uzytkownikDto.Nazwisko,
                EMail = uzytkownikDto.EMail,
                DataRejestracji = uzytkownikDto.DataRejestracji,
                IloscOcen = uzytkownikDto.IloscOcen,
                IloscRecenzji = uzytkownikDto.IloscRecenzji,
                OstaniaAktywnosc = uzytkownikDto.OstaniaAktywnosc,
                Zdjecie = uzytkownikDto.Zdjecie,
                Znajomi = uzytkownikDto.Znajomi,
                Opis = uzytkownikDto.Opis,
                DataUrodzenia=uzytkownikDto.DataUrodzenia
            };

            _context.Uzytkowniks.Add(uzytkownik);
            await _context.SaveChangesAsync();

            // Zwracamy utworzonego użytkownika
            return CreatedAtAction("GetUzytkownik", new { id = uzytkownik.IdUzytkownik }, uzytkownikDto);
        }

        // PUT: api/Uzytkowniks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUzytkownik(int id, UzytkownikDTO uzytkownikDto)
        {
            // Sprawdzamy, czy użytkownik istnieje
            var uzytkownik = await _context.Uzytkowniks.FindAsync(id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            // Aktualizujemy dane użytkownika
            uzytkownik.Imie = uzytkownikDto.Imie;
            uzytkownik.Nazwisko = uzytkownikDto.Nazwisko;
            uzytkownik.EMail = uzytkownikDto.EMail;
            uzytkownik.DataRejestracji = uzytkownikDto.DataRejestracji;
            uzytkownik.IloscOcen = uzytkownikDto.IloscOcen;
            uzytkownik.IloscRecenzji = uzytkownikDto.IloscRecenzji;
            uzytkownik.OstaniaAktywnosc = uzytkownikDto.OstaniaAktywnosc;
            uzytkownik.Zdjecie = uzytkownikDto.Zdjecie;
            uzytkownik.Znajomi = uzytkownikDto.Znajomi;
            uzytkownik.Opis = uzytkownikDto.Opis;

            _context.Entry(uzytkownik).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UzytkownikExists(id))
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

        // DELETE: api/Uzytkowniks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUzytkownik(int id)
        {
            var uzytkownik = await _context.Uzytkowniks.FindAsync(id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            _context.Uzytkowniks.Remove(uzytkownik);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UzytkownikExists(int id)
        {
            return _context.Uzytkowniks.Any(e => e.IdUzytkownik == id);
        }
    }
}
