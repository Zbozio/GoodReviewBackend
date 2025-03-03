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

    public async Task<List<KsiazkaDto>> GetBooksByUserIdAsync(int userId)
    {
        var books = await _context.Ocenas
            .Where(o => o.IdUzytkownik == userId) 
            .Include(o => o.IdKsiazkaNavigation)   
            .Select(o => new KsiazkaDto
            {
                IdKsiazka = o.IdKsiazkaNavigation.IdKsiazka,          
                Tytul = o.IdKsiazkaNavigation.Tytul,           
                Okladka = o.IdKsiazkaNavigation.Okladka,       
            })
            .ToListAsync();

        return books;
    }
}
