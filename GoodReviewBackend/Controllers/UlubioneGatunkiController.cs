using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodReviewBackend.Models;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UlubioneGatunkiController : ControllerBase
{
    private readonly GoodReviewDatabaseContext _dbContext;

    public UlubioneGatunkiController(GoodReviewDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Endpoint do dodawania gatunków do ulubionych
    [HttpPost("AddToFavorites/{idUzytkownik}/{idGatunku}")]
    public async Task<IActionResult> AddToFavorites(int idUzytkownik, int idGatunku)
    {
        // Pobieramy użytkownika na podstawie ID
        var user = await _dbContext.Uzytkowniks
            .Include(u => u.IdGatunkus)  // Pobieramy gatunki użytkownika
            .FirstOrDefaultAsync(u => u.IdUzytkownik == idUzytkownik);

        if (user == null)
        {
            return NotFound("Użytkownik nie znaleziony.");
        }

        // Pobieramy gatunek na podstawie ID
        var gatunekDoDodania = await _dbContext.Gatuneks
            .FirstOrDefaultAsync(g => g.IdGatunku == idGatunku);

        if (gatunekDoDodania == null)
        {
            return NotFound("Gatunek o podanym ID nie istnieje.");
        }

        // Sprawdzamy, czy użytkownik już dodał ten gatunek do ulubionych
        if (user.IdGatunkus.Contains(gatunekDoDodania))
        {
            return BadRequest("Gatunek już jest w ulubionych.");
        }

        // Dodajemy gatunek do ulubionych użytkownika
        user.IdGatunkus.Add(gatunekDoDodania);

        await _dbContext.SaveChangesAsync();
        return Ok("Gatunek został dodany do ulubionych.");
    }

    // Endpoint do pobierania ulubionych gatunków użytkownika
    [HttpGet("GetFavorites/{idUzytkownik}")]
    public async Task<IActionResult> GetFavorites(int idUzytkownik)
    {
        var user = await _dbContext.Uzytkowniks
            .Include(u => u.IdGatunkus) // Pobieramy gatunki użytkownika
            .FirstOrDefaultAsync(u => u.IdUzytkownik == idUzytkownik);

        if (user == null)
        {
            return NotFound("Użytkownik nie znaleziony.");
        }

        // Zwracamy tylko gatunki użytkownika
        var favorites = user.IdGatunkus.Select(g => new
        {
            g.IdGatunku,
            g.NazwaGatunku // Wybieramy tylko te dane, które chcemy zwrócić
        }).ToList();

        return Ok(favorites);
    }

    // Endpoint do usuwania gatunków z ulubionych (metoda DELETE)
    [HttpDelete("RemoveFromFavorites/{idUzytkownik}/{idGatunku}")]
    public async Task<IActionResult> RemoveFromFavorites(int idUzytkownik, int idGatunku)
    {
        // Pobieramy użytkownika na podstawie ID
        var user = await _dbContext.Uzytkowniks
            .Include(u => u.IdGatunkus)  // Pobieramy gatunki użytkownika
            .FirstOrDefaultAsync(u => u.IdUzytkownik == idUzytkownik);

        if (user == null)
        {
            return NotFound("Użytkownik nie znaleziony.");
        }

        // Pobieramy gatunek na podstawie ID
        var gatunekDoUsuniecia = await _dbContext.Gatuneks
            .FirstOrDefaultAsync(g => g.IdGatunku == idGatunku);

        if (gatunekDoUsuniecia == null)
        {
            return NotFound("Gatunek o podanym ID nie istnieje.");
        }

        // Sprawdzamy, czy użytkownik ma dany gatunek w swoich ulubionych
        if (!user.IdGatunkus.Contains(gatunekDoUsuniecia))
        {
            return NotFound("Gatunek nie znajduje się w ulubionych użytkownika.");
        }

        // Usuwamy gatunek z ulubionych
        user.IdGatunkus.Remove(gatunekDoUsuniecia);

        await _dbContext.SaveChangesAsync();
        return Ok("Gatunek został usunięty z ulubionych.");
    }
}
