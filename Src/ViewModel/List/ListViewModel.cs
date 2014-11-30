using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using CoffeeClientPrototype.ViewModel.Support;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class ListViewModel : ViewModelBase, INavigationListener
    {
        private readonly IDataService dataService;
        private readonly IBookmarkService bookmarkService;
        private readonly INavigationService navigationService;
        private readonly IGeolocationProvider geolocationProvider;
        private CancellationTokenSource cancellationTokenSource;

        public ObservableCollection<CafeSummaryViewModel> NearbyCafes { get; private set; }

        public ObservableCollection<CafeSummaryViewModel> BestCafes { get; private set; }

        public ObservableCollection<CafeSummaryViewModel> BookmarkedCafes { get; private set; }

        public RelayCommand ShowMap { get; private set; }

        public ListViewModel(IDataService dataService, IBookmarkService bookmarkService, INavigationService navigationService, IGeolocationProvider geolocationProvider)
        {
            this.dataService = dataService;
            this.bookmarkService = bookmarkService;
            this.navigationService = navigationService;
            this.geolocationProvider = geolocationProvider;
            this.BestCafes = new ObservableCollection<CafeSummaryViewModel>();
            this.NearbyCafes = new ObservableCollection<CafeSummaryViewModel>();
            this.BookmarkedCafes = new ObservableCollection<CafeSummaryViewModel>();
            this.ShowMap = new RelayCommand(this.OnShowMapExecuted);

#if DEBUG
            if (this.IsInDesignMode)
            {
                this.OnNavigatedTo();
            }
#endif
        }

        public async Task OnNavigatedTo(IDictionary<string, object> parameters = null)
        {
            this.cancellationTokenSource = new CancellationTokenSource();

            var getCafes = this.dataService.GetAllCafes();
            var getLocation = this.geolocationProvider
                                .GetLocationAsync(this.cancellationTokenSource.Token);
            var getBookmarks = this.bookmarkService.GetBookmarkedCafeIds();

            var cafes = await getCafes;
            var cafeItems = cafes.Select(CreateCafeSummary).ToArray();
            this.PopulateBestCafes(cafeItems);

            var location = await getLocation;
            this.PopulateNearbyCafes(location, cafeItems);

            var bookmarks = await getBookmarks;
            this.PopulateBookmarkedCafes(cafeItems, bookmarks);
        }

        public void OnNavigatedFrom()
        {
            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource = null;
            }
        }

        private void PopulateBestCafes(IEnumerable<CafeSummaryViewModel> cafes)
        {
            var items = cafes
                .OrderByDescending(summary => summary.Rating)
                .ThenByDescending(summary => summary.NumberOfVotes)
                .Take(10);

            this.BestCafes.Clear();
            foreach (var item in items)
            {
                this.BestCafes.Add(item);
            }
        }

        private void PopulateNearbyCafes(Coordinate location, IEnumerable<CafeSummaryViewModel> cafes)
        {
            if (location == null) return;

            foreach (var cafe in cafes)
            {
                cafe.DistanceToCurrentLocation = location.DistanceTo(cafe);
            }

            var items = cafes
                .Where(cafe => cafe.DistanceToCurrentLocation < 3000)
                .OrderBy(cafe => cafe.DistanceToCurrentLocation);

            this.NearbyCafes.Clear();
            foreach (var item in items)
            {
                this.NearbyCafes.Add(item);
            }
        }
        private void PopulateBookmarkedCafes(IEnumerable<CafeSummaryViewModel> cafes, IEnumerable<string> bookmarks)
        {
            var items = cafes
                .Where(cafe => bookmarks.Contains(cafe.Id))
                .OrderBy(cafe => cafe.Name);

            this.BookmarkedCafes.Clear();
            foreach (var item in items)
            {
                this.BookmarkedCafes.Add(item);
            }
        }

        private CafeSummaryViewModel CreateCafeSummary(Cafe cafe)
        {
            return new CafeSummaryViewModel(cafe, this.navigationService);
        }

        private void OnShowMapExecuted()
        {
            this.navigationService.NavigateToMap();
        }
    }
}
