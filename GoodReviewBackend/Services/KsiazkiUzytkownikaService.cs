using GoodReviewBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class KsiazkiUzytkownikaService
{
    private readonly GoodReviewDatabaseContext _context;

    public KsiazkiUzytkownikaService(GoodReviewDatabaseContext context)
    {
        _context = context;
    }

    // Przykładowa metoda do pobierania książek z ocenami użytkownika
    public async Task<List<KsiazkaDto>> GetBooksByUserIdAsync(int userId)
    {
        var books = await _context.Ocenas
            .Where(o => o.IdUzytkownik == userId)  // Filtrujemy książki przypisane do danego użytkownika
            .Include(o => o.IdKsiazkaNavigation)   // Łączymy z tabelą książek (IdKsiazkaNavigation to nawigacja do książki)
            .Select(o => new KsiazkaDto
            {
                IdKsiazka = o.IdKsiazkaNavigation.IdKsiazka,           // Identyfikator książki
                Tytul = o.IdKsiazkaNavigation.Tytul,            // Tytuł książki
                Okladka = o.IdKsiazkaNavigation.Okladka,        // Okładka książki (jeśli istnieje)
            })
            .ToListAsync();

        return books;
    }
}
