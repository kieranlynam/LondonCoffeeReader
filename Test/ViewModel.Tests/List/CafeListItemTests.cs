using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.List;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModel.Tests.List
{
    [TestClass]
    public class CafeListItemTests
    {
        [TestMethod]
        public void PropertiesPopulatedFromModel()
        {
            var cafe = new Cafe
                {
                    Name = "Coffee Shop",
                    Address = "1 Main Street",
                    PostCode = "A1",
                    Longitude = 45,
                    Latitude = -12,
                    CoffeeRating = 3.5,
                    AtmosphereRating = 4.5,
                    NumberOfVotes = 12
                };

            var result = new CafeListItem(cafe, new MockNavigationService());

            Assert.AreEqual("Coffee Shop", result.Name, "Name");
            Assert.AreEqual(45, result.Longitude, "Longitude");
            Assert.AreEqual(-12, result.Latitude, "Latitude");
            Assert.AreEqual(4, result.Rating, "Rating");
            Assert.AreEqual(12, result.NumberOfVotes, "NumberOfVotes");
        }

        [TestMethod]
        public void MostPopularPhotoShown()
        {
            var cafe = new Cafe
                {
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

            var result = new CafeListItem(cafe, new MockNavigationService());

            Assert.IsNotNull(result.Photo);
            Assert.AreEqual("Dick", result.Photo.SubmittedBy);
        }

        [TestMethod]
        public void NavigateToCafeDetails()
        {
            var navigationService = new MockNavigationService();
            var cafe = new Cafe { Id = 1 };
            var item = new CafeListItem(cafe, navigationService);
            
            item.Navigate.Execute(null);

            Assert.AreEqual("CafeDetails", navigationService.Current.Location);
            Assert.AreEqual(1, navigationService.Current.Parameters["Id"]);
        }
    }
}
