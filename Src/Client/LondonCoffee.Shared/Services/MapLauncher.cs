using System;
using System.Threading.Tasks;
using Windows.System;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.Services
{
    public sealed class MapLauncher : IMapLauncher
    {
        public async Task<bool> Launch(double longitude, double latitude, string name)
        {
            var uriToLaunch = string.Format(
                @"ms-drive-to:?destination.latitude={0}&destination.longitude={1}&destination.name={2}",
                latitude, longitude, name);

            return await Launcher.LaunchUriAsync(new Uri(uriToLaunch));
        }
    }
}
