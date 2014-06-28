using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype
{
    internal class DesignDataService : IDataService
    {
        private readonly static IEnumerable<Cafe> AllCafes = 
            new[]
            {
                new Cafe { Id = 1, Name = "Tina, I Salute You" },
                new Cafe { Id = 2, Name = "Shoreditch Grind" },
                new Cafe { Id = 3, Name = "Prufrock Coffee" }
            };

        public Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return Task.FromResult(AllCafes);
        }
    }
}
