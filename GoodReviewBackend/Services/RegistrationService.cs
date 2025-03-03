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
            string hashedPassword = _passwordHasherService.HashPassword(userDto.Haslo);

            var user = new Uzytkownik
            {
                Imie = userDto.Imie,
                Nazwisko = userDto.Nazwisko,
                EMail = userDto.EMail,
                DataRejestracji = userDto.DataRejestracji ?? DateTime.Now, 
                IloscOcen = userDto.IloscOcen ?? 0,
                IloscRecenzji = userDto.IloscRecenzji ?? 0,
                OstaniaAktywnosc = userDto.OstaniaAktywnosc ?? DateTime.Now, 
                Zdjecie = userDto.Zdjecie,
                Znajomi = userDto.Znajomi ?? 0,
                Opis = userDto.Opis,
                Haslo = hashedPassword, 
                DataUrodzenia=userDto.DataUrodzenia ?? DateTime.Now,
                IdOceny2 = 2 
            };

            _dbContext.Uzytkowniks.Add(user);
            await _dbContext.SaveChangesAsync();
            if (userDto.GatunkiIds != null && userDto.GatunkiIds.Any())
            {
                var selectedGenres = await _dbContext.Gatuneks
                    .Where(g => userDto.GatunkiIds.Contains(g.IdGatunku))
                    .ToListAsync();

                if (selectedGenres.Any())
                {
                    foreach (var genre in selectedGenres)
                    {
                        user.IdGatunkus.Add(genre); 
                    }

                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
