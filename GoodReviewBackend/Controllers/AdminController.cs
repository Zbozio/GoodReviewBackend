using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodReviewBackend.Models;
using System.Linq;
using System.Threading.Tasks;

namespace GoodReviewBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public AdminController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // Metoda sprawdzająca, czy użytkownik ma rolę admina
        [HttpGet("panel/{userId}")]
        [Authorize] // Tylko zalogowani użytkownicy mogą uzyskać dostęp
        public async Task<IActionResult> GetAdminPanel(int userId)
        {
            // Jeśli chcesz sprawdzić rolę użytkownika w bazie, możesz pobrać jego dane na podstawie userId przekazanego w URL
            var user = await _context.Uzytkowniks
                .Include(u => u.IdOceny2Navigation) // Załóżmy, że masz relację z tabelą Rola
                .FirstOrDefaultAsync(u => u.IdUzytkownik == userId);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik nie znaleziony." });
            }

            // Sprawdzenie, czy użytkownik ma rolę 'admin' (IdOceny2 == 1)
            if (user.IdOceny2 == 1)
            {
                // Jeśli użytkownik jest adminem, udzielamy dostępu do panelu admina
                return Ok(new { Message = "Witaj w panelu administracyjnym!" });
            }
            else
            {
                // Jeśli użytkownik nie jest adminem, odmawiamy dostępu
                return Forbid(); // Zwraca 403 Forbidden
            }
        }
    }
}
