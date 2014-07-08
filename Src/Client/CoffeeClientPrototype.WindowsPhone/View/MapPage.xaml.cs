using System;
using System.Collections.Specialized;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Details;
using CoffeeClientPrototype.ViewModel.List;

namespace CoffeeClientPrototype.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage
    {
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
            this.Map.LoadingStatusChanged += this.OnMapLoadingStatusChanged;
            this.NotifyNavigatedTo(e);
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
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.ViewModel.Cafes.CollectionChanged -= this.OnCafesCollectionChanged;
            this.Map.LoadingStatusChanged -= this.OnMapLoadingStatusChanged;
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
    }
}
