using System;
using System.Threading.Tasks;

namespace CoffeeClientPrototype.ViewModel.Services
{
    public interface IIdentityService
    {
        string CurrentUserId { get; }

        bool IsAuthenticated { get; }

        event EventHandler IsAuthenticatedChanged;

        Task<bool> AuthenticateAsync();
    }
}
