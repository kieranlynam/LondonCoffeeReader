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

        public ObservableCollection<PhotoViewModel> Photos { get; private set; }
        
        public ObservableCollection<ReviewViewModel> Reviews { get; private set; }
        
        public ReviewViewModel CurrentIdentityReview { get; private set; }

        public DetailsViewModel(IDataService dataService, IIdentityService identityService)
        {
            this.dataService = dataService;
            this.identityService = identityService;
            this.Photos = new ObservableCollection<PhotoViewModel>();
            this.Reviews = new ObservableCollection<ReviewViewModel>();
            this.CurrentIdentityReview = new ReviewViewModel(this.dataService, this.identityService);
            this.CurrentIdentityReview.ReviewSubmitted += this.OnCurrentIdentityReviewSubmitted;

#if DEBUG
            if (this.IsInDesignMode)
            {
                this.Populate(cafeId: 2);
            }
#endif
        }

        public Task OnNavigatedTo(IDictionary<string, object> parameters)
        {
            var cafeId = (int) parameters["Id"];
            return this.Populate(cafeId);
        }

        public override void Cleanup()
        {
            base.Cleanup();
            this.CurrentIdentityReview.ReviewSubmitted -= this.OnCurrentIdentityReviewSubmitted;
        }

        private Task Populate(int cafeId)
        {
            this.Reviews.Clear();

            var detailsTask = this.GetCafe(cafeId)
                .ContinueWith(task => Populate(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
            var reviewsTask = this.dataService.GetCafeReviews(cafeId)
                .ContinueWith(task => Populate(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
            return Task.WhenAll(detailsTask, reviewsTask);
        }

        private void Populate(Cafe cafe)
        {
            this.CurrentIdentityReview.AssociatedCafe = cafe; 

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

            foreach (var photo in cafe.Photos.OrderByDescending(p => p.NumberOfVotes))
            {
                this.Photos.Add(new PhotoViewModel(photo));
            }
        }

        private void Populate(IEnumerable<Review> reviews)
        {
            Review reviewByCurrentIdentity = null;

            var sorted = reviews
                .OrderByDescending(review => review.SubmittedDate);
            foreach (var review in sorted)
            {
                if (!string.IsNullOrEmpty(review.Comment))
                {
                    this.Reviews.Add(this.CreateReviewViewModel(review));
                }

                if (reviewByCurrentIdentity != null) continue;
                if (this.identityService.Id == null) continue;
                if (review.SubmittedBy == this.identityService.Id)
                {
                    reviewByCurrentIdentity = review;
                }
            }

            this.CurrentIdentityReview.Initialize(reviewByCurrentIdentity);
            this.RaisePropertyChanged(() => this.CurrentIdentityReview);
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

        private void OnCurrentIdentityReviewSubmitted(object sender, ReviewSubmittedEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Review.Comment))
            {
                return;
            }

            var existingReview = this.Reviews.FirstOrDefault(review => review.SubmittedBy == args.Review.SubmittedBy);
            this.Reviews.Remove(existingReview);

            this.Reviews.Insert(0, this.CreateReviewViewModel(args.Review));
        }

        private ReviewViewModel CreateReviewViewModel(Review model)
        {
            var viewModel = new ReviewViewModel(this.dataService, this.identityService);
            viewModel.Initialize(model);
            return viewModel;
        }
    }
}
