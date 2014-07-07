using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.List;
using CoffeeClientPrototype.ViewModel.Services;
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

        [TestMethod]
        public async Task CentrePopulatedFromGeolocationProviderWhenNavigatedTo()
        {
            using (var context = new Context())
            {
                context.GeolocationProvider.CurrentLocation = new Coordinate(45.54, 12.21);

                await context.ViewModel.OnNavigatedTo(new Dictionary<string, object>());

                Assert.AreEqual(45.54, context.ViewModel.Centre.Latitude);
                Assert.AreEqual(12.21, context.ViewModel.Centre.Longitude);
            }
        }

        private class Context : BaseTestContext
        {
            public MapViewModel ViewModel { get; private set; }

            public MockGeolocationProvider GeolocationProvider { get; private set; }

            public Context()
            {
                this.GeolocationProvider = new MockGeolocationProvider();
                this.ViewModel = new MapViewModel(this.NavigationService, this.DataService, this.GeolocationProvider);
            }
        }
    }
}
