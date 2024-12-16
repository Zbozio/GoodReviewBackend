using GoodReviewBackend.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // Endpoint do logowania
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var token = await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);

        if (token == null)
        {
            return Unauthorized("Niepoprawny email lub hasło.");
        }

        return Ok(new { Token = token });
    }
}
