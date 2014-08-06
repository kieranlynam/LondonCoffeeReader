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

        public Dictionary<Cafe, List<Review>> Reviews { get; private set; }

        public MockDataService()
        {
            this.Cafes = new List<Cafe>();
            this.Reviews = new Dictionary<Cafe, List<Review>>();
        }

        public async Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return this.Cafes;
        }

        public async Task<IEnumerable<Review>> GetCafeReviews(string cafeId)
        {
            var cafe = this.Cafes.SingleOrDefault(c => c.Id == cafeId);

            if (cafe != null)
            {
                List<Review> result;
                if (Reviews.TryGetValue(cafe, out result))
                {
                    return result;
                }    
            }

            return Enumerable.Empty<Review>();
        }

        public async Task SaveCafeReview(Review review)
        {
            var cafe = this.Cafes.Single(c => c.Id == review.CafeId);

            if (!this.Reviews.ContainsKey(cafe))
            {
                this.Reviews.Add(cafe, new List<Review>());
            }
            else
            {
                var existingReview = this.Reviews[cafe].FirstOrDefault(r => r.Id == review.Id);
                if (existingReview != null)
                {
                    this.Reviews[cafe].Remove(existingReview);
                }
            }

            this.Reviews[cafe].Add(review);
            
            cafe.NumberOfVotes = this.Reviews[cafe].Count(r => r.CoffeeRating.HasValue || r.AtmosphereRating.HasValue);
            cafe.CoffeeRating = this.Reviews[cafe].Average(r => r.CoffeeRating.HasValue ? r.CoffeeRating.Value : 0);
            cafe.AtmosphereRating = this.Reviews[cafe].Average(r => r.AtmosphereRating.HasValue ? r.AtmosphereRating.Value : 0);
        }
    }
}
