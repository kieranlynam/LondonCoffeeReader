using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class ListViewModel : ViewModelBase, INavigationListener
    {
        private readonly IDataService dataService;
        private readonly INavigationService navigationService;


        public ObservableCollection<ListItemViewModel> NearbyCafes { get; private set; }

        public ObservableCollection<ListItemViewModel> BestCafes { get; private set; }

        public RelayCommand ShowMap { get; private set; }

        public ListViewModel(IDataService dataService, INavigationService navigationService)
        {
            this.dataService = dataService;
            this.navigationService = navigationService;
            this.BestCafes = new ObservableCollection<ListItemViewModel>();
            this.NearbyCafes = new ObservableCollection<ListItemViewModel>();
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
            this.PopulateNearbyCafes(cafes);
        }

        private void PopulateBestCafes(IEnumerable<Cafe> cafes)
        {
            var items = cafes
                .OrderByDescending(cafe => (cafe.CoffeeRating + cafe.AtmosphereRating) / 2)
                .ThenByDescending(cafe => cafe.NumberOfVotes)
                .Take(10)
                .Select(CreateCafeListItem);

            this.BestCafes.Clear();
            foreach (var item in items)
            {
                this.BestCafes.Add(item);
            }
        }

        private void PopulateNearbyCafes(IEnumerable<Cafe> cafes)
        {
            var items = cafes.Select(CreateCafeListItem);

            this.NearbyCafes.Clear();
            foreach (var item in items)
            {
                this.NearbyCafes.Add(item);
            }
        }

        private ListItemViewModel CreateCafeListItem(Cafe cafe)
        {
            return new ListItemViewModel(cafe, this.navigationService);
        }

        private void OnShowMapExecuted()
        {
            this.navigationService.Navigate("Map");
        }
    }
}
