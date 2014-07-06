using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class MapViewModel : ViewModelBase, INavigationListener
    {
        private readonly INavigationService navigationService;
        private readonly IDataService dataService;

        public ObservableCollection<ListItemViewModel> Cafes { get; private set; }

        public MapViewModel(INavigationService navigationService, IDataService dataService)
        {
            this.navigationService = navigationService;
            this.dataService = dataService;
            this.Cafes = new ObservableCollection<ListItemViewModel>();
        }

        public async Task OnNavigatedTo(IDictionary<string, object> parameters)
        {
            if (!this.Cafes.Any())
            {
                foreach (var cafe in await this.dataService.GetAllCafes())
                {
                    this.Cafes.Add(new ListItemViewModel(cafe, this.navigationService));
                }
            }
        }
    }
}
