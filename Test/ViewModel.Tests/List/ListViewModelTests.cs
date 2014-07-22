using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.List;
using CoffeeClientPrototype.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModel.Tests.List
{
    [TestClass]
    public class ListViewModelTests
    {
        [TestMethod]
        public async Task TenBestCafesPopulatedWhenNavigatedTo()
        {
            using (var context = new Context())
            {
                for (int i = 0; i < 20; i++)
                {
                    context.Cafes.Add(new Cafe { Name = "Cafe " + i });
                }

                await context.ViewModel.OnNavigatedTo();

                Assert.AreEqual(10, context.ViewModel.BestCafes.Count);
                Assert.IsTrue(context.ViewModel.BestCafes.All(c => c.Name.StartsWith("Cafe ")));

                // navigate a second time
                await context.ViewModel.OnNavigatedTo();

                Assert.AreEqual(10, context.ViewModel.BestCafes.Count);
                Assert.IsTrue(context.ViewModel.BestCafes.All(c => c.Name.StartsWith("Cafe ")));
            }
        }

        [TestMethod]
        public async Task BestCafesOrderedByRatingAndThenNumberOfVotes()
        {
            using (var context = new Context())
            {
                context.Cafes.AddRange(
                    new[]
                    {
                        new Cafe { Name = "B1", CoffeeRating = 4, AtmosphereRating = 3, NumberOfVotes = 10 },
                        new Cafe { Name = "A",  CoffeeRating = 5, AtmosphereRating = 3.75 },
                        new Cafe { Name = "C",  CoffeeRating = 1, AtmosphereRating = 4 },
                        new Cafe { Name = "B2", CoffeeRating = 4, AtmosphereRating = 3, NumberOfVotes = 5 }
                    });

                await context.ViewModel.OnNavigatedTo();

                var expected = new[] { "A", "B1", "B2", "C" };
                var actual = context.ViewModel.BestCafes.Select(cafe => cafe.Name).ToArray();
                CollectionAssert.AreEqual(expected, actual);

                // navigate a second time
                await context.ViewModel.OnNavigatedTo();
                actual = context.ViewModel.BestCafes.Select(cafe => cafe.Name).ToArray();
                CollectionAssert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public async Task NearbyCafesSortedByDistance()
        {
            using (var context = new Context())
            {
                context.Cafes.Add(new Cafe { Name = "C", Latitude = 10.0003, Longitude = 10.0003 });
                context.Cafes.Add(new Cafe { Name = "A", Latitude = 10.0001, Longitude = 10.0001 });
                context.Cafes.Add(new Cafe { Name = "B", Latitude = 10.0002, Longitude = 10.0002 });
                context.Cafes.Add(new Cafe { Name = "D", Latitude = 10.0004, Longitude = 10.0004 });

                context.GeolocationProvider.CurrentLocation = new Coordinate(10, 10);

                await context.ViewModel.OnNavigatedTo();

                var expected = new[] { "A", "B", "C", "D" };
                var actual = context.ViewModel.NearbyCafes.Select(cafe => cafe.Name).ToArray();
                CollectionAssert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void ShowMap()
        {
            using (var context = new Context())
            {
                context.ViewModel.ShowMap.Execute(null);
                Assert.AreEqual("Map", context.NavigationService.Current.Location);
            }
        }

        private class Context : BaseTestContext
        {
            public ListViewModel ViewModel { get; private set; }

            public MockGeolocationProvider GeolocationProvider { get; private set; }

            public Context()
            {
                this.GeolocationProvider = new MockGeolocationProvider();
                
                this.ViewModel = new ListViewModel(
                    this.DataService,
                    this.NavigationService,
                    this.GeolocationProvider);
            }
        }
    }
}
