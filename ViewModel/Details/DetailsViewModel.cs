using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;

namespace CoffeeClientPrototype.ViewModel.Details
{
    public class DetailsViewModel : ViewModelBase, INavigationListener
    {
        private readonly IDataService dataService;
        private readonly INavigationService navigationService;

        public string Name { get; private set; }

        public DetailsViewModel(IDataService dataService, INavigationService navigationService)
        {
            this.dataService = dataService;
            this.navigationService = navigationService;
        }

        public async Task OnNavigatedTo(IDictionary<string, object> parameters)
        {
            var cafe = await GetCafe((int) parameters["Id"]);

            this.Name = cafe.Name;
            this.RaisePropertyChanged(() => this.Name);
        }

        private async Task<Cafe> GetCafe(int cafeId)
        {
            var cafes = await this.dataService.GetAllCafes();
            var cafe = cafes.FirstOrDefault(c => c.Id == cafeId);
            if (cafe == null)
            {
                throw new ArgumentException(
                    string.Format("Could not retrieve cafe {0} from data service", cafeId));
            }
            return cafe;
        }
    }
}
