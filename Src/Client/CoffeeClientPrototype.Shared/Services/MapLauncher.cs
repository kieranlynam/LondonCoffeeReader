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
            var uri = new Uri(string.Format("bingmaps:?cp={0}~{1}&lvl=16&collection=point.{0}_{1}_{2}", latitude, longitude, name));
            return await Launcher.LaunchUriAsync(uri);
        }
    }
}
