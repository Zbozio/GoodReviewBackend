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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UzytkownikDTO>>> GetUzytkowniks()
        {
            var uzytkownicy = await _context.Uzytkowniks
                .Select(u => new UzytkownikDTO
                {
                    IdUzytkownik = u.IdUzytkownik,
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
                    DataUrodzenia = u.DataUrodzenia,
                     IloscOcenionychKsiazek = _context.Ocenas
                        .Where(o => o.IdUzytkownik == u.IdUzytkownik)
                        .Select(o => o.IdKsiazka)
                        .Distinct()
                        .Count(),
                })
                .ToListAsync();

            return Ok(uzytkownicy);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Uzytkownik>> GetUzytkownik(int id)
        {
            var uzytkownik = await _context.Uzytkowniks
                .Where(u => u.IdUzytkownik == id)
                .Select(u => new
                {
                    u.IdUzytkownik,
                    u.Imie,
                    u.Nazwisko,
                    u.EMail,
                    u.DataRejestracji,
                    u.Zdjecie,
                    u.Opis,
                    u.DataUrodzenia,
                    u.IdOceny2, 

                    IloscZnajomych = _context.Znajomis.Count(z => z.IdUzytkownik == u.IdUzytkownik || z.UzyIdUzytkownik == u.IdUzytkownik),
                    IloscOcenionychKsiazek = _context.Ocenas
                        .Where(o => o.IdUzytkownik == u.IdUzytkownik)
                        .Select(o => o.IdKsiazka)
                        .Distinct()
                        .Count(),
                    IloscRecenzji = _context.Recenzjas.Count(r => r.IdUzytkownik == u.IdUzytkownik)
                })
                .FirstOrDefaultAsync();

            if (uzytkownik == null)
            {
                return NotFound();
            }

            return Ok(uzytkownik);
        }
        [HttpGet("{id}/statistics")]
        public async Task<ActionResult> GetUserStatistics(int id)
        {
            var user = await _context.Uzytkowniks
                .Include(u => u.Ocenas)
                    .ThenInclude(o => o.IdKsiazkaNavigation)
                        .ThenInclude(k => k.IdGatunkus) 
                .FirstOrDefaultAsync(u => u.IdUzytkownik == id);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik nie został znaleziony." });
            }

            int iloscOcenionychKsiazek = user.Ocenas
                .Select(o => o.IdKsiazka)
                .Distinct()
                .Count();

            int iloscRecenzji = _context.Recenzjas.Count(r => r.IdUzytkownik == user.IdUzytkownik);

            double sredniaOcena = (double)(user.Ocenas.Any()
                ? user.Ocenas.Average(o => o.WartoscOceny)
                : 0.0);

            var ulubionyGatunek = user.Ocenas
                .SelectMany(o => o.IdKsiazkaNavigation.IdGatunkus) 
                .GroupBy(g => g.IdGatunku)
                .OrderByDescending(g => g.Count()) 
                .Select(g => new
                {
                    Gatunek = g.First().NazwaGatunku, 
                    LiczbaKsiazek = g.Count()
                })
                .FirstOrDefault(); 

            return Ok(new
            {
                IloscOcenionychKsiazek = iloscOcenionychKsiazek,
                IloscRecenzji = iloscRecenzji,
                SredniaOcena = sredniaOcena,
                UlubionyGatunek = ulubionyGatunek?.Gatunek ?? "Brak danych",
                LiczbaKsiazekWGatunku = ulubionyGatunek?.LiczbaKsiazek ?? 0
            });
        }






        [HttpPost]
        public async Task<ActionResult<UzytkownikDTO>> PostUzytkownik(UzytkownikDTO uzytkownikDto)
        {
            var uzytkownik = new Uzytkownik
            {

                IdUzytkownik=uzytkownikDto.IdUzytkownik,
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

            return CreatedAtAction("GetUzytkownik", new { id = uzytkownik.IdUzytkownik }, uzytkownikDto);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutUzytkownik(int id, UzytkownikDTO uzytkownikDto)
        {
            var uzytkownik = await _context.Uzytkowniks.FindAsync(id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            uzytkownik.IdUzytkownik = uzytkownikDto.IdUzytkownik;
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


        [HttpPut("updateProfile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, UpdateProfileRequest uzytkownikDto)
        {
            var uzytkownik = await _context.Uzytkowniks.FindAsync(id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(uzytkownikDto.Zdjecie))
            {
                uzytkownik.Zdjecie = uzytkownikDto.Zdjecie;
            }

            if (!string.IsNullOrEmpty(uzytkownikDto.Opis))
            {
                uzytkownik.Opis = uzytkownikDto.Opis;
            }

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

            return Ok(uzytkownik); 
        }

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

