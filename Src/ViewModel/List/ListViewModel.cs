using System;
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
        private readonly INavigationService navigationService;
        private readonly IGeolocationProvider geolocationProvider;
        private CancellationTokenSource cancellationTokenSource = null;

        public ObservableCollection<CafeSummaryViewModel> NearbyCafes { get; private set; }

        public ObservableCollection<CafeSummaryViewModel> BestCafes { get; private set; }

        public RelayCommand ShowMap { get; private set; }

        public ListViewModel(IDataService dataService, INavigationService navigationService, IGeolocationProvider geolocationProvider)
        {
            this.dataService = dataService;
            this.navigationService = navigationService;
            this.geolocationProvider = geolocationProvider;
            this.BestCafes = new ObservableCollection<CafeSummaryViewModel>();
            this.NearbyCafes = new ObservableCollection<CafeSummaryViewModel>();
            this.ShowMap = new RelayCommand(this.OnShowMapExecuted);

#if DEBUG
            if (this.IsInDesignMode)
            {
                this.OnNavigatedTo();
            }
#endif
        }

        public async Task OnNavigatedTo(IDictionary<string,object> parameters = null)
        {
            var cafes = await this.dataService.GetAllCafes();
            this.PopulateBestCafes(cafes);

            this.cancellationTokenSource = new CancellationTokenSource();
            var location = await this.geolocationProvider.GetLocationAsync(this.cancellationTokenSource.Token);
            this.PopulateNearbyCafes(location, cafes);
        }

        public void OnNavigatedFrom()
        {
            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource = null;
            }
        }

        private void PopulateBestCafes(IEnumerable<Cafe> cafes)
        {
            var items = cafes
                .OrderByDescending(cafe => (cafe.CoffeeRating + cafe.AtmosphereRating) / 2)
                .ThenByDescending(cafe => cafe.NumberOfVotes)
                .Take(10)
                .Select(this.CreateCafeSummary);

            this.BestCafes.Clear();
            foreach (var item in items)
            {
                this.BestCafes.Add(item);
            }
        }

        private void PopulateNearbyCafes(Coordinate location, IEnumerable<Cafe> cafes)
        {
            var items = cafes
                .Select(this.CreateCafeSummary)
                .OrderBy(location.DistanceTo);

            this.NearbyCafes.Clear();
            foreach (var item in items)
            {
                this.NearbyCafes.Add(item);
            }
        }

        private CafeSummaryViewModel CreateCafeSummary(Cafe cafe)
        {
            return new CafeSummaryViewModel(cafe, this.navigationService);
        }

        private void OnShowMapExecuted()
        {
            this.navigationService.Navigate("Map");
        }
    }
}
