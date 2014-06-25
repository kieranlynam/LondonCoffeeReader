using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.List;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModel.Tests.List
{
    [TestClass]
    public class CafeListItemTest
    {
        [TestMethod]
        public void CreateFromModel()
        {
            var cafe = new Cafe
                {
                    Name = "Coffee Shop",
                    Address = "1 Main Street",
                    PostCode = "A1",
                    Longitude = 45,
                    Latitude = -12,
                    Rating = 3.5,
                    NumberOfVotes = 12
                };

            var result = CafeListItem.FromModel(cafe);

            Assert.AreEqual("Coffee Shop", result.Name, "Name");
            Assert.AreEqual(45, result.Longitude, "Longitude");
            Assert.AreEqual(-12, result.Latitude, "Latitude");
            Assert.AreEqual(3.5, result.Rating, "Rating");
            Assert.AreEqual(12, result.NumberOfVotes, "NumberOfVotes");
        }
    }
}
