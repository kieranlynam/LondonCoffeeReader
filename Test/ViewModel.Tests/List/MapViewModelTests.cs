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
        public async Task NearbyCafesPopulatedRelativeToCurrentLocation()
        {
            using (var context = new Context())
            {
                context.Cafes.Add(new Cafe { Name = "D", Latitude = 10.0003, Longitude = 10.0003 });
                context.Cafes.Add(new Cafe { Name = "C", Latitude = 10.00025, Longitude = 10.00025 });
                context.Cafes.Add(new Cafe { Name = "A", Latitude = 10.0001, Longitude = 10.0001 });
                context.Cafes.Add(new Cafe { Name = "B", Latitude = 10.0002, Longitude = 10.0002 });

                context.GeolocationProvider.CurrentLocation = new Coordinate(10, 10);

                await context.ViewModel.OnNavigatedTo(new Dictionary<string, object>());

                CollectionAssert.AreEqual(
                    context.ViewModel.NearbyCafes.Select(cafe => cafe.Name).ToArray(),
                    new[] { "A", "B", "C", "D" });
            }
        }

        [TestMethod]
        public async Task InitialSelectedCafeSetToSpecificCafeIfRequested()
        {
            using (var context = new Context())
            {
                context.Cafes.Add(new Cafe { Id = 1, Name = "Nearest", Longitude = 8, Latitude =  8 });
                context.Cafes.Add(new Cafe { Id = 5, Name = "Requested", Longitude = 5, Latitude = 5 });

                context.GeolocationProvider.CurrentLocation = new Coordinate(8, 8);

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", 5 }
                    });

                Assert.AreEqual("Requested", context.ViewModel.SelectedCafe.Name);
            }
        }

        [TestMethod]
        public async Task InitialSelectedCafeSetToNearestToCurrentLocationIfNoSpecificRequested()
        {
            using (var context = new Context())
            {
                context.Cafes.Add(new Cafe { Name = "C", Latitude = 11.1, Longitude = 10.1 });
                context.Cafes.Add(new Cafe { Name = "A", Latitude = 10.0001, Longitude = 10.0001 });
                context.Cafes.Add(new Cafe { Name = "B", Latitude = 10.5, Longitude = 10.6 });

                context.GeolocationProvider.CurrentLocation = new Coordinate(10, 10);

                await context.ViewModel.OnNavigatedTo(new Dictionary<string, object>());

                Assert.AreEqual("A", context.ViewModel.SelectedCafe.Name);
            }
        }

        [TestMethod]
        public async Task CentrePopulatedFromCurrentLocationWhenNavigatedTo()
        {
            using (var context = new Context())
            {
                context.GeolocationProvider.CurrentLocation = new Coordinate(10, 10);

                await context.ViewModel.OnNavigatedTo(new Dictionary<string, object>());

                Assert.AreEqual(10, context.ViewModel.Centre.Latitude);
                Assert.AreEqual(10, context.ViewModel.Centre.Longitude);
            }
        }

        [TestMethod]
        public async Task CentrePopulatedFromSelectedCafeWhenNavigatedTo()
        {
            using (var context = new Context())
            {
                context.Cafes.Add(new Cafe { Latitude = 7, Longitude = 7 });
                await context.ViewModel.OnNavigatedTo(new Dictionary<string, object>());

                context.ViewModel.SelectCafe.Execute(context.ViewModel.Cafes[0]);
                await context.ViewModel.OnNavigatedTo(new Dictionary<string, object>());

                Assert.AreEqual(7, context.ViewModel.Centre.Latitude);
                Assert.AreEqual(7, context.ViewModel.Centre.Longitude);
            }
        }

        [TestMethod]
        public async Task RecentreAtCurrentLocationCommand()
        {
            using (var context = new Context())
            {
                context.GeolocationProvider.CurrentLocation = new Coordinate(10, 10);

                await context.ViewModel.OnNavigatedTo(new Dictionary<string, object>());

                context.ViewModel.Centre.Latitude = 5;
                context.ViewModel.Centre.Longitude = 5;
                context.ViewModel.RecentreAtCurrentLocation.Execute(null);

                Assert.AreEqual(10, context.ViewModel.Centre.Latitude);
                Assert.AreEqual(10, context.ViewModel.Centre.Longitude);
            }
        }

        [TestMethod]
        public async Task SelectCafeCommand()
        {
            using (var context = new Context())
            {
                context.Cafes.Add(new Cafe { Name = "A" });
                context.Cafes.Add(new Cafe { Name = "B" });
                
                await context.ViewModel.OnNavigatedTo(new Dictionary<string, object>());

                context.ViewModel.SelectCafe.Execute(context.ViewModel.Cafes[0]);
                Assert.AreEqual(context.ViewModel.Cafes[0], context.ViewModel.SelectedCafe);
                
                context.ViewModel.SelectCafe.Execute(context.ViewModel.Cafes[1]);
                Assert.AreEqual(context.ViewModel.Cafes[1], context.ViewModel.SelectedCafe);
            }
        }

        private class Context : BaseTestContext
        {
            public MapViewModel ViewModel { get; private set; }

            public MockGeolocationProvider GeolocationProvider { get; private set; }

            public Context()
            {
                this.GeolocationProvider = new MockGeolocationProvider();
                
                this.ViewModel = new MapViewModel(
                    this.NavigationService,
                    this.DataService,
                    this.GeolocationProvider);
            }
        }
    }
}
