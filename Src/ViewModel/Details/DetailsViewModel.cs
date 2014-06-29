using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IIdentityService identityService;

        private double coffeeRating;
        private double atmosphereRating;
        private int numberOfVotes;

        public string Name { get; private set; }

        public double Longitude { get; private set; }

        public double Latitude { get; private set; }

        public string Address { get; private set; }

        public string PostCode { get; private set; }

        public double CoffeeRating
        {
            get { return this.coffeeRating; }
            private set { this.Set(ref this.coffeeRating, value); }
        }

        public double AtmosphereRating
        {
            get { return this.atmosphereRating; }
            private set { this.Set(ref this.atmosphereRating, value); }
        }

        public int NumberOfVotes
        {
            get { return this.numberOfVotes; }
            private set { this.Set(ref this.numberOfVotes, value); }
        }

        public ObservableCollection<Photo> Photos { get; private set; }
        
        public ObservableCollection<Review> Reviews { get; private set; }
        
        public UserReviewViewModel UserReview { get; private set; }

        public DetailsViewModel(IDataService dataService, IIdentityService identityService)
        {
            this.dataService = dataService;
            this.identityService = identityService;
            this.Photos = new ObservableCollection<Photo>();
            this.Reviews = new ObservableCollection<Review>();
            this.UserReview = new UserReviewViewModel(this.dataService, this.identityService);
        }

        public Task OnNavigatedTo(IDictionary<string, object> parameters)
        {
            var cafeId = (int) parameters["Id"];

            var detailsTask = this.GetCafe(cafeId)
                .ContinueWith(task => Populate(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
            var reviewsTask = this.dataService.GetCafeReviews(cafeId)
                .ContinueWith(task => Populate(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
            return Task.WhenAll(detailsTask, reviewsTask);
        }

        private void Populate(Cafe cafe)
        {
            // TODO: Unassign during navigate away?
            this.UserReview.AssociatedCafe = cafe;

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

            this.CoffeeRating = cafe.CoffeeRating;
            this.AtmosphereRating = cafe.AtmosphereRating;
            this.NumberOfVotes = cafe.NumberOfVotes;

            foreach (var photo in cafe.Photos)
            {
                this.Photos.Add(photo);
            }
        }

        private void Populate(IEnumerable<Review> reviews)
        {
            var sorted = reviews
                .OrderByDescending(comment => comment.SubmittedDate);
            foreach (var review in sorted)
            {
                this.Reviews.Add(review);
            }
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
