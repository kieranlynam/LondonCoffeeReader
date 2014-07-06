using System.Threading.Tasks;

namespace CoffeeClientPrototype.ViewModel.Services
{
    public interface IMapLauncher
    {
        Task<bool> Launch(double longitude, double latitude, string name);
    }
}