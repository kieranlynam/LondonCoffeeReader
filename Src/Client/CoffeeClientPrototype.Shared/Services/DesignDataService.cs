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
                    Name = "Tina, we salute you",
                    CoffeeRating = 4,
                    AtmosphereRating = 5,
                    NumberOfVotes = 250,
                    Address = "47 King Henry's Walk",
                    PostCode = "N1 4NH",
                    Latitude = 51.549112,
                    Longitude = -0.07934,
                    Photos = new[]
                    {
                        new Photo
                        {
                            SubmittedBy = "Ashley Kang",
                            NumberOfVotes = 100,
                            ImageUri = new Uri("http://keelertornero.com/blog/wp-content/uploads/2011/11/TWSY.jpg")
                        }
                    }
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
                    Longitude = -0.088196,
                    Photos = new[]
                    {
                        new Photo
                        {
                            SubmittedBy = "Kieran Lynam",
                            NumberOfVotes = 100,
                            ImageUri = new Uri("http://intertitles.com/wp-content/uploads/2013/01/shoreditch_grind.jpg")
                        },
                        new Photo
                        {
                            SubmittedBy = "Kieran Lynam",
                            NumberOfVotes = 100,
                            ImageUri = new Uri("http://www.fluidnetwork.co.uk/gfx/venues/22551/shoreditch_grind_cafe_london.jpg")
                        },
                        new Photo
                        {
                            SubmittedBy = "Kieran Lynam",
                            NumberOfVotes = 70,
                            ImageUri = new Uri("https://c2.staticflickr.com/8/7178/6895462310_2a71a5f3ef_z.jpg")
                        },
                        new Photo
                        {
                            SubmittedBy = "Kieran Lynam",
                            NumberOfVotes = 25,
                            ImageUri = new Uri("http://lh3.ggpht.com/-NAGfMVwzr8I/TfHXUo7zpHI/AAAAAAAAAj4/MFdgfVSQHuY/bloggerPlus.jpg")
                        }
                    }
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
                    Longitude = -0.109494,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://www.prufrockcoffee.com/wp-content/themes/prufrocktheme/library/images/home-cafe-02.jpg")
                        }
                    }
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
                    Longitude = -0.049501,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://file-magazine.com/wp-content/uploads/Railroad.jpeg")
                        }
                    }
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
                    Longitude = -0.097009,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://thesugarmagnolia.files.wordpress.com/2011/02/bike-cafes-londons-latest-hangouts.jpg")
                        }
                    }
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
                    Longitude = -0.126778,
                    Photos = new[]
                    {
                        new Photo
                        {
                            NumberOfVotes = 20,
                            ImageUri = new Uri("http://www.monmouthcoffee.co.uk/store/image/file/0g/cp/5m7rau/Covent-Garden-Shop.jpg")
                        },
                        new Photo
                        {
                            NumberOfVotes = 10,
                            ImageUri = new Uri("http://www.urbanistgame.com/wp-content/uploads/2011/12/m1.jpg")
                        }
                    }
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
                    Longitude = -0.1347474,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://themodelsoffice.com/wp-content/uploads/2013/01/Flat-White-3-e1357925027110.jpg")
                        }
                    }
                },
                new Cafe
                {
                    Id = 8,
                    Name = "Haggerston Espresso Room",
                    CoffeeRating = 4.5,
                    AtmosphereRating = 3.5,
                    NumberOfVotes = 290,
                    Address = "13 Downham Rd",
                    PostCode = "N1 5AA",
                    Latitude = 51.538976,
                    Longitude = -0.077686,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://2.bp.blogspot.com/-Z60GfhWcq8I/T2DO9-OtdCI/AAAAAAAAENI/8A_WyxsS80E/s1600/her.jpg")
                        }
                    }
                },
                new Cafe
                {
                    Id = 9,
                    Name = "Department of Coffee and Social Affairs",
                    CoffeeRating = 4.25,
                    AtmosphereRating = 4,
                    NumberOfVotes = 324,
                    Address = "14-16 Leather Lane",
                    PostCode = "EC1N 7SU",
                    Latitude = 51.51945,
                    Longitude = -0.109094,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("https://c2.staticflickr.com/8/7129/6930994854_c619c0cd1e_z.jpg")
                        }
                    }
                },
                new Cafe
                {
                    Id = 10,
                    Name = "Protein by Dunnefrankowski",
                    CoffeeRating = 4.5,
                    AtmosphereRating = 3.75,
                    NumberOfVotes = 94,
                    Address = "18 Hewett Street",
                    PostCode = "EC2A 3NN",
                    Latitude = 51.5231171,
                    Longitude = -0.0797379,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://now-here-this.timeout.com/wp-content/uploads/2012/12/Unknown-198.jpg")
                        },
                        new Photo
                        {
                            ImageUri = new Uri("https://protein-publisher-prod.s3.amazonaws.com/uploads/medium_PBDF_VF-3.jpg")
                        }
                    }
                },
                new Cafe
                {
                    Id = 11,
                    Name = "63 Wilton Way",
                    CoffeeRating = 4.25,
                    AtmosphereRating = 4.25,
                    NumberOfVotes = 102,
                    Address = "63 Wilton Way",
                    PostCode = "E8 1BG",
                    Latitude = 51.545701,
                    Longitude = -0.061426,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://media.tumblr.com/tumblr_ls72dgJrZS1qckwze.jpg")
                        }
                    }
                },
                new Cafe
                {
                    Id = 12,
                    Name = "Climpson and Sons",
                    CoffeeRating = 4.75,
                    AtmosphereRating = 4,
                    NumberOfVotes = 502,
                    Address = "67 Broadway Market",
                    PostCode = "E8 4PH",
                    Latitude = 51.53747,
                    Longitude = -0.061339,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://farm4.staticflickr.com/3354/3414211179_ca33c85689.jpg")
                        }
                    }
                },
                new Cafe
                {
                    Id = 13,
                    Name = "Talkhouse Coffee",
                    CoffeeRating = 4,
                    AtmosphereRating = 3.75,
                    NumberOfVotes = 75,
                    Address = "275 Portobello Rd",
                    PostCode = "E8 4PH",
                    Latitude = 51.517815,
                    Longitude = -0.206698,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://media-cdn.tripadvisor.com/media/photo-s/04/7a/b9/b3/talkhouse-coffee.jpg")
                        }
                    }
                },
                new Cafe
                {
                    Id = 14,
                    Name = "Nude Espresso",
                    CoffeeRating = 4.45,
                    AtmosphereRating = 3.85,
                    NumberOfVotes = 240,
                    Address = "19 Soho Square",
                    PostCode = "W1D 3QN",
                    Latitude = 51.515787,
                    Longitude = -0.131787,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://www.urban75.org/blog/wp-content/uploads/2012/03/nude-espresso-soho-square-02.jpg")
                        }
                    }
                },
                new Cafe
                {
                    Id = 15,
                    Name = "Store Street Espresso",
                    CoffeeRating = 4,
                    AtmosphereRating = 4.25,
                    NumberOfVotes = 102,
                    Address = "40 Store St",
                    PostCode = "WC1E 7DB",
                    Latitude = 51.520115,
                    Longitude = -0.130749,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://payload82.cargocollective.com/1/8/274917/3952612/Store-Street-Espresso-Outside.jpg")
                        },
                        new Photo
                        {
                            ImageUri = new Uri("http://ic.pics.livejournal.com/cafediaries/21119118/101017/101017_640.jpg")
                        }
                    }
                },
                new Cafe
                {
                    Id = 16,
                    Name = "The Liberty of Norton Folgate",
                    CoffeeRating = 4.1,
                    AtmosphereRating = 3.25,
                    NumberOfVotes = 98,
                    Address = "201 Bishopsgate",
                    PostCode = "EC2M 3UG",
                    Latitude = 51.520864,
                    Longitude = -0.079039,
                    Photos = new[]
                    {
                        new Photo
                        {
                            ImageUri = new Uri("http://www.broadgate.co.uk/Content/Upload/DetailImages/120_detail_1.jpg")
                        },
                        new Photo
                        {
                            ImageUri = new Uri("http://sweetsoundofcoffee.files.wordpress.com/2012/08/img_08182.jpg")
                        }
                    }
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
