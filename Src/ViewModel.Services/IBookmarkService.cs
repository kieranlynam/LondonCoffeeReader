using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeClientPrototype.ViewModel.Services
{
    public interface IBookmarkService
    {
        Task<IEnumerable<string>> GetBookmarkedCafeIds();
    }
}
