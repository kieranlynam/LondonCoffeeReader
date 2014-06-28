using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoffeeClientPrototype.ViewModel.Details
{
    public class UserReviewViewModel : ViewModelBase
    {
        private readonly IDataService dataService;

        private string comment;
        private Cafe associatedCafe;

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

        public UserReviewViewModel(IDataService dataService)
        {
            this.dataService = dataService;
            this.Submit = new RelayCommand(this.OnSubmitExecuted, this.CanExecuteSubmit);
        }

        private bool CanExecuteSubmit()
        {
            if (this.AssociatedCafe == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(this.comment))
            {
                return false;
            }
            
            return true;
        }

        private void OnSubmitExecuted()
        {
            var review = new Review
                {
                    Comment = this.comment
                };
            this.dataService.SubmitCafeReview(
                this.AssociatedCafe.Id,
                review);
        }
    }
}
