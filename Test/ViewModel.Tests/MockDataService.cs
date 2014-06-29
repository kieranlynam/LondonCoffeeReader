using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;

namespace ViewModel.Tests
{
    public class MockDataService : IDataService
    {
        public List<Cafe> Cafes { get; private set; }

        public Dictionary<Cafe, IList<Review>> Reviews { get; private set; }

        public MockDataService()
        {
            this.Cafes = new List<Cafe>();
            this.Reviews = new Dictionary<Cafe, IList<Review>>();
        }

        public async Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return this.Cafes;
        }

        public async Task<IEnumerable<Review>> GetCafeReviews(int cafeId)
        {
            var cafe = this.Cafes.SingleOrDefault(c => c.Id == cafeId);

            if (cafe != null)
            {
                IList<Review> result;
                if (Reviews.TryGetValue(cafe, out result))
                {
                    return result;
                }    
            }

            return Enumerable.Empty<Review>();
        }

        public async Task SaveCafeReview(int cafeId, Review review)
        {
            var cafe = this.Cafes.Single(c => c.Id == cafeId);

            if (!this.Reviews.ContainsKey(cafe))
            {
                this.Reviews.Add(cafe, new List<Review>());
            }

            this.Reviews[cafe].Add(review);
        }
    }
}
