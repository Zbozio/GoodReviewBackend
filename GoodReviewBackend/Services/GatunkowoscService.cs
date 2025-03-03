using GoodReviewBackend.Models;
using Microsoft.EntityFrameworkCore;


public class GatunkowoscService : IGatunkowoscService
{
    private readonly GoodReviewDatabaseContext _context;

    public GatunkowoscService(GoodReviewDatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Gatunek>> GetGatunkiForKsiazkaAsync(int idKsiazka)
    {
        var ksiazka = await _context.Ksiazkas
            .Include(k => k.IdGatunkus)
            .FirstOrDefaultAsync(k => k.IdKsiazka == idKsiazka);

        return ksiazka?.IdGatunkus ?? new List<Gatunek>();
    }

    public async Task AddGatunekToKsiazkaAsync(int idKsiazka, int idGatunek)
    {
        var ksiazka = await _context.Ksiazkas
            .Include(k => k.IdGatunkus)
            .FirstOrDefaultAsync(k => k.IdKsiazka == idKsiazka);

        var gatunek = await _context.Gatuneks
            .FirstOrDefaultAsync(g => g.IdGatunku == idGatunek);

        if (ksiazka != null && gatunek != null)
        {
            ksiazka.IdGatunkus.Add(gatunek);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveGatunekFromKsiazkaAsync(int idKsiazka, int idGatunek)
    {
        var ksiazka = await _context.Ksiazkas
            .Include(k => k.IdGatunkus)
            .FirstOrDefaultAsync(k => k.IdKsiazka == idKsiazka);

        var gatunek = ksiazka?.IdGatunkus
            .FirstOrDefault(g => g.IdGatunku == idGatunek);

        if (gatunek != null)
        {
            ksiazka.IdGatunkus.Remove(gatunek); 
            await _context.SaveChangesAsync();
        }
    }
}
