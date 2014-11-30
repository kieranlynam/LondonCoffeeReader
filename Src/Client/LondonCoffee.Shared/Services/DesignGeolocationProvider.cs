using System.Threading;
using System.Threading.Tasks;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.Services
{
    public sealed class DesignGeolocationProvider : IGeolocationProvider
    {
        public Task<Coordinate> GetLocationAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new Coordinate(51.5214859, -0.1072635));
        }
    }
}
