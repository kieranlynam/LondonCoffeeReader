using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeClientPrototype.ViewModel
{
    public interface INavigationListener
    {
        Task OnNavigatedTo(IDictionary<string, object> parameters);
    }
}