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

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _dbContext.Uzytkowniks.FirstOrDefaultAsync(u => u.EMail == email);
        if (user == null || !VerifyPassword(password, user.Haslo)) 
        {
            return null; 
        }

        var token = GenerateJwtToken(user);
        return token;
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    private string GenerateJwtToken(Uzytkownik user)
    {
        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.IdUzytkownik.ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.EMail),
            new System.Security.Claims.Claim("role", user.IdOceny2.ToString())

        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtIssuer,     
            audience: _jwtAudience, 
            claims: claims,          
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds  
        );

        return new JwtSecurityTokenHandler().WriteToken(token); 
    }
}
