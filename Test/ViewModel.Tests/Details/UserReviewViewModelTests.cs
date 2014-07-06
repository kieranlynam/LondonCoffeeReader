using System.Linq;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Details;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModel.Tests.Details
{
    [TestClass]
    public class UserReviewViewModelTests
    {
        [TestMethod]
        public void InitializedWithReview()
        {
            using (var context = new Context())
            {
                context.ViewModel.Initialize(
                    new Review
                    {
                        Comment = "Great!",
                        CoffeeRating = 3.5,
                        AtmosphereRating = 4
                    });

                Assert.AreEqual("Great!", context.ViewModel.Comment, "Comment");
                Assert.AreEqual(3.5, context.ViewModel.CoffeeRating, "CoffeeRating");
                Assert.AreEqual(4, context.ViewModel.AtmosphereRating, "AtmosphereRating");
            }
        }

        [TestMethod]
        public void InitializedWithoutReview()
        {
            using (var context = new Context())
            {
                context.ViewModel.Comment = "Fantastic";
                context.ViewModel.CoffeeRating = 4;
                context.ViewModel.AtmosphereRating = 5;
                
                context.ViewModel.Initialize();

                Assert.IsNull(context.ViewModel.Comment, "Comment");
                Assert.IsNull(context.ViewModel.CoffeeRating, "CoffeeRating");
                Assert.IsNull(context.ViewModel.AtmosphereRating, "AtmosphereRating");
            }
        }

        [TestMethod]
        public void SubmittingSavesReviewToDataService()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 5 };
                context.DataService.Cafes.Add(cafe);

                context.IdentityService.Id = "UserA";
                context.ViewModel.AssociatedCafe = cafe;
                context.ViewModel.Comment = "Great!";
                context.ViewModel.CoffeeRating = 3.5;
                context.ViewModel.AtmosphereRating = 4;
                context.ViewModel.Submit.Execute(null);

                var review = context.DataService.Reviews[cafe].Single();
                Assert.AreEqual("Great!", review.Comment, "Comment");
                Assert.AreEqual(3.5, review.CoffeeRating, "CoffeeRating");
                Assert.AreEqual(4, review.AtmosphereRating, "AtmosphereRating");
                Assert.AreEqual("UserA", review.SubmittedBy, "SubmittedBy");
            }
        }

        [TestMethod]
        public void SubmittingRaisesEvent()
        {
            using (var context = new Context())
            {
                context.IdentityService.Id = "UserA";
                context.ViewModel.AssociatedCafe = new Cafe();
                context.ViewModel.Comment = "Great!";
                context.ViewModel.CoffeeRating = 3.5;
                context.ViewModel.AtmosphereRating = 4;

                bool wasEventRaised = false;
                context.ViewModel.ReviewSubmitted += (sender, submitted) =>
                {
                    wasEventRaised = true;
                    Assert.AreEqual("Great!", submitted.Review.Comment, "Comment");
                    Assert.AreEqual(3.5, submitted.Review.CoffeeRating, "CoffeeRating");
                    Assert.AreEqual(4, submitted.Review.AtmosphereRating, "AtmosphereRating");
                    Assert.AreEqual("UserA", submitted.Review.SubmittedBy, "SubmittedBy");
                };
                
                context.ViewModel.Submit.Execute(null);

                Assert.IsTrue(wasEventRaised);
            }
        }

        [TestMethod]
        public void CannotSubmitIfNoCommentOrRatingChanged()
        {
            using (var context = new Context())
            {
                context.IdentityService.Id = "UserA";
                context.ViewModel.AssociatedCafe = new Cafe();
                context.ViewModel.Initialize();
                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));

                context.ViewModel.Initialize(
                    new Review
                    {
                        Comment = "Alright",
                        CoffeeRating = 1,
                        AtmosphereRating = 2
                    });
                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CanSubmitAfterChangingComment()
        {
            using (var context = new Context())
            {
                context.IdentityService.Id = "UserA";
                context.ViewModel.AssociatedCafe = new Cafe();

                context.ViewModel.Initialize();
                context.ViewModel.Comment = "Like it";
                Assert.IsTrue(context.ViewModel.Submit.CanExecute(null));

                context.ViewModel.Initialize(new Review { Comment = "Good" });
                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));

                context.ViewModel.Comment = "Great!";
                Assert.IsTrue(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CanSubmitAfterChangingCoffeeRating()
        {
            using (var context = new Context())
            {
                context.IdentityService.Id = "UserA";
                context.ViewModel.AssociatedCafe = new Cafe();
                
                context.ViewModel.Initialize();
                context.ViewModel.CoffeeRating = 3;
                Assert.IsTrue(context.ViewModel.Submit.CanExecute(null));

                context.ViewModel.Initialize(new Review { CoffeeRating = 4 });
                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));

                context.ViewModel.CoffeeRating = 3;
                Assert.IsTrue(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CanSubmitAfterChangingAtmosphereRating()
        {
            using (var context = new Context())
            {
                context.IdentityService.Id = "UserA";
                context.ViewModel.AssociatedCafe = new Cafe();

                context.ViewModel.Initialize();
                context.ViewModel.AtmosphereRating = 3;
                Assert.IsTrue(context.ViewModel.Submit.CanExecute(null));

                context.ViewModel.Initialize(new Review { AtmosphereRating = 4 });
                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));

                context.ViewModel.AtmosphereRating = 3;
                Assert.IsTrue(context.ViewModel.Submit.CanExecute(null));
            }
        }
        
        [TestMethod]
        public void CannotSubmitWithoutAssociatedCafe()
        {
            using (var context = new Context())
            {
                context.IdentityService.Id = "UserA";
                context.ViewModel.AssociatedCafe = null;
                context.ViewModel.CoffeeRating = 1;
                context.ViewModel.AtmosphereRating = 1.5;

                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CannotSubmitWithoutAuthenticatedIdentity()
        {
            using (var context = new Context())
            {
                context.ViewModel.AssociatedCafe = new Cafe();
                context.ViewModel.CoffeeRating = 1;
                context.ViewModel.AtmosphereRating = 1.5;

                context.IdentityService.Id = null;

                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));
            }
        }

        private class Context : BaseTestContext
        {
            public UserReviewViewModel ViewModel { get; private set; }

            public Context()
            {
                this.ViewModel = 
                    new UserReviewViewModel(
                        this.DataService,
                        this.IdentityService);
            }
        }
    }
}
