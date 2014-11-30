using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.ViewModel.Services;

namespace ViewModel.Tests
{
    internal sealed class MockBookmarkService : IBookmarkService
    {
        public List<string> Bookmarks { get; private set; }

        public MockBookmarkService()
        {
            this.Bookmarks = new List<string>();
        }

        public Task<IEnumerable<string>> GetBookmarkedCafeIds()
        {
            return Task.FromResult(this.Bookmarks.AsEnumerable());
        }
    }
}
