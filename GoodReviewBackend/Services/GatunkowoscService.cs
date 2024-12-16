using GoodReviewBackend.Models;
using Microsoft.EntityFrameworkCore;


public class GatunkowoscService : IGatunkowoscService
{
    private readonly GoodReviewDatabaseContext _context;

    public GatunkowoscService(GoodReviewDatabaseContext context)
    {
        _context = context;
    }

    // Pobieranie gatunków dla książki
    public async Task<IEnumerable<Gatunek>> GetGatunkiForKsiazkaAsync(int idKsiazka)
    {
        var ksiazka = await _context.Ksiazkas
            .Include(k => k.IdGatunkus) // Ładowanie powiązanych gatunków
            .FirstOrDefaultAsync(k => k.IdKsiazka == idKsiazka);

        return ksiazka?.IdGatunkus ?? new List<Gatunek>();
    }

    // Dodawanie gatunku do książki
    public async Task AddGatunekToKsiazkaAsync(int idKsiazka, int idGatunek)
    {
        var ksiazka = await _context.Ksiazkas
            .Include(k => k.IdGatunkus)
            .FirstOrDefaultAsync(k => k.IdKsiazka == idKsiazka);

        var gatunek = await _context.Gatuneks
            .FirstOrDefaultAsync(g => g.IdGatunku == idGatunek);

        if (ksiazka != null && gatunek != null)
        {
            ksiazka.IdGatunkus.Add(gatunek); // Dodawanie gatunku do książki
            await _context.SaveChangesAsync();
        }
    }

    // Usuwanie gatunku z książki
    public async Task RemoveGatunekFromKsiazkaAsync(int idKsiazka, int idGatunek)
    {
        var ksiazka = await _context.Ksiazkas
            .Include(k => k.IdGatunkus)
            .FirstOrDefaultAsync(k => k.IdKsiazka == idKsiazka);

        var gatunek = ksiazka?.IdGatunkus
            .FirstOrDefault(g => g.IdGatunku == idGatunek);

        if (gatunek != null)
        {
            ksiazka.IdGatunkus.Remove(gatunek); // Usuwanie gatunku z książki
            await _context.SaveChangesAsync();
        }
    }
}
