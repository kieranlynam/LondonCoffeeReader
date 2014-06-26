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
                new Cafe { Name = "Tina, I Salute You" },
                new Cafe { Name = "Shoreditch Grind" },
                new Cafe { Name = "Prufrock Coffee" }
            };

        public Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return Task.FromResult(AllCafes);
        }
    }
}
