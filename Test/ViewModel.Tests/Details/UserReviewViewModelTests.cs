﻿using System;
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
                context.ViewModel.CoffeeRating = 3.5;
                context.ViewModel.AtmosphereRating = 4;
                context.ViewModel.Submit.Execute(null);

                var review = context.DataService.Reviews[cafe].Single();
                Assert.AreEqual("Great!", review.Comment, "Comment");
                Assert.AreEqual(3.5, review.CoffeeRating, "CoffeeRating");
                Assert.AreEqual(4, review.AtmosphereRating, "AtmosphereRating");
            }
        }

        [TestMethod]
        public void CanSubmitWithAssociatedCafeAndCommentAndRatings()
        {
            using (var context = new Context())
            {
                context.ViewModel.AssociatedCafe = new Cafe();
                context.ViewModel.Comment = "Great!";
                context.ViewModel.CoffeeRating = 5;
                context.ViewModel.AtmosphereRating = 3;
                Assert.IsTrue(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CannotSubmitWithoutComment()
        {
            using (var context = new Context())
            {
                context.ViewModel.AssociatedCafe = new Cafe();
                context.ViewModel.CoffeeRating = 1;
                context.ViewModel.AtmosphereRating = 1.5;

                context.ViewModel.Comment = string.Empty;
                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));

                context.ViewModel.Comment = null;
                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CannotSubmitWithoutCoffeeRating()
        {
            using (var context = new Context())
            {
                context.ViewModel.AssociatedCafe = new Cafe();
                context.ViewModel.Comment = "OK";
                context.ViewModel.CoffeeRating = null;
                context.ViewModel.AtmosphereRating = 1.5;

                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CannotSubmitWithoutAtmosphereRating()
        {
            using (var context = new Context())
            {
                context.ViewModel.AssociatedCafe = new Cafe();
                context.ViewModel.Comment = "OK";
                context.ViewModel.CoffeeRating = 3;
                context.ViewModel.AtmosphereRating = null;

                Assert.IsFalse(context.ViewModel.Submit.CanExecute(null));
            }
        }

        [TestMethod]
        public void CannotSubmitWithoutAssociatedCafe()
        {
            using (var context = new Context())
            {
                context.ViewModel.AssociatedCafe = null;
                context.ViewModel.CoffeeRating = 1;
                context.ViewModel.AtmosphereRating = 1.5;

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