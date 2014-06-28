using System;
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
        public void SubmittingPushesToDataService()
        {
            using (var context = new Context())
            {
                var cafe = new Cafe { Id = 5 };
                context.DataService.Cafes.Add(cafe);

                context.ViewModel.AssociatedCafe = cafe;
                context.ViewModel.Comment = "Great!";
                context.ViewModel.Submit.Execute(null);

                var review = context.DataService.Reviews[cafe].Single();
                Assert.AreEqual("Great!", review.Comment);
            }
        }

        [TestMethod]
        public void CanSubmitWithAssociatedCafeAndComment()
        {
            using (var context = new Context())
            {
                context.ViewModel.AssociatedCafe = new Cafe();
                context.ViewModel.Comment = "Great!";
                Assert.IsTrue(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CannotSubmitWithoutComment()
        {
            using (var context = new Context())
            {
                context.ViewModel.AssociatedCafe = new Cafe();

                context.ViewModel.Comment = string.Empty;
                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));

                context.ViewModel.Comment = null;
                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CannotSubmitWithoutAssociatedCafe()
        {
            using (var context = new Context())
            {
                context.ViewModel.AssociatedCafe = null;

                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));
            }
        }

        private class Context : BaseTestContext
        {
            public UserReviewViewModel ViewModel { get; private set; }

            public Context()
            {
                this.ViewModel = new UserReviewViewModel(this.DataService);
            }
        }
    }
}
