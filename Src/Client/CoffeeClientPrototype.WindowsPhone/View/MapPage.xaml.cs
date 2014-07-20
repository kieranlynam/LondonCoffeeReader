using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
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
        private bool hasMapLoaded = false;

        private readonly IDictionary<MapIcon, CafeSummaryViewModel> cafeMapIcons = new Dictionary<MapIcon, CafeSummaryViewModel>(); 

        public MapViewModel ViewModel
        {
            get { return ((ViewModelLocator)Application.Current.Resources["Locator"]).Map; }
        }

        public MapPage()
        {
            this.InitializeComponent();
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
            this.NotifyNavigatedFrom();
            this.cafeMapIcons.Clear();
        }

        private void OnMapLoadingStatusChanged(MapControl sender, object args)
        {
            if (this.Map.LoadingStatus == MapLoadingStatus.Loaded)
            {
                // the LoadingStatus fires multiple time - only interested in the intial load
                this.Map.LoadingStatusChanged -= this.OnMapLoadingStatusChanged;

                if (!this.hasMapLoaded)
                {
                    this.hasMapLoaded = true;
                    this.ViewModel.Cafes.CollectionChanged += this.OnCafesCollectionChanged;
                    this.Dispatcher.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () =>
                        {
                            foreach (var cafe in this.ViewModel.Cafes.ToArray().Where(c => c.Latitude != 0))
                            {
                                this.AddCafeToMap(cafe);
                            }
                        });
                }
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case MapViewModel.CentrePropertyName:
                    this.TryCenterMap();
                    break;
                case MapViewModel.CurrentLocationPropertyName:
                    this.UpdateCurrentLocationIcon();
                    break;
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
                var image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapCurrentLocation.png"));

                this.currentLocationIcon = new MapIcon
                    {
                        Location = new Geopoint(position),
                        NormalizedAnchorPoint = new Point(1.0, 0.5),
                        Image = image
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
            }
        }

        private void OnMapTapped(MapControl sender, MapInputEventArgs args)
        {
            var icon = sender.FindMapElementsAtOffset(args.Position).OfType<MapIcon>().FirstOrDefault();

            if (icon != null)
            {
                CafeSummaryViewModel cafe;
                if (this.cafeMapIcons.TryGetValue(icon, out cafe))
                {
                    this.ViewModel.SelectCafe.Execute(cafe);
                }    
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

            this.cafeMapIcons.Add(icon, cafe);
        }

        private async void TryCenterMap()
        {
            if (this.ViewModel.Centre == null) return;

            var centrePoint = new Geopoint(
                    new BasicGeoposition
                    {
                        Latitude = this.ViewModel.Centre.Latitude,
                        Longitude = this.ViewModel.Centre.Longitude
                    });

            if (!this.hasMapLoaded)
            {
                // the map has not loaded, so setting view doesn't work well.
                // just update the centre so that when it loads it will load here.
                this.Map.Center = centrePoint;
                return;
            }

            if (this.ViewModel.NearbyCafes.Any())
            {
                var points = this.ViewModel
                    .NearbyCafes
                    .Select(cafe => new BasicGeoposition { Latitude = cafe.Latitude, Longitude = cafe.Longitude })
                    .ToList();
                points.Add(centrePoint.Position);
            
                var box = GeoboundingBox.TryCompute(points);
                await this.Map.TrySetViewBoundsAsync(box, null, MapAnimationKind.Bow);
            }
            else
            {
                await this.Map.TrySetViewAsync(centrePoint, null, null, null, MapAnimationKind.Linear);
            }
        }
    }
}
