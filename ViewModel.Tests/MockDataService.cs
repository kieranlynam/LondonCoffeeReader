using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;

namespace ViewModel.Tests
{
    public class MockDataService : IDataService
    {
        private readonly Func<IEnumerable<Cafe>> getCafes;

        public MockDataService(Func<IEnumerable<Cafe>> getCafes = null)
        {
            this.getCafes = getCafes ?? (Enumerable.Empty<Cafe>);
        }

        public async Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return this.getCafes();
        }
    }
}
