﻿using System;
using System.Linq;
using CoffeeClientPrototype.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.Services
{
    internal class DesignDataService : IDataService
    {
        private readonly static IEnumerable<Cafe> AllCafes = 
            new[]
            {
                new Cafe
                {
                    Id = 1,
                    Name = "Tina, We Salute You",
                    CoffeeRating = 4,
                    AtmosphereRating = 5,
                    NumberOfVotes = 250,
                    Address = "47 King Henry's Walk",
                    PostCode = "N1 4NH"
                },
                new Cafe
                {
                    Id = 2,
                    Name = "Shoreditch Grind",
                    CoffeeRating = 4.75,
                    AtmosphereRating = 3.5,
                    NumberOfVotes = 320,
                    Address = "213 Old Street",
                    PostCode = "EC1V 9NR"
                },
                new Cafe
                {
                    Id = 3,
                    Name = "Prufrock Café",
                    CoffeeRating = 5,
                    AtmosphereRating = 3.5,
                    NumberOfVotes = 527,
                    Address = "23-25 Leather Ln",
                    PostCode = "EC1N 7TE"
                }
            };

        public Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return Task.FromResult(AllCafes);
        }

        public async Task<IEnumerable<Review>> GetCafeReviews(int cafeId)
        {
            var cafe = AllCafes.First(c => c.Id == cafeId);

            return new List<Review>
                {
                    new Review
                    {
                        Comment = cafe.Name + " is great!",
                        SubmittedDate = DateTime.Now.AddDays(-100)
                    },
                    new Review
                    {
                        Comment = "Not crazy about " + cafe.Name
                    }
                };
        }

        public Task SaveCafeReview(int cafeId, Review review)
        {
            throw new NotImplementedException();
        }
    }
}
