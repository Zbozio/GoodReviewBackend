using GoodReviewBackend.Models;
using BCrypt.Net;
using System.Threading.Tasks;

public class PasswordHasherService
{
    private readonly GoodReviewDatabaseContext _dbContext;

    public PasswordHasherService(GoodReviewDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Hasło nie może być puste.");

        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public async Task RegisterUserAsync(Uzytkownik user)
    {
        user.Haslo = HashPassword(user.Haslo);

        _dbContext.Uzytkowniks.Add(user);
        await _dbContext.SaveChangesAsync();
    }
}
