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

        private string comment;
        private Cafe associatedCafe;
        private double? coffeeRating;
        private double? atmosphereRating;

        public string Comment
        {
            get { return this.comment; }
            set
            {
                var before = this.CanExecuteSubmit();
                if (this.Set(ref this.comment, value))
                {
                    var after = this.CanExecuteSubmit();
                    if (before != after)
                    {
                        this.Submit.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        public double? CoffeeRating
        {
            get { return this.coffeeRating; }
            set
            {
                var before = this.CanExecuteSubmit();
                if (this.Set(ref this.coffeeRating, value))
                {
                    var after = this.CanExecuteSubmit();
                    if (before != after)
                    {
                        this.Submit.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        public double? AtmosphereRating
        {
            get { return this.atmosphereRating; }
            set
            {
                var before = this.CanExecuteSubmit();
                if (this.Set(ref this.atmosphereRating, value))
                {
                    var after = this.CanExecuteSubmit();
                    if (before != after)
                    {
                        this.Submit.RaiseCanExecuteChanged();
                    }
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

            if (string.IsNullOrEmpty(this.comment))
            {
                return false;
            }

            if (!this.coffeeRating.HasValue)
            {
                return false;
            }

            if (!this.atmosphereRating.HasValue)
            {
                return false;
            }
            
            return true;
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
        }
    }
}
