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

        private string reviewId;
        private string comment;
        private Cafe associatedCafe;
        private double? coffeeRating;
        private double? atmosphereRating;
        private string submittedBy;
        private DateTime? submittedDate;

        public string Comment
        {
            get { return this.comment; }
        }

        public double? CoffeeRating
        {
            get { return this.coffeeRating; }
        }

        public double? AtmosphereRating
        {
            get { return this.atmosphereRating; }
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
            set { this.Set(ref this.associatedCafe, value); }
        }

        public ReviewViewModel(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public void Initialize(Review review = null)
        {
            this.Set(ref this.comment, review != null ? review.Comment : null);
            this.Set(ref this.coffeeRating, review != null ? review.CoffeeRating : null);
            this.Set(ref this.atmosphereRating, review != null ? review.AtmosphereRating : null);
            this.reviewId = review != null ? review.Id : null;
            this.SubmittedBy = review != null ? review.SubmittedBy : null;
            this.SubmittedDate = review != null ? review.SubmittedDate : (DateTime?) null;
        }
    }
}
