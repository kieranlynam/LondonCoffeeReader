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

        private double rating;
        private int numberOfVotes;

        public string Name { get; private set; }

        public double Rating
        {
            get { return this.rating; }
            private set { this.Set(ref this.rating, value); }
        }

        public int NumberOfVotes
        {
            get { return this.numberOfVotes; }
            private set { this.Set(ref this.numberOfVotes, value); }
        }

        public double Longitude { get; private set; }

        public double Latitude { get; private set; }

        public string Address { get; private set; }

        public string PostCode { get; private set; }

        public DetailsViewModel(IDataService dataService, INavigationService navigationService)
        {
            this.dataService = dataService;
        }

        public async Task OnNavigatedTo(IDictionary<string, object> parameters)
        {
            var cafe = await GetCafe((int) parameters["Id"]);
            this.Populate(cafe);
        }

        private void Populate(Cafe cafe)
        {
            this.Name = cafe.Name;
            this.RaisePropertyChanged(() => this.Name);

            this.Address = cafe.Address;
            this.RaisePropertyChanged(() => this.Address);

            this.PostCode = cafe.PostCode;
            this.RaisePropertyChanged(() => this.PostCode);

            this.Longitude = cafe.Longitude;
            this.RaisePropertyChanged(() => this.Longitude);

            this.Latitude = cafe.Latitude;
            this.RaisePropertyChanged(() => this.Latitude);

            this.Rating = cafe.Rating;
            this.NumberOfVotes = cafe.NumberOfVotes;
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
