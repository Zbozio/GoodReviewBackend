using GoodReviewBackend.Models;
using GoodReviewBackend.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoodReviewBackend.Services
{
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
                DataUrodzenia=userDto.DataUrodzenia ?? DateTime.Now,
                IdOceny2 = 2 // Domyślna ocena
            };

            // Dodajemy użytkownika do bazy danych
            _dbContext.Uzytkowniks.Add(user);
            await _dbContext.SaveChangesAsync(); // Zapisujemy użytkownika, aby przypisać mu ID

            // Dodanie gatunków, jeśli są podane w DTO
            if (userDto.GatunkiIds != null && userDto.GatunkiIds.Any())
            {
                // Pobieramy wszystkie gatunki na podstawie ich ID
                var selectedGenres = await _dbContext.Gatuneks
                    .Where(g => userDto.GatunkiIds.Contains(g.IdGatunku))
                    .ToListAsync();

                // Jeśli gatunki istnieją, przypisujemy je do użytkownika
                if (selectedGenres.Any())
                {
                    // Przypisujemy gatunki do użytkownika poprzez kolekcję nawigacyjną
                    foreach (var genre in selectedGenres)
                    {
                        user.IdGatunkus.Add(genre); // Dodajemy gatunek do kolekcji nawigacyjnej użytkownika
                    }

                    // Zapisujemy zmiany w bazie
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
