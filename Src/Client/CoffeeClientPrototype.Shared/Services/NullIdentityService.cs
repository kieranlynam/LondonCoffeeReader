using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.Services
{
    public class NullIdentityService : IIdentityService
    {
        public string Id { get; private set; }

        public NullIdentityService()
        {
            this.Id = null;
        }
    }
}
