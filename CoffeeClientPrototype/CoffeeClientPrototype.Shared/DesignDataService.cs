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

        public async Task<IEnumerable<Comment>> GetCafeComments(int cafeId)
        {
            var cafe = AllCafes.First(c => c.Id == cafeId);

            return new List<Comment>
                {
                    new Comment
                    {
                        Text = cafe.Name + " is great!",
                        CreatedDate = DateTime.Now.AddDays(-100)
                    },
                    new Comment
                    {
                        Text = "Not crazy about " + cafe.Name
                    }
                };
        }
    }
}
