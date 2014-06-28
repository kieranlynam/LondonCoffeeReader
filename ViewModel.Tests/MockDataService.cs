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

        public Dictionary<Cafe, IList<Comment>> Comments { get; private set; }

        public MockDataService()
        {
            this.Cafes = new List<Cafe>();
            this.Comments = new Dictionary<Cafe, IList<Comment>>();
        }

        public async Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return this.Cafes;
        }

        public async Task<IEnumerable<Comment>> GetCafeComments(int cafeId)
        {
            var cafe = this.Cafes.SingleOrDefault(c => c.Id == cafeId);

            if (cafe != null)
            {
                IList<Comment> result;
                if (Comments.TryGetValue(cafe, out result))
                {
                    return result;
                }    
            }

            return Enumerable.Empty<Comment>();
        }

        public async Task SubmitComment(int cafeId, Comment comment)
        {
            var cafe = this.Cafes.Single(c => c.Id == cafeId);

            if (!this.Comments.ContainsKey(cafe))
            {
                this.Comments.Add(cafe, new List<Comment>());
            }

            this.Comments[cafe].Add(comment);
        }
    }
}
