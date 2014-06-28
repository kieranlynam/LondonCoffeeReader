using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task CommentsPopulatedWhenNavigatedTo()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);
                context.Comments[cafe] = new[]
                    {
                        new Comment { Text = "Good!" },
                        new Comment { Text = "Bad!" },
                        new Comment { Text = "Ugly!" }
                    };

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafe.Id }
                    });

                CollectionAssert.AreEquivalent(
                    context.Comments[cafe].ToArray(),
                    context.ViewModel.Comments);
            }
        }

        [TestMethod]
        public async Task CommentsSortedNewestToOldest()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);
                context.Comments[cafe] = new[]
                    {
                        new Comment
                        {
                            Text = "Yesterday",
                            CreatedDate = DateTime.Today.AddDays(-1)
                        },
                        new Comment
                        {
                            Text = "Ancient!",
                            CreatedDate = DateTime.Today.AddYears(-5)
                        },
                        new Comment
                        {
                            Text = "Today",
                            CreatedDate = DateTime.Today
                        }
                    };

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafe.Id }
                    });

                Assert.AreEqual("Today", context.ViewModel.Comments[0].Text);
                Assert.AreEqual("Yesterday", context.ViewModel.Comments[1].Text);
                Assert.AreEqual("Ancient!", context.ViewModel.Comments[2].Text);
            }
        }

        private class Context : BaseTestContext
        {
            public DetailsViewModel ViewModel { get; private set; }

            public Dictionary<Cafe, IEnumerable<Comment>> Comments { get { return this.DataService.Comments; } }

            public Context()
            {
                this.ViewModel = new DetailsViewModel(
                    this.DataService,
                    new MockNavigationService());
            }
        }
    }
}
