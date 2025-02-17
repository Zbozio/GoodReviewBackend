using GoodReviewBackend.Models;
using GoodReviewBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

public class AuthService : IAuthService
{
    private readonly GoodReviewDatabaseContext _dbContext;
    private readonly string _jwtKey;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;

    public AuthService(GoodReviewDatabaseContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _jwtKey = configuration["Jwt:Key"];
        _jwtIssuer = configuration["Jwt:Issuer"];
        _jwtAudience = configuration["Jwt:Audience"];
    }

    // Metoda logowania użytkownika
    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _dbContext.Uzytkowniks.FirstOrDefaultAsync(u => u.EMail == email);
        if (user == null || !VerifyPassword(password, user.Haslo)) // Sprawdzamy email i hasło
        {
            return null; // Jeśli dane są nieprawidłowe, zwróć null (nie ma tokenu)
        }

        // Generujemy token JWT
        var token = GenerateJwtToken(user);
        return token;
    }

    // Weryfikacja poprawności hasła
    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword); // Weryfikujemy hasło za pomocą BCrypt
    }

    // Generowanie tokenu JWT
    private string GenerateJwtToken(Uzytkownik user)
    {
        // Definiowanie claims, które będą zawarte w tokenie
        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.IdUzytkownik.ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.EMail),
            new System.Security.Claims.Claim("role", user.IdOceny2.ToString()) // Dodanie roli (ID roli użytkownika)

        };

        // Konfiguracja klucza i podpisu tokenu
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Generowanie tokenu
        var token = new JwtSecurityToken(
            issuer: _jwtIssuer,      // Kto wystawił token
            audience: _jwtAudience,  // Kto jest odbiorcą tokenu
            claims: claims,          // Informacje o użytkowniku
            expires: DateTime.Now.AddHours(1), // Ważność tokenu (1 godzina)
            signingCredentials: creds  // Podpisanie tokenu
        );

        return new JwtSecurityTokenHandler().WriteToken(token); // Zwracamy wygenerowany token w postaci string
    }
}
