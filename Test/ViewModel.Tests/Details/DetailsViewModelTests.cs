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
        public void PropertiesPopulatedWhenNavigatedTo()
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

                context.NavigateTo(cafeId: 1);
                
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
        public void PhotosPopulatedWhenNavigatedTo()
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

                context.NavigateTo(cafe.Id);

                var expected = cafe.Photos
                    .OrderByDescending(photo => photo.NumberOfVotes)
                    .ToArray();
                CollectionAssert.AreEqual(
                    expected.Select(p => p.SubmittedBy).ToArray(),
                    context.ViewModel.Photos.Select(p => p.SubmittedBy).ToArray());
            }
        }

        [TestMethod]
        public void ReviewsPopulatedWhenNavigatedTo()
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

                context.NavigateTo(cafe.Id);

                CollectionAssert.AreEquivalent(
                    context.Reviews[cafe].Select(r => r.Comment).ToArray(),
                    context.ViewModel.Reviews.Select(r => r.Comment).ToArray());
            }
        }

        [TestMethod]
        public void UserReviewPopulatedIfReviewByCurrentIdentityExists()
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

                context.NavigateTo(cafe.Id);

                Assert.AreEqual("My review!", context.ViewModel.UserReview.Comment);
                Assert.AreEqual(2, context.ViewModel.UserReview.CoffeeRating);
                Assert.AreEqual(4, context.ViewModel.UserReview.AtmosphereRating);
            }
        }

        [TestMethod]
        public void UserReviewResetIfNoReviewByCurrentIdentity()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);
                context.Reviews[cafe] = new[]
                    {
                        new Review
                        {
                            Comment = "Somebody else's opinion",
                            CoffeeRating = 1,
                            AtmosphereRating = 3,
                            SubmittedBy = "SomebodyElse"
                        }
                    };

                context.IdentityService.Id = "SomebodyElse";
                context.NavigateTo(cafe.Id);

                context.IdentityService.Id = "Me";
                context.NavigateTo(cafe.Id);

                Assert.IsNull(context.ViewModel.UserReview.Comment);
                Assert.IsNull(context.ViewModel.UserReview.CoffeeRating);
                Assert.IsNull(context.ViewModel.UserReview.AtmosphereRating);
            }
        }

        [TestMethod]
        public void ReviewsSortedNewestToOldest()
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

                context.NavigateTo(cafe.Id);

                Assert.AreEqual("Today", context.ViewModel.Reviews[0].Comment);
                Assert.AreEqual("Yesterday", context.ViewModel.Reviews[1].Comment);
                Assert.AreEqual("Ancient!", context.ViewModel.Reviews[2].Comment);
            }
        }

        [TestMethod]
        public void SubmitUserReviewUpdatesReviewCollection()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);

                context.NavigateTo(cafe.Id);
                Assert.AreEqual(0, context.ViewModel.Reviews.Count);

                context.IdentityService.Id = "UserA";
                context.ViewModel.UserReview.Comment = "New!";
                context.ViewModel.UserReview.CoffeeRating = 3.5;
                context.ViewModel.UserReview.AtmosphereRating = 4.5;
                context.ViewModel.UserReview.Submit.Execute(null);

                Assert.AreEqual(1, context.ViewModel.Reviews.Count);
                var review = context.ViewModel.Reviews.First();
                Assert.AreEqual("New!", review.Comment);
                Assert.AreEqual(3.5, review.CoffeeRating);
                Assert.AreEqual(4.5, review.AtmosphereRating);
                Assert.AreEqual("UserA", review.SubmittedBy);
            }
        }

        [TestMethod]
        public void CannotSubmitReviewBeforeNavigating()
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

            public void NavigateTo(int cafeId)
            {
                 var navigation = this.ViewModel.OnNavigatedTo(
                    new Dictionary<string, object>
                    {
                        { "Id", cafeId }
                    });
                navigation.Wait();
            }
        }
    }
}
