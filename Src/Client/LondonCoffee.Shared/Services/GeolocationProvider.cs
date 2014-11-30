using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.Services
{
    public sealed class GeolocationProvider : IGeolocationProvider
    {
        private readonly Geolocator geolocator;
        private Geoposition lastPosition;

        public GeolocationProvider()
        {
            this.geolocator = new Geolocator
                                {
                                    DesiredAccuracy = PositionAccuracy.Default,
                                    MovementThreshold = 250
                                };
#if WINDOWS_PHONE_APP
            geolocator.DesiredAccuracyInMeters = 100;
#endif

            this.geolocator.PositionChanged += OnPositionChanged;
        }

        public async Task<Coordinate> GetLocationAsync(CancellationToken cancellationToken)
        {
            if (this.lastPosition == null)
            {
                try
                {
                    this.lastPosition = await geolocator
                            .GetGeopositionAsync(TimeSpan.FromMinutes(15), TimeSpan.FromSeconds(45))
                            .AsTask(cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    return null;
                }
            }

            return new Coordinate(
                this.lastPosition.Coordinate.Point.Position.Latitude,
                this.lastPosition.Coordinate.Point.Position.Longitude);
        }

        private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            this.lastPosition = args.Position;
        }
    }
}
