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

        [HttpGet("panel/{userId}")]
        [Authorize] 
        public async Task<IActionResult> GetAdminPanel(int userId)
        {
            var user = await _context.Uzytkowniks
                .Include(u => u.IdOceny2Navigation) 
                .FirstOrDefaultAsync(u => u.IdUzytkownik == userId);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik nie znaleziony." });
            }

            if (user.IdOceny2 == 1)
            {
                return Ok(new { Message = "Witaj w panelu administracyjnym!" });
            }
            else
            {
                return Forbid(); 
            }
        }
    }
}
