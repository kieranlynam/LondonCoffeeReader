using System;
using System.Linq;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Details;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModel.Tests.Details
{
    [TestClass]
    public class ReviewViewModelTests
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
                        AtmosphereRating = 4,
                        SubmittedBy = "John",
                        SubmittedDate = new DateTime(2001, 1, 2)
                    });

                Assert.AreEqual("Great!", context.ViewModel.Comment, "Comment");
                Assert.AreEqual(3.5, context.ViewModel.CoffeeRating, "CoffeeRating");
                Assert.AreEqual(4, context.ViewModel.AtmosphereRating, "AtmosphereRating");
                Assert.AreEqual("John", context.ViewModel.SubmittedBy, "SubmittedBy");
                Assert.AreEqual(new DateTime(2001, 1, 2), context.ViewModel.SubmittedDate, "SubmittedDate");

                // without a comment
                context.ViewModel.Initialize(
                    new Review
                    {
                        Comment = null,
                        CoffeeRating = 2.5,
                        AtmosphereRating = 5
                    });
                Assert.IsNull(context.ViewModel.Comment, "Comment");
                Assert.AreEqual(2.5, context.ViewModel.CoffeeRating, "CoffeeRating");
                Assert.AreEqual(5, context.ViewModel.AtmosphereRating, "AtmosphereRating");

                // without a coffee rating
                context.ViewModel.Initialize(
                    new Review
                    {
                        Comment = "Fab!",
                        CoffeeRating = null,
                        AtmosphereRating = 5
                    });
                Assert.AreEqual("Fab!", context.ViewModel.Comment, "Comment");
                Assert.IsNull(context.ViewModel.CoffeeRating, "CoffeeRating");
                Assert.AreEqual(5, context.ViewModel.AtmosphereRating, "AtmosphereRating");

                // without an atmosphere rating
                context.ViewModel.Initialize(
                    new Review
                    {
                        Comment = "OK",
                        CoffeeRating = 2,
                        AtmosphereRating = null
                    });
                Assert.AreEqual("OK", context.ViewModel.Comment, "Comment");
                Assert.AreEqual(2, context.ViewModel.CoffeeRating, "CoffeeRating");
                Assert.IsNull(context.ViewModel.AtmosphereRating, "AtmosphereRating");
            }
        }

        [TestMethod]
        public void InitializedWithoutReview()
        {
            using (var context = new Context())
            {
                context.ViewModel.Initialize(
                    new Review
                    {
                        Comment = "Lovely",
                        CoffeeRating = 4,
                        AtmosphereRating = 3,
                        SubmittedBy = "Mary"
                    });
                
                context.ViewModel.Initialize();

                Assert.IsNull(context.ViewModel.Comment, "Comment");
                Assert.IsNull(context.ViewModel.CoffeeRating, "CoffeeRating");
                Assert.IsNull(context.ViewModel.AtmosphereRating, "AtmosphereRating");
                Assert.IsNull(context.ViewModel.SubmittedBy, "SubmittedBy");
                Assert.IsNull(context.ViewModel.SubmittedDate, "SubmittedDate");
            }
        }

        [TestMethod]
        public void SubmittingNewReviewSavesToDataService()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = "5" };
                context.DataService.Cafes.Add(cafe);

                context.IdentityService.SetCurrentIdentity("UserA");
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
        public void SubmittingExistingReviewSavesToDataService()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = "5" };
                context.DataService.Cafes.Add(cafe);
                context.IdentityService.SetCurrentIdentity("UserB");

                context.ViewModel.Initialize(new Review { Id = "777" });
                context.ViewModel.AssociatedCafe = cafe;
                context.ViewModel.Comment = "Fantastic!";
                context.ViewModel.CoffeeRating = 4.5;
                context.ViewModel.AtmosphereRating = 3;
                context.ViewModel.Submit.Execute(null);

                var review = context.DataService.Reviews[cafe].Single();
                Assert.AreEqual("777", review.Id, "Id");
                Assert.AreEqual("Fantastic!", review.Comment, "Comment");
                Assert.AreEqual(4.5, review.CoffeeRating, "CoffeeRating");
                Assert.AreEqual(3, review.AtmosphereRating, "AtmosphereRating");
                Assert.AreEqual("UserB", review.SubmittedBy, "SubmittedBy");
            }
        }

        [TestMethod]
        public void SubmittingTrimsComment()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = "5" };
                context.DataService.Cafes.Add(cafe);

                context.IdentityService.SetCurrentIdentity("UserA");
                context.ViewModel.AssociatedCafe = cafe;
                context.ViewModel.Comment = " Love  it!  ";
                context.ViewModel.Submit.Execute(null);

                var review = context.DataService.Reviews[cafe].Single();
                Assert.AreEqual("Love  it!", review.Comment, "Comment");
            }
        }

        [TestMethod]
        public void SubmittingSkipsSaveIfNothingToSubmitAfterTrimming()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = "5" };
                context.DataService.Cafes.Add(cafe);

                context.IdentityService.SetCurrentIdentity("UserA");
                context.ViewModel.AssociatedCafe = cafe;
                context.ViewModel.Comment = "   ";
                context.ViewModel.CoffeeRating = null;
                context.ViewModel.AtmosphereRating = null;
                context.ViewModel.Submit.Execute(null);

                Assert.IsFalse(context.DataService.Reviews.ContainsKey(cafe));
            }
        }

        [TestMethod]
        public void SubmittingSetsSubmissionProperties()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = "5" };
                context.DataService.Cafes.Add(cafe);

                context.IdentityService.SetCurrentIdentity("Jim");
                context.ViewModel.AssociatedCafe = cafe;
                context.ViewModel.Comment = "Tasted better";
                context.ViewModel.Submit.Execute(null);

                Assert.AreEqual("Jim", context.ViewModel.SubmittedBy);
                Assert.IsNotNull(context.ViewModel.SubmittedDate);
            }
        }

        [TestMethod]
        public void SubmittingChallengesIfUnauthenticated()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = "5" };
                context.DataService.Cafes.Add(cafe);

                context.IdentityService.ClearCurrentIdentity();
                context.IdentityService.AuthenticationRequested +=
                    (sender, args) => args.Success("Tim");
                
                context.ViewModel.AssociatedCafe = cafe;
                context.ViewModel.Comment = "Smashing!";
                context.ViewModel.Submit.Execute(null);

                Assert.AreEqual("Tim", context.ViewModel.SubmittedBy);
            }
        }

        [TestMethod]
        public void SubmittingSkippedIfAuthenticationFailed()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = "5" };
                context.DataService.Cafes.Add(cafe);

                context.IdentityService.ClearCurrentIdentity();
                context.IdentityService.AuthenticationRequested +=
                    (sender, args) => args.Fail();

                context.ViewModel.AssociatedCafe = cafe;
                context.ViewModel.Comment = "Good stuff";
                context.ViewModel.Submit.Execute(null);

                Assert.IsFalse(context.DataService.Reviews.ContainsKey(cafe));
            }
        }

        [TestMethod]
        public void SubmittingRaisesEvent()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = "5" };
                context.DataService.Cafes.Add(cafe);

                context.IdentityService.SetCurrentIdentity("UserA");
                context.ViewModel.AssociatedCafe = cafe;
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
        public void CannotSubmitIfUnchangedSinceLastSubmit()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = "5" };
                context.DataService.Cafes.Add(cafe);

                context.IdentityService.SetCurrentIdentity("UserA");
                context.ViewModel.AssociatedCafe = cafe;
                context.ViewModel.Comment = "Great!";

                context.ViewModel.Submit.Execute(null);
                context.ViewModel.Comment = "Great!";
                
                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CannotSubmitIfNoCommentOrRatingChanged()
        {
            using (var context = new Context())
            {
                context.IdentityService.SetCurrentIdentity("UserA");
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
                context.IdentityService.SetCurrentIdentity("UserA");
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
                context.IdentityService.SetCurrentIdentity("UserA");
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
                context.IdentityService.SetCurrentIdentity("UserA");
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
                context.IdentityService.SetCurrentIdentity("UserA");
                context.ViewModel.AssociatedCafe = null;
                context.ViewModel.CoffeeRating = 1;
                context.ViewModel.AtmosphereRating = 1.5;

                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CannotSubmitIfReviewSubmittedByAnotherIdentity()
        {
            using (var context = new Context())
            {
                context.ViewModel.AssociatedCafe = new Cafe();
                context.ViewModel.Initialize(
                    new Review
                    {
                        CoffeeRating = 5,
                        SubmittedBy = "Carl"
                    });

                context.IdentityService.SetCurrentIdentity("UserA");
                context.ViewModel.Comment = "Trying to take over Carl's review!";

                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));
            }
        }

        private class Context : BaseTestContext
        {
            public ReviewViewModel ViewModel { get; private set; }

            public Context()
            {
                this.ViewModel = 
                    new ReviewViewModel(
                        this.DataService,
                        this.IdentityService);
            }
        }
    }
}
