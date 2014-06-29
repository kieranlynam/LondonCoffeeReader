using CoffeeClientPrototype.ViewModel.Services;

namespace ViewModel.Tests
{
    public class MockIdentityService : IIdentityService
    {
        public string Id { get; set; }
    }
}
