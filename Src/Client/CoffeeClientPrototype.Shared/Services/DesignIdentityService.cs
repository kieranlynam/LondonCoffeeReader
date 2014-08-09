using System.Threading.Tasks;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.Services
{
    internal sealed class DesignIdentityService : IIdentityService
    {
        public const string CurrentUserIdentity = "Me";

        public string Id { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public DesignIdentityService()
        {
            this.Id = CurrentUserIdentity;
            this.IsAuthenticated = true;
        }
        
        public Task<bool> AuthenticateAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
