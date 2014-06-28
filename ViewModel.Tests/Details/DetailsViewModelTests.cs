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
                        Name = "Cafe A",
                        Address = "1 Main Street",
                        PostCode = "A1 11A",
                        Latitude = 15,
                        Longitude = -10,
                        NumberOfVotes = 200,
                        Rating = 3.5
                    });

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", 1 }
                    });
                
                Assert.AreEqual("Cafe A", context.ViewModel.Name, "Name");
                Assert.AreEqual("1 Main Street", context.ViewModel.Address, "Address");
                Assert.AreEqual("A1 11A", context.ViewModel.PostCode, "PostCode");
                Assert.AreEqual(15, context.ViewModel.Latitude, "Latitude");
                Assert.AreEqual(-10, context.ViewModel.Longitude, "Longitude");
                Assert.AreEqual(200, context.ViewModel.NumberOfVotes, "NumberOfVotes");
                Assert.AreEqual(3.5, context.ViewModel.Rating, "Rating");
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
