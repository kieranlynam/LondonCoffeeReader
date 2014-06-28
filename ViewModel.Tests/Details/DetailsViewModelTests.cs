using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                        CoffeeRating = 3.5,
                        AtmosphereRating = 4.75,
                        NumberOfVotes = 200
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
                Assert.AreEqual(3.5, context.ViewModel.CoffeeRating, "CoffeeRating");
                Assert.AreEqual(4.75, context.ViewModel.AtmosphereRating, "AtmosphereRating");
                Assert.AreEqual(200, context.ViewModel.NumberOfVotes, "NumberOfVotes");
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
                        new Feedback { Comment = "Good!" },
                        new Feedback { Comment = "Bad!" },
                        new Feedback { Comment = "Ugly!" }
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
                        new Feedback
                        {
                            Comment = "Yesterday",
                            CreatedDate = DateTime.Today.AddDays(-1)
                        },
                        new Feedback
                        {
                            Comment = "Ancient!",
                            CreatedDate = DateTime.Today.AddYears(-5)
                        },
                        new Feedback
                        {
                            Comment = "Today",
                            CreatedDate = DateTime.Today
                        }
                    };

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafe.Id }
                    });

                Assert.AreEqual("Today", context.ViewModel.Comments[0].Comment);
                Assert.AreEqual("Yesterday", context.ViewModel.Comments[1].Comment);
                Assert.AreEqual("Ancient!", context.ViewModel.Comments[2].Comment);
            }
        }

        [TestMethod]
        public async Task SubmitUserFeedback()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafe.Id }
                    });

                context.ViewModel.UserFeedback.Comment = "New!";
                context.ViewModel.UserFeedback.Submit.Execute(null);

                Assert.IsTrue(context.Comments.ContainsKey(cafe),
                    "Expected a comment to be submitted");
                var comments = context.Comments[cafe];
                Assert.AreEqual(1, comments.Count);
                Assert.AreEqual("New!", comments.Last().Comment, "Feedback text");
            }
        }

        [TestMethod]
        public async Task CannotSubmitFeedbackWithoutText()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafe.Id }
                    });

                context.ViewModel.UserFeedback.Comment = "";
                Assert.IsFalse(context.ViewModel.UserFeedback.Submit.CanExecute(null));

                context.ViewModel.UserFeedback.Comment = "Something";
                Assert.IsTrue(context.ViewModel.UserFeedback.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public async Task CannotSubmitFeedbackBeforeNavigating()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);

                context.ViewModel.UserFeedback.Comment = "Something";
                Assert.IsFalse(context.ViewModel.UserFeedback.Submit.CanExecute(null));

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafe.Id }
                    });

                Assert.IsTrue(context.ViewModel.UserFeedback.Submit.CanExecute(null));
            }
        }

        private class Context : BaseTestContext
        {
            public DetailsViewModel ViewModel { get; private set; }

            public Dictionary<Cafe, IList<Feedback>> Comments { get { return this.DataService.Comments; } }

            public Context()
            {
                this.ViewModel = new DetailsViewModel(
                    this.DataService,
                    new MockNavigationService());
            }
        }
    }
}
