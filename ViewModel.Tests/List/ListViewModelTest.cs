using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task RefreshNearByCafes()
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
