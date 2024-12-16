using GoodReviewBackend.Models;
using GoodReviewBackend.DTOs;
using GoodReviewBackend.Services;
using System.Threading.Tasks;

public class RegistrationService
{
    private readonly GoodReviewDatabaseContext _dbContext;
    private readonly PasswordHasherService _passwordHasherService;

    public RegistrationService(GoodReviewDatabaseContext dbContext, PasswordHasherService passwordHasherService)
    {
        _dbContext = dbContext;
        _passwordHasherService = passwordHasherService;
    }

    public async Task RegisterUserAsync(UzytkownikDTO userDto)
    {
        // Hashowanie hasła przed zapisaniem użytkownika
        string hashedPassword = _passwordHasherService.HashPassword(userDto.Haslo);

        // Mapowanie DTO do modelu Uzytkownik
        var user = new Uzytkownik
        {
            Imie = userDto.Imie,
            Nazwisko = userDto.Nazwisko,
            EMail = userDto.EMail,
            DataRejestracji = userDto.DataRejestracji ?? DateTime.Now, // Jeśli brak daty rejestracji, ustawiamy obecną datę
            IloscOcen = userDto.IloscOcen ?? 0,
            IloscRecenzji = userDto.IloscRecenzji ?? 0,
            OstaniaAktywnosc = userDto.OstaniaAktywnosc ?? DateTime.Now, // Jeżeli brak, ustawiamy obecny czas
            Zdjecie = userDto.Zdjecie,
            Znajomi = userDto.Znajomi ?? 0,
            Opis = userDto.Opis,
            Haslo = hashedPassword, // Zapisujemy zahaszowane hasło
            IdOceny2 =  2
        };

        // Dodaj użytkownika do bazy danych
        _dbContext.Uzytkowniks.Add(user);
        await _dbContext.SaveChangesAsync();
    }
}
