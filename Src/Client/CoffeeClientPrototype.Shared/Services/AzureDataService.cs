using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using Microsoft.WindowsAzure.MobileServices;

namespace CoffeeClientPrototype.Services
{
    public class AzureDataService : IDataService
    {
        private readonly MobileServiceClient serviceClient;

        public AzureDataService(MobileServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
        }

        public async Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return await this.serviceClient
                .GetTable<Cafe>()
                .ReadAsync();
        }

        public Task<IEnumerable<Review>> GetCafeReviews(string cafeId)
        {
            return this.serviceClient
                .GetTable<Review>()
                .Where(review => review.CafeId == cafeId)
                .ToEnumerableAsync();
        }

        public async Task SaveCafeReview(Review review)
        {
            var tasks = new List<Task>();

            Task saveReviewTask;
            var reviewTable = this.serviceClient.GetTable<Review>();
            if (review.Id == null)
            {
                review.Id = Guid.NewGuid().ToString();
                saveReviewTask = reviewTable.InsertAsync(review);
            }
            else
            {
                saveReviewTask = reviewTable.UpdateAsync(review);
            }
            tasks.Add(saveReviewTask);

            if (review.CoffeeRating.HasValue || review.AtmosphereRating.HasValue)
            {
                var cafeTable = this.serviceClient.GetTable<Cafe>();
                var cafe = await cafeTable.LookupAsync(review.CafeId);
                cafe.NumberOfVotes++;
                tasks.Add(cafeTable.UpdateAsync(cafe));
            }
            
            await Task.WhenAll(tasks);
        }
    }
}
