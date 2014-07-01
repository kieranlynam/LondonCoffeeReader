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
        public async Task PhotosPopulatedWhenNavigatedTo()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe
                    {
                        Id = 1,
                        Photos = new[]
                        {
                            new Photo
                            {
                                SubmittedBy = "Tom",
                                NumberOfVotes = 2
                            },
                            new Photo
                            {
                                SubmittedBy = "Dick",
                                NumberOfVotes = 5
                            },
                            new Photo
                            {
                                SubmittedBy = "Harry",
                                NumberOfVotes = 3
                            }
                        }
                    };
                context.Cafes.Add(cafe);

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafe.Id }
                    });

                var expected = cafe.Photos
                    .OrderByDescending(photo => photo.NumberOfVotes)
                    .ToArray();
                CollectionAssert.AreEqual(
                    expected,
                    context.ViewModel.Photos);
            }
        }

        [TestMethod]
        public async Task ReviewsPopulatedWhenNavigatedTo()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);
                context.Reviews[cafe] = new[]
                    {
                        new Review { Comment = "Good!" },
                        new Review { Comment = "Bad!" },
                        new Review { Comment = "Ugly!" }
                    };

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafe.Id }
                    });

                CollectionAssert.AreEquivalent(
                    context.Reviews[cafe].ToArray(),
                    context.ViewModel.Reviews);
            }
        }

        [TestMethod]
        public async Task UserReviewPopulatedWithPreviouslySubmittedReviewIfExists()
        {
            using (var context = new Context())
            {
                context.IdentityService.Id = "Me";

                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);
                context.Reviews[cafe] = new[]
                    {
                        new Review
                        {
                            Comment = "My review!",
                            CoffeeRating = 2,
                            AtmosphereRating = 4,
                            SubmittedBy = "Me"
                        }
                    };

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafe.Id }
                    });

                Assert.AreEqual("My review!", context.ViewModel.UserReview.Comment);
                Assert.AreEqual(2, context.ViewModel.UserReview.CoffeeRating);
                Assert.AreEqual(4, context.ViewModel.UserReview.AtmosphereRating);
            }
        }

        [TestMethod]
        public async Task ReviewsSortedNewestToOldest()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);
                context.Reviews[cafe] = new[]
                    {
                        new Review
                        {
                            Comment = "Yesterday",
                            SubmittedDate = DateTime.Today.AddDays(-1)
                        },
                        new Review
                        {
                            Comment = "Ancient!",
                            SubmittedDate = DateTime.Today.AddYears(-5)
                        },
                        new Review
                        {
                            Comment = "Today",
                            SubmittedDate = DateTime.Today
                        }
                    };

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafe.Id }
                    });

                Assert.AreEqual("Today", context.ViewModel.Reviews[0].Comment);
                Assert.AreEqual("Yesterday", context.ViewModel.Reviews[1].Comment);
                Assert.AreEqual("Ancient!", context.ViewModel.Reviews[2].Comment);
            }
        }

        [TestMethod]
        public async Task SubmitUserReview()
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

                context.IdentityService.Id = "UserA";
                context.ViewModel.UserReview.Comment = "New!";
                context.ViewModel.UserReview.CoffeeRating = 3.5;
                context.ViewModel.UserReview.AtmosphereRating = 4.5;
                context.ViewModel.UserReview.Submit.Execute(null);

                Assert.IsTrue(context.Reviews.ContainsKey(cafe),
                    "Expected a comment to be submitted");
                var reviews = context.Reviews[cafe];
                Assert.AreEqual(1, reviews.Count);
                Assert.AreEqual("New!", reviews.Last().Comment);
                Assert.AreEqual(3.5, reviews.Last().CoffeeRating);
                Assert.AreEqual(4.5, reviews.Last().AtmosphereRating);
                Assert.AreEqual("UserA", reviews.Last().SubmittedBy);
            }
        }

        [TestMethod]
        public async Task CannotSubmitReviewBeforeNavigating()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);

                context.IdentityService.Id = "UserA";
                context.ViewModel.UserReview.Comment = "Something";
                context.ViewModel.UserReview.CoffeeRating = 3;
                context.ViewModel.UserReview.AtmosphereRating = 4.5;
                Assert.IsFalse(context.ViewModel.UserReview.Submit.CanExecute(null));

                await context.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafe.Id }
                    });

                Assert.IsTrue(context.ViewModel.UserReview.Submit.CanExecute(null));
            }
        }

        private class Context : BaseTestContext
        {
            public DetailsViewModel ViewModel { get; private set; }

            public Dictionary<Cafe, IList<Review>> Reviews { get { return this.DataService.Reviews; } }

            public Context()
            {
                this.ViewModel = new DetailsViewModel(
                    this.DataService,
                    this.IdentityService);
            }
        }
    }
}
