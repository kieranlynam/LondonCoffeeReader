using System;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoffeeClientPrototype.ViewModel.Details
{
    public class UserReviewViewModel : ViewModelBase
    {
        private readonly IDataService dataService;
        private readonly IIdentityService identityService;

        private bool isDirty;
        private string comment;
        private Cafe associatedCafe;
        private double? coffeeRating;
        private double? atmosphereRating;

        public event EventHandler<ReviewSubmittedEventArgs> ReviewSubmitted; 

        public string Comment
        {
            get { return this.comment; }
            set
            {
                if (this.Set(ref this.comment, value))
                {
                    this.isDirty = true;
                    this.Submit.RaiseCanExecuteChanged();
                }
            }
        }

        public double? CoffeeRating
        {
            get { return this.coffeeRating; }
            set
            {
                if (this.Set(ref this.coffeeRating, value))
                {
                    this.isDirty = true;
                    this.Submit.RaiseCanExecuteChanged();
                }
            }
        }

        public double? AtmosphereRating
        {
            get { return this.atmosphereRating; }
            set
            {
                if (this.Set(ref this.atmosphereRating, value))
                {
                    this.isDirty = true;
                    this.Submit.RaiseCanExecuteChanged();
                }
            }
        }

        public Cafe AssociatedCafe
        {
            get
            {
                return this.associatedCafe;
            }
            set
            {
                if (this.Set(ref this.associatedCafe, value))
                {
                    this.Submit.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand Submit { get; private set; }

        public UserReviewViewModel(IDataService dataService, IIdentityService identityService)
        {
            this.dataService = dataService;
            this.identityService = identityService;
            this.Submit = new RelayCommand(this.OnSubmitExecuted, this.CanExecuteSubmit);
        }

        public void Initialize(Review review = null)
        {
            this.Set(ref this.comment, review != null ? review.Comment : null);
            this.Set(ref this.coffeeRating, review != null ? review.CoffeeRating : (double?) null);
            this.Set(ref this.atmosphereRating, review != null ? review.AtmosphereRating : (double?) null);
            this.isDirty = false;
            this.Submit.RaiseCanExecuteChanged();
        }

        private bool CanExecuteSubmit()
        {
            if (this.identityService.Id == null)
            {
                return false;
            }

            if (this.AssociatedCafe == null)
            {
                return false;
            }

            return this.isDirty;
        }

        private void OnSubmitExecuted()
        {
            var review = new Review
                {
                    Comment = this.comment,
                    SubmittedBy = this.identityService.Id
                };
            if (this.coffeeRating.HasValue)
            {
                review.CoffeeRating = this.coffeeRating.Value;
            }
            if (this.atmosphereRating.HasValue)
            {
                review.AtmosphereRating = this.atmosphereRating.Value;
            }

            this.dataService.SaveCafeReview(
                this.AssociatedCafe.Id,
                review);

            if (this.ReviewSubmitted != null)
            {
                this.ReviewSubmitted(this, new ReviewSubmittedEventArgs(review));
            }
        }
    }

    public class ReviewSubmittedEventArgs : EventArgs
    {
        public Review Review { get; private set; }

        public ReviewSubmittedEventArgs(Review review)
        {
            this.Review = review;
        }
    }
}
