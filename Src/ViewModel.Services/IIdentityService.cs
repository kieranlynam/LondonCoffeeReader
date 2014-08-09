using System.Threading.Tasks;

namespace CoffeeClientPrototype.ViewModel.Services
{
    public interface IIdentityService
    {
        string Id { get; }

        bool IsAuthenticated { get; }

        Task<bool> AuthenticateAsync();
    }
}
