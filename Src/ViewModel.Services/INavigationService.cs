using System.Collections.Generic;

namespace CoffeeClientPrototype.ViewModel.Services
{
    public interface INavigationService
    {
        void Navigate(string destination, IDictionary<string, object> parameters = null);
    }
}
