using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class MapViewModel : ViewModelBase, INavigationListener
    {
        private readonly INavigationService navigationService;
        private readonly IDataService dataService;
        private readonly IGeolocationProvider geolocationProvider;

        public ObservableCoordinate Centre { get; private set; }

        public ObservableCollection<CafeSummaryViewModel> Cafes { get; private set; }

        public MapViewModel(INavigationService navigationService, IDataService dataService, IGeolocationProvider geolocationProvider)
        {
            this.navigationService = navigationService;
            this.dataService = dataService;
            this.geolocationProvider = geolocationProvider;
            this.Cafes = new ObservableCollection<CafeSummaryViewModel>();
            this.Centre = new ObservableCoordinate
                            {
                                Latitude = 51.5214859,
                                Longitude = -0.1072635,
                            };
        }

        public async Task OnNavigatedTo(IDictionary<string, object> parameters)
        {
            // TODO: Hook cancellation task to "navigated from" event
            var locationTask = this.geolocationProvider.GetLocationAsync(new CancellationToken());

            if (!this.Cafes.Any())
            {
                foreach (var cafe in await this.dataService.GetAllCafes())
                {
                    this.Cafes.Add(new CafeSummaryViewModel(cafe, this.navigationService));
                }
            }

            var location = await locationTask;
            if (location != null)
            {
                this.Centre.Latitude = location.Latitude;
                this.Centre.Longitude = location.Longitude;
                this.RaisePropertyChanged(() => this.Centre);    
            }
        }
    }
}
