using System;
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
                    PostCode = "N1 4NH",
                    Latitude = 51.549112,
                    Longitude = -0.07934
                },
                new Cafe
                {
                    Id = 2,
                    Name = "Shoreditch Grind",
                    CoffeeRating = 4.75,
                    AtmosphereRating = 3.5,
                    NumberOfVotes = 320,
                    Address = "213 Old Street",
                    PostCode = "EC1V 9NR",
                    Latitude = 51.526,
                    Longitude = -0.088196
                },
                new Cafe
                {
                    Id = 3,
                    Name = "Prufrock Café",
                    CoffeeRating = 5,
                    AtmosphereRating = 3.5,
                    NumberOfVotes = 527,
                    Address = "23-25 Leather Ln",
                    PostCode = "EC1N 7TE",
                    Latitude = 51.519933,
                    Longitude = -0.109494
                },
                new Cafe
                {
                    Id = 4,
                    Name = "Railroad",
                    CoffeeRating = 4,
                    AtmosphereRating = 4,
                    NumberOfVotes = 127,
                    Address = "120-122 Morning Ln",
                    PostCode = "E9 6LH",
                    Latitude = 51.546979,
                    Longitude = -0.049501
                },
                new Cafe
                {
                    Id = 5,
                    Name = "Look Mum No Hands",
                    CoffeeRating = 4,
                    AtmosphereRating = 3.5,
                    NumberOfVotes = 201,
                    Address = "49 Old Street",
                    PostCode = "EC1V 9HX",
                    Latitude = 51.523928,
                    Longitude = -0.097009
                },
                new Cafe
                {
                    Id = 6,
                    Name = "Monmouth Coffee",
                    CoffeeRating = 4,
                    AtmosphereRating = 3,
                    NumberOfVotes = 520,
                    Address = "27 Monmouth Street",
                    PostCode = "WC2H 9EU",
                    Latitude = 51.514371,
                    Longitude = -0.126778
                },
                new Cafe
                {
                    Id = 7,
                    Name = "Flat White",
                    CoffeeRating = 4.5,
                    AtmosphereRating = 4,
                    NumberOfVotes = 493,
                    Address = "17 Berwick Street",
                    PostCode = "W1D 4AG",
                    Latitude = 51.5136374,
                    Longitude = -0.1347474
                }
            };

        private readonly static IDictionary<Cafe, IEnumerable<Review>> CafeReviews = new Dictionary<Cafe, IEnumerable<Review>>(); 

        public Task<IEnumerable<Cafe>> GetAllCafes()
        {
            return Task.FromResult(AllCafes);
        }

        public async Task<IEnumerable<Review>> GetCafeReviews(int cafeId)
        {
            if (!CafeReviews.Any())
            {
                foreach (var cafe in AllCafes)
                {
                    var reviews = new List<Review>
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

                    if (cafe.Id == 4)
                    {
                        reviews.Add(
                            new Review
                            {
                                Comment = "My favourite coffeeshop in town!",
                                SubmittedDate = DateTime.Today.AddDays(-5),
                                CoffeeRating = 4.5,
                                SubmittedBy = DesignIdentityService.CurrentUserIdentity
                            });
                    }

                    CafeReviews[cafe] = reviews;
                }
            }

            return CafeReviews[AllCafes.First(cafe => cafe.Id == cafeId)];
        }

        public async Task SaveCafeReview(int cafeId, Review review)
        {
            var cafe = AllCafes.First(c => c.Id == cafeId);
            var reviews = CafeReviews[cafe].Where(rev => rev.SubmittedBy != review.SubmittedBy).ToList();
            reviews.Insert(0, review);
            CafeReviews[cafe] = reviews;
        }
    }
}
