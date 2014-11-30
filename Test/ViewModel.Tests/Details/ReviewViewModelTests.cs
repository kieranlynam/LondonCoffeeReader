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

        private class Context : BaseTestContext
        {
            public ReviewViewModel ViewModel { get; private set; }

            public Context()
            {
                this.ViewModel =
                    new ReviewViewModel(this.DataService);
            }
        }
    }
}
