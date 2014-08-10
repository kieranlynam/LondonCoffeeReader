using System;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoffeeClientPrototype.ViewModel.Details
{
    public class ReviewViewModel : ObservableObject
    {
        private readonly IDataService dataService;
        private readonly IIdentityService identityService;

        private bool isDirty;
        private string reviewId;
        private string comment;
        private Cafe associatedCafe;
        private double? coffeeRating;
        private double? atmosphereRating;
        private string submittedBy;
        private DateTime? submittedDate;

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

        public string SubmittedBy
        {
            get { return this.submittedBy; }
            private set { this.Set(ref this.submittedBy, value); }
        }

        public DateTime? SubmittedDate
        {
            get { return this.submittedDate; }
            private set { this.Set(ref this.submittedDate, value); }
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

        public ReviewViewModel(IDataService dataService, IIdentityService identityService)
        {
            this.dataService = dataService;
            this.identityService = identityService;
            this.Submit = new RelayCommand(this.OnSubmitExecuted, this.CanExecuteSubmit);
        }

        public void Initialize(Review review = null)
        {
            this.Set(ref this.comment, review != null ? review.Comment : null);
            this.Set(ref this.coffeeRating, review != null ? review.CoffeeRating : null);
            this.Set(ref this.atmosphereRating, review != null ? review.AtmosphereRating : null);
            this.reviewId = review != null ? review.Id : null;
            this.SubmittedBy = review != null ? review.SubmittedBy : null;
            this.SubmittedDate = review != null ? review.SubmittedDate : (DateTime?) null;
            this.isDirty = false;
            this.Submit.RaiseCanExecuteChanged();
        }

        private bool CanExecuteSubmit()
        {
            if (this.submittedBy != null && this.submittedBy != this.identityService.CurrentUserId)
            {
                return false;
            }

            if (this.AssociatedCafe == null)
            {
                return false;
            }

            return this.isDirty;
        }

        private async void OnSubmitExecuted()
        {
            if (!this.identityService.IsAuthenticated)
            {
                if (!await this.identityService.AuthenticateAsync())
                {
                    return;
                }
            }

            if (!this.CanExecuteSubmit())
            {
                return;
            }

            if (this.comment != null)
            {
                var formatted = this.comment.Trim();
                formatted = formatted.Length > 0 ? formatted : null;
                this.comment = formatted;
                this.RaisePropertyChanged(() => this.Comment);
            }

            var review = new Review
                {
                    Id = this.reviewId,
                    CafeId = this.associatedCafe.Id,
                    Comment = this.comment,
                    SubmittedBy = this.identityService.CurrentUserId
                };
            if (this.coffeeRating.HasValue)
            {
                review.CoffeeRating = this.coffeeRating.Value;
            }
            if (this.atmosphereRating.HasValue)
            {
                review.AtmosphereRating = this.atmosphereRating.Value;
            }

            this.isDirty = false;
            this.Submit.RaiseCanExecuteChanged();

            if (review.Comment == null && review.CoffeeRating == null && review.AtmosphereRating == null)
            {
                return;
            }

            await this.dataService.SaveCafeReview(review);

            this.SubmittedBy = review.SubmittedBy;
            this.SubmittedDate = review.SubmittedDate;

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
