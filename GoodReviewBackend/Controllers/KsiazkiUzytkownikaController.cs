using GoodReviewBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class KsiazkiUzytkownikaController : ControllerBase
{
    private readonly KsiazkiUzytkownikaService _ksiazkaService;

    public KsiazkiUzytkownikaController(KsiazkiUzytkownikaService ksiazkaService)
    {
        _ksiazkaService = ksiazkaService;
    }

    // Endpoint do pobrania książek użytkownika
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetBooksByUserId([FromRoute] int userId) // [FromRoute] wskazuje, że parametr jest w URL
    {
        var books = await _ksiazkaService.GetBooksByUserIdAsync(userId);
        return Ok(books);  // Zwracamy książki użytkownika
    }
}
