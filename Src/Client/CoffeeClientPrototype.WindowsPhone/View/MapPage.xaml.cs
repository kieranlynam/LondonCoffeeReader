using System;
using System.ComponentModel;
using CoffeeClientPrototype.ViewModel.List;
using System.Collections.Specialized;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;

namespace CoffeeClientPrototype.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage
    {
        private MapIcon currentLocationIcon;
        private bool hasCentredMap;

        public MapViewModel ViewModel
        {
            get { return ((ViewModelLocator)Application.Current.Resources["Locator"]).Map; }
        }

        public MapPage()
        {
            this.InitializeComponent();
            this.Map.Center = new Geopoint(
                new BasicGeoposition
                {
                    Latitude = 51.5214859,
                    Longitude = -0.1072635
                });
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.ViewModel.PropertyChanged += this.OnViewModelPropertyChanged;
            this.Map.LoadingStatusChanged += this.OnMapLoadingStatusChanged;
            this.NotifyNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.ViewModel.Cafes.CollectionChanged -= this.OnCafesCollectionChanged;
            this.ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            this.Map.LoadingStatusChanged -= this.OnMapLoadingStatusChanged;
        }

        private void OnMapLoadingStatusChanged(MapControl sender, object args)
        {
            if (this.Map.LoadingStatus == MapLoadingStatus.Loaded)
            {
                this.ViewModel.Cafes.CollectionChanged += this.OnCafesCollectionChanged;

                foreach (var cafe in this.ViewModel.Cafes.ToArray().Where(c => c.Latitude != 0))
                {
                    this.AddCafeToMap(cafe);
                }

                this.TryCenterMap();
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == MapViewModel.CurrentLocationPropertyName)
            {
                this.UpdateCurrentLocationIcon();
                this.TryCenterMap();
            }
        }

        private void UpdateCurrentLocationIcon()
        {
            var position = new BasicGeoposition
                {
                    Latitude = this.ViewModel.CurrentLocation.Latitude,
                    Longitude = this.ViewModel.CurrentLocation.Longitude
                };

            if (this.currentLocationIcon == null)
            {
                this.currentLocationIcon = new MapIcon
                    {
                        Location = new Geopoint(position),
                        NormalizedAnchorPoint = new Point(1.0, 0.5),
                    };

                this.Map.MapElements.Add(this.currentLocationIcon);
            }
            else
            {
                this.currentLocationIcon.Location = new Geopoint(position);
            }
        }

        private void OnCafesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var cafe in args.NewItems.OfType<CafeSummaryViewModel>().Where(c => c.Longitude != 0))
                {
                    this.AddCafeToMap(cafe);
                }

                this.TryCenterMap();
            }
        }

        private void AddCafeToMap(CafeSummaryViewModel cafe)
        {
            var position = new BasicGeoposition
                {
                    Latitude = cafe.Latitude,
                    Longitude = cafe.Longitude
                };
            var icon = new MapIcon
                {
                    Location = new Geopoint(position),
                    NormalizedAnchorPoint = new Point(1.0, 0.5),
                    Title = cafe.Name
                };

            this.Map.MapElements.Add(icon);
        }

        private async void TryCenterMap()
        {
            if (this.hasCentredMap) return;
            if (this.Map.LoadingStatus != MapLoadingStatus.Loaded) return;
            if (!this.ViewModel.Cafes.Any()) return;
            if (this.ViewModel.CurrentLocation == null) return;

            var points = this.ViewModel
                .Cafes
                .Where(cafe => cafe.DistanceToCurrentLocation.HasValue && cafe.DistanceToCurrentLocation < 1500)
                .Select(cafe => new BasicGeoposition { Latitude = cafe.Latitude, Longitude = cafe.Longitude })
                .ToList();

            if (!points.Any())
            {
                return;
            }

            points.Add(
                new BasicGeoposition
                {
                    Latitude = this.ViewModel.CurrentLocation.Latitude,
                    Longitude = this.ViewModel.CurrentLocation.Longitude
                });

            var box = GeoboundingBox.TryCompute(points);
            this.hasCentredMap = await this.Map.TrySetViewBoundsAsync(box, null, MapAnimationKind.Bow);
        }
    }
}
