using GoodReviewBackend.Models;

public interface IGatunkowoscService
{
    Task<IEnumerable<Gatunek>> GetGatunkiForKsiazkaAsync(int idKsiazka);
    Task AddGatunekToKsiazkaAsync(int idKsiazka, int idGatunek);
    Task RemoveGatunekFromKsiazkaAsync(int idKsiazka, int idGatunek);
}
