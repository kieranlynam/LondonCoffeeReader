using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return this.serviceClient.GetTable<Cafe>().ReadAsync();
        }

        public Task<IEnumerable<Review>> GetCafeReviews(int cafeId)
        {
            return Task.FromResult(Enumerable.Empty<Review>());
        }

        public Task SaveCafeReview(int cafeId, Review review)
        {
            throw new NotImplementedException();
        }
    }
}
