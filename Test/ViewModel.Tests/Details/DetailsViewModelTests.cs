using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Details;
using CoffeeClientPrototype.ViewModel.Services;
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
                Assert.AreEqual("1 Main Street, A1 11A", context.ViewModel.Address, "Address");
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
                context.Reviews[cafe] = new List<Review>
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
        public void ReviewsWithoutCommentsSkippedWhenPopulating()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);
                context.Reviews[cafe] = new List<Review>
                    {
                        new Review { Comment = null, CoffeeRating = 5 },
                        new Review { Comment = null, AtmosphereRating = 3 }
                    };

                context.NavigateTo(cafe.Id);

                Assert.IsFalse(context.ViewModel.Reviews.Any());
            }
        }

        [TestMethod]
        public void IdentityReviewPopulatedWithLatestReviewByCurrentIdentity()
        {
            using (var context = new Context())
            {
                context.IdentityService.Id = "Me";

                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);
                context.Reviews[cafe] = new List<Review>
                    {
                        new Review
                        {
                            Comment = "My latest review!",
                            CoffeeRating = 2,
                            AtmosphereRating = 4,
                            SubmittedBy = "Me",
                            SubmittedDate = DateTime.Today.AddMonths(-1)
                        },
                        new Review
                        {
                            Comment = "My ancient review",
                            CoffeeRating = 1,
                            AtmosphereRating = 1,
                            SubmittedBy = "Me",
                            SubmittedDate = DateTime.Today.AddYears(-3)
                        }
                    };

                context.NavigateTo(cafe.Id);

                Assert.AreEqual("My latest review!", context.ViewModel.CurrentIdentityReview.Comment);
                Assert.AreEqual(2, context.ViewModel.CurrentIdentityReview.CoffeeRating);
                Assert.AreEqual(4, context.ViewModel.CurrentIdentityReview.AtmosphereRating);
            }
        }

        [TestMethod]
        public void IdentityReviewResetIfNoReviewByCurrentIdentity()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);
                context.Reviews[cafe] = new List<Review>
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

                Assert.IsNull(context.ViewModel.CurrentIdentityReview.Comment);
                Assert.IsNull(context.ViewModel.CurrentIdentityReview.CoffeeRating);
                Assert.IsNull(context.ViewModel.CurrentIdentityReview.AtmosphereRating);
            }
        }

        [TestMethod]
        public void ReviewsSortedNewestToOldest()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);
                context.Reviews[cafe] = new List<Review>
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
        public void SubmittedIdentityReviewUpdatesReviewCollectionIfCommentSupplied()
        {
            using (var context = new Context())
            {
                context.IdentityService.Id = "Me";

                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);
                context.NavigateTo(cafe.Id);

                context.ViewModel.CurrentIdentityReview.Comment = "My review";
                context.ViewModel.CurrentIdentityReview.Submit.Execute(null);

                Assert.AreEqual(1, context.ViewModel.Reviews.Count);
                var review = context.ViewModel.Reviews.First();
                Assert.AreEqual("My review", review.Comment);
                Assert.AreEqual("Me", review.SubmittedBy);

                context.ViewModel.CurrentIdentityReview.Comment = "My updated review!";
                context.ViewModel.CurrentIdentityReview.Submit.Execute(null);

                Assert.AreEqual(1, context.ViewModel.Reviews.Count);
                review = context.ViewModel.Reviews.First();
                Assert.AreEqual("My updated review!", review.Comment);
                Assert.AreEqual("Me", review.SubmittedBy);
            }
        }

        [TestMethod]
        public void SubmittedIdentityReviewDoesNotUpdatesReviewCollectionIfNoComment()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 1 };
                context.Cafes.Add(cafe);

                context.NavigateTo(cafe.Id);
                Assert.AreEqual(0, context.ViewModel.Reviews.Count);

                context.IdentityService.Id = "UserA";
                context.ViewModel.CurrentIdentityReview.CoffeeRating = 3;
                context.ViewModel.CurrentIdentityReview.Submit.Execute(null);

                Assert.IsFalse(context.ViewModel.Reviews.Any());
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
                context.ViewModel.CurrentIdentityReview.Comment = "Something";
                context.ViewModel.CurrentIdentityReview.CoffeeRating = 3;
                context.ViewModel.CurrentIdentityReview.AtmosphereRating = 4.5;

                Assert.IsFalse(context.ViewModel.CurrentIdentityReview.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void ShowMap()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe
                            {
                                Id = 1,
                                Name = "Coffee Shop",
                                Longitude = 45.15,
                                Latitude = 15.45
                            };
                context.Cafes.Add(cafe);
                context.NavigateTo(cafe.Id);

                context.ViewModel.ShowMap.Execute(null);

                Assert.IsNotNull(context.MapLauncher.LastLaunch);
                Assert.AreEqual("Coffee Shop", context.MapLauncher.LastLaunch.Name);
                Assert.AreEqual(45.15, context.MapLauncher.LastLaunch.Longitude);
                Assert.AreEqual(15.45, context.MapLauncher.LastLaunch.Latitude);
            }
        }

        [TestMethod]
        public void ShareCafe()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe
                    {
                        Name = "Coffee Shop",
                        Address = "1 Main Street",
                        PostCode = "X1 XXX"
                    };
                context.Cafes.Add(cafe);
                context.NavigateTo(cafe.Id);

                context.ViewModel.Share.Execute(null);

                Assert.AreEqual(1, context.ShareSource.SharedPackages.Count);
                Assert.AreEqual("Coffee Shop", context.ShareSource.SharedPackages.Last().Title);
                Assert.AreEqual("1 Main Street, X1 XXX", context.ShareSource.SharedPackages.Last().Text);
            }
        }
        
        [TestMethod]
        public void ShareNotExecutableBeforeOrAfterNavigating()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Name = "A" };
                context.Cafes.Add(cafe);

                Assert.IsFalse(context.ViewModel.Share.CanExecute(null));

                context.NavigateTo(cafe.Id);
                Assert.IsTrue(context.ViewModel.Share.CanExecute(null));

                context.ViewModel.OnNavigatedFrom();
                Assert.IsFalse(context.ViewModel.Share.CanExecute(null));
            }
        }
        private class Context : BaseTestContext
        {
            public DetailsViewModel ViewModel { get; private set; }

            public Dictionary<Cafe, List<Review>> Reviews { get { return this.DataService.Reviews; } }

            public MockMapLauncher MapLauncher { get; private set; }

            public MockShareSource ShareSource { get; private set; }

            public Context()
            {
                this.MapLauncher = new MockMapLauncher();
                this.ShareSource = new MockShareSource();

                this.ViewModel = new DetailsViewModel(
                                    this.DataService,
                                    this.IdentityService,
                                    this.MapLauncher,
                                    this.ShareSource);
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

        private class MockMapLauncher : IMapLauncher
        {
            public LaunchDetails LastLaunch { get; private set; }

            public async Task<bool> Launch(double longitude, double latitude, string name)
            {
                this.LastLaunch = new LaunchDetails
                                    {
                                        Longitude = longitude,
                                        Latitude = latitude,
                                        Name = name
                                    };
                return true;
            }

            public class LaunchDetails
            {
                public double Longitude { get; set; }

                public double Latitude { get; set; }

                public string Name { get; set; }
            }
        }

        private class MockShareSource : IShareSource
        {
            public IList<SharePackage> SharedPackages { get; private set; }

            public bool IsEnabled { get; set; }

            public MockShareSource()
            {
                this.SharedPackages = new List<SharePackage>();
            }

            public void Share(SharePackage package)
            {
                this.SharedPackages.Add(package);
            }
        }
    }
}
