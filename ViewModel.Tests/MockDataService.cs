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

        public Dictionary<Cafe, IList<Feedback>> Comments { get; private set; }

        public MockDataService()
        {
            this.Cafes = new List<Cafe>();
            this.Comments = new Dictionary<Cafe, IList<Feedback>>();
        }

        public async Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return this.Cafes;
        }

        public async Task<IEnumerable<Feedback>> GetAllCafeFeedback(int cafeId)
        {
            var cafe = this.Cafes.SingleOrDefault(c => c.Id == cafeId);

            if (cafe != null)
            {
                IList<Feedback> result;
                if (Comments.TryGetValue(cafe, out result))
                {
                    return result;
                }    
            }

            return Enumerable.Empty<Feedback>();
        }

        public async Task SubmitFeedback(int cafeId, Feedback feedback)
        {
            var cafe = this.Cafes.Single(c => c.Id == cafeId);

            if (!this.Comments.ContainsKey(cafe))
            {
                this.Comments.Add(cafe, new List<Feedback>());
            }

            this.Comments[cafe].Add(feedback);
        }
    }
}
