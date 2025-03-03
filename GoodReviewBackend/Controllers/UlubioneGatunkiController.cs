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

    [HttpPost("AddToFavorites/{idUzytkownik}/{idGatunku}")]
    public async Task<IActionResult> AddToFavorites(int idUzytkownik, int idGatunku)
    {
        var user = await _dbContext.Uzytkowniks
            .Include(u => u.IdGatunkus) 
            .FirstOrDefaultAsync(u => u.IdUzytkownik == idUzytkownik);

        if (user == null)
        {
            return NotFound("Użytkownik nie znaleziony.");
        }

        var gatunekDoDodania = await _dbContext.Gatuneks
            .FirstOrDefaultAsync(g => g.IdGatunku == idGatunku);

        if (gatunekDoDodania == null)
        {
            return NotFound("Gatunek o podanym ID nie istnieje.");
        }

        if (user.IdGatunkus.Contains(gatunekDoDodania))
        {
            return BadRequest("Gatunek już jest w ulubionych.");
        }

        user.IdGatunkus.Add(gatunekDoDodania);

        await _dbContext.SaveChangesAsync();
        return Ok("Gatunek został dodany do ulubionych.");
    }

    [HttpGet("GetFavorites/{idUzytkownik}")]
    public async Task<IActionResult> GetFavorites(int idUzytkownik)
    {
        var user = await _dbContext.Uzytkowniks
            .Include(u => u.IdGatunkus) 
            .FirstOrDefaultAsync(u => u.IdUzytkownik == idUzytkownik);

        if (user == null)
        {
            return NotFound("Użytkownik nie znaleziony.");
        }

        var favorites = user.IdGatunkus.Select(g => new
        {
            g.IdGatunku,
            g.NazwaGatunku 
        }).ToList();

        return Ok(favorites);
    }

   
    [HttpDelete("RemoveFromFavorites/{idUzytkownik}/{idGatunku}")]
    public async Task<IActionResult> RemoveFromFavorites(int idUzytkownik, int idGatunku)
    {
        var user = await _dbContext.Uzytkowniks
            .Include(u => u.IdGatunkus) 
            .FirstOrDefaultAsync(u => u.IdUzytkownik == idUzytkownik);

        if (user == null)
        {
            return NotFound("Użytkownik nie znaleziony.");
        }

        
        var gatunekDoUsuniecia = await _dbContext.Gatuneks
            .FirstOrDefaultAsync(g => g.IdGatunku == idGatunku);

        if (gatunekDoUsuniecia == null)
        {
            return NotFound("Gatunek o podanym ID nie istnieje.");
        }

        
        if (!user.IdGatunkus.Contains(gatunekDoUsuniecia))
        {
            return NotFound("Gatunek nie znajduje się w ulubionych użytkownika.");
        }

        
        user.IdGatunkus.Remove(gatunekDoUsuniecia);

        await _dbContext.SaveChangesAsync();
        return Ok("Gatunek został usunięty z ulubionych.");
    }
}
