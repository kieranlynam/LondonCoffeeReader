using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssertExLib;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Details;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModel.Tests.Details
{
    [TestClass]
    public class DetailsViewModelTests
    {
        [TestMethod]
        public async Task PropertiesPopulatedWhenNavigatedTo()
        {
            using (var context = new Context())
            {
                context.Cafes.Add(
                    new Cafe
                    {
                        Id = 1,
                        Name = "Cafe A"
                    });

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", 1 }
                    });
                
                Assert.AreEqual("Cafe A", context.ViewModel.Name);
            }
        }

        [TestMethod]
        public void ExceptionWhenBadIdPassed()
        {
            using (var context = new Context())
            {
                context.Cafes.Add(new Cafe { Id = 1 });

                AssertEx.TaskThrows<ArgumentException>(() =>
                    context.ViewModel.OnNavigatedTo(
                        new Dictionary<string, object>
                        {
                            { "Id", 2 }
                        }));
            }
        }

        private class Context : BaseTestContext
        {
            public DetailsViewModel ViewModel { get; private set; }

            public Context()
            {
                this.ViewModel = new DetailsViewModel(
                    this.DataService,
                    new MockNavigationService());
            }
        }
    }
}
