using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.List;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModel.Tests.List
{
    [TestClass]
    public class MapViewModelTests
    {
        [TestMethod]
        public async Task CafesPopulatedWhenNavigatedTo()
        {
            using (var context = new Context())
            {
                for (int id = 0; id < 50; id++)
                {
                    context.Cafes.Add(new Cafe { Id = id, Name = id.ToString() });
                }

                await context.ViewModel.OnNavigatedTo(new Dictionary<string, object>());

                var expectedNames = Enumerable.Range(0, 50).Select(i => i.ToString()).ToArray();
                var actualNames = context.ViewModel.Cafes.Select(c => c.Name).ToArray();
                CollectionAssert.AreEqual(expectedNames, actualNames);
            }
        }

        private class Context : BaseTestContext
        {
            public MapViewModel ViewModel { get; private set; }

            public Context()
            {
                this.ViewModel = new MapViewModel(this.NavigationService, this.DataService);
            }
        }
    }
}
