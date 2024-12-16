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

    // Metoda do haszowania hasła podczas rejestracji
    public string HashPassword(string password)
    {
        // Sprawdzamy, czy hasło nie jest puste
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Hasło nie może być puste.");

        // Hashujemy hasło przy użyciu bcrypt
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Możesz dodać inne metody do rejestracji użytkowników, gdzie będziesz wywoływać tę metodę
    public async Task RegisterUserAsync(Uzytkownik user)
    {
        // Hashowanie hasła przed zapisaniem użytkownika w bazie danych
        user.Haslo = HashPassword(user.Haslo);

        // Dodaj użytkownika do bazy danych
        _dbContext.Uzytkowniks.Add(user);
        await _dbContext.SaveChangesAsync();
    }
}
