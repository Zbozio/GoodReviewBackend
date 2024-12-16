using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodReviewBackend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodReviewBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatunkowoscController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public GatunkowoscController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Gatunkowosc/ksiazka/{idKsiazka}
        [HttpGet("ksiazka/{idKsiazka}")]
        public async Task<ActionResult<IEnumerable<Gatunek>>> GetGatunkiForKsiazka(int idKsiazka)
        {
            var ksiazka = await _context.Ksiazkas
                .Include(k => k.IdGatunkus)
                .FirstOrDefaultAsync(k => k.IdKsiazka == idKsiazka);

            if (ksiazka == null)
            {
                return NotFound($"Książka o ID {idKsiazka} nie została znaleziona.");
            }

            return Ok(ksiazka.IdGatunkus); // Zwracamy gatunki przypisane do książki
        }

        // GET: api/Gatunkowosc/gatunek/{idGatunek}
        [HttpGet("gatunek/{idGatunek}")]
        public async Task<ActionResult<IEnumerable<Ksiazka>>> GetKsiazkiForGatunek(int idGatunek)
        {
            var gatunek = await _context.Gatuneks
                .Include(g => g.IdKsiazkas)
                .FirstOrDefaultAsync(g => g.IdGatunku == idGatunek);

            if (gatunek == null)
            {
                return NotFound($"Gatunek o ID {idGatunek} nie został znaleziony.");
            }

            return Ok(gatunek.IdKsiazkas); // Zwracamy książki przypisane do gatunku
        }

        // POST: Dodawanie gatunku do książki
        [HttpPost("ksiazka/{idKsiazka}/gatunek/{idGatunek}")]
        public async Task<IActionResult> AddGatunekToKsiazka(int idKsiazka, int idGatunek)
        {
            var ksiazka = await _context.Ksiazkas
                .Include(k => k.IdGatunkus)
                .FirstOrDefaultAsync(k => k.IdKsiazka == idKsiazka);

            if (ksiazka == null)
            {
                return NotFound($"Książka o ID {idKsiazka} nie została znaleziona.");
            }

            var gatunek = await _context.Gatuneks
                .FirstOrDefaultAsync(g => g.IdGatunku == idGatunek);

            if (gatunek == null)
            {
                return NotFound($"Gatunek o ID {idGatunek} nie został znaleziony.");
            }

            ksiazka.IdGatunkus.Add(gatunek);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: Usuwanie gatunku z książki
        [HttpDelete("ksiazka/{idKsiazka}/gatunek/{idGatunek}")]
        public async Task<IActionResult> RemoveGatunekFromKsiazka(int idKsiazka, int idGatunek)
        {
            var ksiazka = await _context.Ksiazkas
                .Include(k => k.IdGatunkus)
                .FirstOrDefaultAsync(k => k.IdKsiazka == idKsiazka);

            if (ksiazka == null)
            {
                return NotFound($"Książka o ID {idKsiazka} nie została znaleziona.");
            }

            var gatunek = ksiazka.IdGatunkus
                .FirstOrDefault(g => g.IdGatunku == idGatunek);

            if (gatunek == null)
            {
                return NotFound($"Gatunek o ID {idGatunek} nie jest przypisany do tej książki.");
            }

            ksiazka.IdGatunkus.Remove(gatunek);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
