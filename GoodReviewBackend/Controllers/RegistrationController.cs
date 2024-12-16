using GoodReviewBackend.Models;
using GoodReviewBackend.DTOs;
using GoodReviewBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RegisterController : ControllerBase
{
    private readonly RegistrationService _registrationService;

    public RegisterController(RegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    // Endpoint do rejestracji
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UzytkownikDTO userDto)
    {
        // Zabezpieczenie przed pustym emailem lub hasłem
        if (string.IsNullOrEmpty(userDto.EMail) || string.IsNullOrEmpty(userDto.Haslo))
        {
            return BadRequest("Email i hasło są wymagane.");
        }

        // Rejestracja użytkownika
        await _registrationService.RegisterUserAsync(userDto);

        return Ok(new { message = "Użytkownik został zarejestrowany!" });
    }
}
