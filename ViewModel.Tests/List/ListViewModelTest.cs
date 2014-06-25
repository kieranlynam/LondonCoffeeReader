using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.List;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModel.Tests.List
{
    [TestClass]
    public class ListViewModelTest
    {
        [TestMethod]
        public async Task BestCafesPopulatedWhenNavigatedTo()
        {
            using (var context = new Context())
            {
                context.Cafes.AddRange(
                    new[]
                    {
                        new Cafe { Name = "Cafe 1" },
                        new Cafe { Name = "Cafe 2" },
                        new Cafe { Name = "Cafe 3" }
                    });

                await context.ViewModel.OnNavigatedTo();

                Assert.AreEqual(3, context.ViewModel.BestCafes.Count);
                Assert.IsTrue(context.ViewModel.BestCafes.Any(item => item.Name == "Cafe 1"));
                Assert.IsTrue(context.ViewModel.BestCafes.Any(item => item.Name == "Cafe 2"));
                Assert.IsTrue(context.ViewModel.BestCafes.Any(item => item.Name == "Cafe 3"));
            }
        }

        [TestMethod]
        public async Task BestCafesOrderedByRating()
        {
            using (var context = new Context())
            {
                context.Cafes.AddRange(
                    new[]
                    {
                        new Cafe { Name = "B1", Rating = 4, NumberOfVotes = 10 },
                        new Cafe { Name = "A",  Rating = 5 },
                        new Cafe { Name = "C",  Rating  = 1 },
                        new Cafe { Name = "B2", Rating = 4, NumberOfVotes = 5 }
                    });

                await context.ViewModel.OnNavigatedTo();

                var expected = new[] { "A", "B1", "B2", "C" };
                var actual = context.ViewModel.BestCafes.Select(cafe => cafe.Name).ToArray();
                CollectionAssert.AreEqual(expected, actual);
            }
        }

        private class Context : IDisposable
        {
            public ListViewModel ViewModel { get; private set; }

            public List<Cafe> Cafes { get; private set; }

            public Context()
            {
                this.Cafes = new List<Cafe>();
                var dataService = new MockDataService(() => this.Cafes);

                this.ViewModel = new ListViewModel(dataService);
            }

            public void Dispose()
            {
            }
        }
    }
}
