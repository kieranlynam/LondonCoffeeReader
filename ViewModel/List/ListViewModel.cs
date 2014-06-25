using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class ListViewModel : ViewModelBase
    {
        private readonly IDataService dataService;

        public ListViewModel(IDataService dataService)
        {
            this.dataService = dataService;
            this.BestCafes = new ObservableCollection<CafeListItem>();
            this.NearByCafes = new ObservableCollection<CafeListItem>();
        }

        public ObservableCollection<CafeListItem> NearByCafes { get; private set; }

        public ObservableCollection<CafeListItem> BestCafes { get; private set; }

        public async Task OnNavigatedTo()
        {
            var cafes = await this.dataService.GetAllCafes();

            foreach (var cafeItem in cafes.Select(CafeListItem.FromModel))
            {
                this.BestCafes.Add(cafeItem);
            }
        }
    }
}
