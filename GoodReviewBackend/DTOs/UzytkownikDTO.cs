using GoodReviewBackend.Models;

namespace GoodReviewBackend.DTOs
{
    public class UzytkownikDTO
    {
        
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public string? EMail { get; set; }
        public DateTime? DataRejestracji { get; set; }
        public int? IloscOcen { get; set; }
        public int? IloscRecenzji { get; set; }
        public DateTime? OstaniaAktywnosc { get; set; }
        public string? Zdjecie { get; set; }
        public int? Znajomi { get; set; }
        public string? Opis { get; set; }

        public int? IdOceny2 { get; set; }

        // Tylko dla rejestracji użytkownika
        public string? Haslo { get; set; }
        public DateTime? DataUrodzenia { get; set; }

            public List<int> GatunkiIds { get; set; } // Lista ID wybranych gatunków

    }
}
