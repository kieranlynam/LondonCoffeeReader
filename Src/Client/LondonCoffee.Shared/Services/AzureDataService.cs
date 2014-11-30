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

        public Task SaveCafeReview(Review review)
        {
            var reviewTable = this.serviceClient.GetTable<Review>();
            
            if (review.Id == null)
            {
                review.Id = Guid.NewGuid().ToString();
                return reviewTable.InsertAsync(review);
            }

            return reviewTable.UpdateAsync(review);
        }
    }
}
