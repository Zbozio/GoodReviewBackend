
namespace GoodReviewBackend.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);

    }
}
