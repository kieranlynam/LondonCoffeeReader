using System;
using System.Threading;
using System.Threading.Tasks;
using CoffeeClientPrototype.ViewModel.Services;

namespace ViewModel.Tests
{
    public class MockGeolocationProvider : IGeolocationProvider
    {
        public Coordinate CurrentLocation { get; set; }

        public bool IsEnabled { get; set; }

        public MockGeolocationProvider()
        {
            this.CurrentLocation = Coordinate.Origin;
            this.IsEnabled = true;
        }

        public Task<Coordinate> GetLocationAsync(CancellationToken cancellationToken)
        {
            if (this.IsEnabled)
            {
                return Task.FromResult(this.CurrentLocation);
            }

            throw new NotSupportedException("Geolocation is disabled!");
        }
    }
}