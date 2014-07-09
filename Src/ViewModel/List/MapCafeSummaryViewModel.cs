using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class MapCafeSummaryViewModel : CafeSummaryViewModel
    {
        private double distanceToCurrentLocation;

        public double DistanceToCurrentLocation
        {
            get { return this.distanceToCurrentLocation; }
            set { this.Set(ref this.distanceToCurrentLocation, value); }
        }

        public MapCafeSummaryViewModel(Cafe model, INavigationService navigationService) : base(model, navigationService)
        {
        }
    }
}
