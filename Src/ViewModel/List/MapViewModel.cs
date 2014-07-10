using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoffeeClientPrototype.ViewModel.Services;
using CoffeeClientPrototype.ViewModel.Support;
using GalaSoft.MvvmLight;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class MapViewModel : ViewModelBase, INavigationListener
    {
        private readonly INavigationService navigationService;
        private readonly IDataService dataService;
        private readonly IGeolocationProvider geolocationProvider;
        private CancellationTokenSource cancellationTokenSource = null;

        private CafeSummaryViewModel selectedCafe;

        public const string CurrentLocationPropertyName = "CurrentLocation";

        public ObservableCoordinate CurrentLocation { get; private set; }

        public CafeSummaryViewModel SelectedCafe
        {
            get { return this.selectedCafe; }
            private set { this.Set(ref this.selectedCafe, value); }
        }

        public ObservableCollection<MapCafeSummaryViewModel> Cafes { get; private set; }

        public MapViewModel(INavigationService navigationService, IDataService dataService, IGeolocationProvider geolocationProvider)
        {
            this.navigationService = navigationService;
            this.dataService = dataService;
            this.geolocationProvider = geolocationProvider;
            this.Cafes = new ObservableCollection<MapCafeSummaryViewModel>();
            this.CurrentLocation = new ObservableCoordinate();
            new ObservableCoordinate
            {
                Latitude = 51.5214859,
                Longitude = -0.1072635
            };
#if DEBUG
            if (this.IsInDesignMode)
            {
                this.OnNavigatedTo(new Dictionary<string, object>());
            }
#endif
        }

        public async Task OnNavigatedTo(IDictionary<string, object> parameters)
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            var locationTask = this.geolocationProvider.GetLocationAsync(this.cancellationTokenSource.Token);
            await PopulateCafes();
            var location = await locationTask;
            this.PopulateCurrentLocation(location);
            this.PopulateEachCafeDistanceToCurrentLocation();
            this.PopulateSelectedCafe();
        }

        public void OnNavigatedFrom()
        {
            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource = null;
            }
        }

        private async Task PopulateCafes()
        {
            if (this.Cafes.Any()) return;
            foreach (var cafe in await this.dataService.GetAllCafes())
            {
                this.Cafes.Add(new MapCafeSummaryViewModel(cafe, this.navigationService));
            }
        }

        private void PopulateCurrentLocation(Coordinate location)
        {
            if (location == null) return;
            this.CurrentLocation.Latitude = location.Latitude;
            this.CurrentLocation.Longitude = location.Longitude;
            this.RaisePropertyChanged(() => this.CurrentLocation);
        }

        private void PopulateEachCafeDistanceToCurrentLocation()
        {
            if (this.CurrentLocation == null) return;
            foreach (var cafe in this.Cafes)
            {
                cafe.DistanceToCurrentLocation = cafe.DistanceTo(this.CurrentLocation);
            }
        }

        private void PopulateSelectedCafe()
        {
            if (this.CurrentLocation == null) return;
            if (this.selectedCafe != null) return;

            var cafesByDistance = this.Cafes.OrderBy(cafe => cafe.DistanceToCurrentLocation);

            var nearestCafe = cafesByDistance.FirstOrDefault();
            if (nearestCafe != null && nearestCafe.DistanceToCurrentLocation < 1500)
            {
                this.SelectedCafe = nearestCafe;
            }
        }
    }
}

