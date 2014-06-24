using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class ListViewModel : ViewModelBase
    {
        public ObservableCollection<CafeListItem> NearByCafes { get; private set; }

        public ObservableCollection<CafeListItem> BestCafes { get; private set; }
    }
}
