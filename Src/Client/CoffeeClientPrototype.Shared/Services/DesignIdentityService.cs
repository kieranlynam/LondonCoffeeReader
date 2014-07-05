using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.Services
{
    public class DesignIdentityService : IIdentityService
    {
        public const string CurrentUserIdentity = "Me";

        public string Id { get; private set; }

        public DesignIdentityService()
        {
            this.Id = CurrentUserIdentity;
        }
    }
}
