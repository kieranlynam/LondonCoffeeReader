using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype
{
    internal class DesignDataService : IDataService
    {
        private readonly static IEnumerable<Cafe> AllCafes = 
            new[]
            {
                new Cafe { Id = 1, Name = "Tina, I Salute You" },
                new Cafe { Id = 2, Name = "Shoreditch Grind" },
                new Cafe { Id = 3, Name = "Prufrock Coffee" }
            };

        public Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return Task.FromResult(AllCafes);
        }

        public async Task<IEnumerable<Feedback>> GetAllCafeFeedback(int cafeId)
        {
            var cafe = AllCafes.First(c => c.Id == cafeId);

            return new List<Feedback>
                {
                    new Feedback
                    {
                        Comment = cafe.Name + " is great!",
                        CreatedDate = DateTime.Now.AddDays(-100)
                    },
                    new Feedback
                    {
                        Comment = "Not crazy about " + cafe.Name
                    }
                };
        }

        public Task SubmitFeedback(int cafeId, Feedback feedback)
        {
            throw new NotImplementedException();
        }
    }
}
