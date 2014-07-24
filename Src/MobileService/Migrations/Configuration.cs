using System.Linq;
using londoncoffeeService.DataObjects;

namespace londoncoffeeService.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<londoncoffeeService.Models.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "londoncoffeeService.Models.DataContext";
        }

        protected override void Seed(Models.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            if (context.Cafes.Any())
            {
                return;
            }

            context.Cafes.AddOrUpdate(
                c => c.Id,
                new CafeData
                {
                    Id = "1",
                    Name = "Tina, we salute you",
                    CoffeeRating = 4,
                    AtmosphereRating = 5,
                    NumberOfVotes = 250,
                    Address = "47 King Henry's Walk",
                    PostCode = "N1 4NH",
                    Latitude = 51.549112,
                    Longitude = -0.07934,
                },
                new CafeData
                {
                    Id = "2",
                    Name = "Shoreditch Grind",
                    CoffeeRating = 4.75,
                    AtmosphereRating = 3.5,
                    NumberOfVotes = 320,
                    Address = "213 Old Street",
                    PostCode = "EC1V 9NR",
                    Latitude = 51.526,
                    Longitude = -0.088196,
                });
        }
    }
}
