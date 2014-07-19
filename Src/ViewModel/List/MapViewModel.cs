using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeClientPrototype.ViewModel.Services;
using CoffeeClientPrototype.ViewModel.Support;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class MapViewModel : ViewModelBase, INavigationListener
    {
        private readonly INavigationService navigationService;
        private readonly IDataService dataService;
        private readonly IGeolocationProvider geolocationProvider;
        private CancellationTokenSource cancellationTokenSource = null;

        private CafeSummaryViewModel selectedCafe;

        public const string CentrePropertyName = "Centre";
        public const string CurrentLocationPropertyName = "CurrentLocation";

        public ObservableCoordinate CurrentLocation { get; private set; }

        public ObservableCoordinate Centre { get; private set; }

        public CafeSummaryViewModel SelectedCafe
        {
            get { return this.selectedCafe; }
            private set { this.Set(ref this.selectedCafe, value); }
        }

        public ObservableCollection<MapCafeSummaryViewModel> Cafes { get; private set; }

        public ObservableCollection<MapCafeSummaryViewModel> NearbyCafes { get; private set; }

        public RelayCommand RecentreAtCurrentLocation { get; private set; }

        public RelayCommand<CafeSummaryViewModel> SelectCafe { get; private set; }

        public MapViewModel(INavigationService navigationService, IDataService dataService, IGeolocationProvider geolocationProvider)
        {
            this.navigationService = navigationService;
            this.dataService = dataService;
            this.geolocationProvider = geolocationProvider;
            this.Cafes = new ObservableCollection<MapCafeSummaryViewModel>();
            this.NearbyCafes = new ObservableCollection<MapCafeSummaryViewModel>();
            this.CurrentLocation = new ObservableCoordinate();
            this.Centre = new ObservableCoordinate
                {
                    Latitude = 51.5214859,
                    Longitude = -0.1072635
                };

            this.RecentreAtCurrentLocation = new RelayCommand(() =>
                {
                    if (this.CurrentLocation == null) return;
                    this.Centre = this.CurrentLocation;
                    this.RaisePropertyChanged(() => this.Centre);
                },
                () => this.CurrentLocation != null);

            this.SelectCafe = new RelayCommand<CafeSummaryViewModel>(cafe =>
                {
                    this.SelectedCafe = cafe;
                });

#if DEBUG
            if (this.IsInDesignMode)
            {
                this.OnNavigatedTo(new Dictionary<string, object>());
            }
#endif
        }

        public async Task OnNavigatedTo(IDictionary<string, object> parameters)
        {
            this.RaisePropertyChanged(() => this.RecentreAtCurrentLocation);

            this.cancellationTokenSource = new CancellationTokenSource();
            var locationTask = this.geolocationProvider.GetLocationAsync(this.cancellationTokenSource.Token);
            await PopulateCafes();
            var location = await locationTask;
            this.PopulateNearbyCafes(location);
            this.PopulateCurrentLocation(location);
            this.PopulateCentre(location);
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
            this.RecentreAtCurrentLocation.RaiseCanExecuteChanged();
        }

        private void PopulateCentre(Coordinate location)
        {
            if (location == null) return;
            this.Centre.Latitude = location.Latitude;
            this.Centre.Longitude = location.Longitude;
            this.RaisePropertyChanged(() => this.Centre);
        }

        private void PopulateNearbyCafes(Coordinate location)
        {
            if (location == null) return;

            foreach (var cafe in this.Cafes)
            {
                cafe.DistanceToCurrentLocation = location.DistanceTo(cafe);
            }

            var nearbyCafes = this.Cafes
                .Where(cafe => location.DistanceTo(cafe) < 1500)
                .OrderBy(cafe => cafe.DistanceToCurrentLocation)
                .Take(5);

            this.NearbyCafes.Clear();
            foreach (var cafe in nearbyCafes)
            {
                this.NearbyCafes.Add(cafe);
            }
        }

        private void PopulateSelectedCafe()
        {
            if (this.selectedCafe != null) return;
            this.SelectedCafe = this.NearbyCafes.FirstOrDefault();
        }
    }
}

