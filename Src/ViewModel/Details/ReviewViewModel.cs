using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Annotations;

namespace CoffeeClientPrototype.ViewModel.Details
{
    public class ReviewViewModel : INotifyPropertyChanged
    {
        private string comment;
        private double? coffeeRating;
        private double? atmosphereRating;
        private string submittedBy;
        private DateTime submittedDate;

        public string Comment
        {
            get { return this.comment; }
            set
            {
                if (value == this.comment) return;
                this.comment = value;
                this.OnPropertyChanged();
            }
        }

        public double? CoffeeRating
        {
            get { return coffeeRating; }
            set
            {
                if (value.Equals(this.coffeeRating)) return;
                this.coffeeRating = value;
                this.OnPropertyChanged();
            }
        }

        public double? AtmosphereRating
        {
            get { return this.atmosphereRating; }
            set
            {
                if (value.Equals(this.atmosphereRating)) return;
                this.atmosphereRating = value;
                this.OnPropertyChanged();
            }
        }

        public string SubmittedBy
        {
            get { return this.submittedBy; }
            set
            {
                if (value == this.submittedBy) return;
                this.submittedBy = value;
                this.OnPropertyChanged();
            }
        }

        public DateTime SubmittedDate
        {
            get { return this.submittedDate; }
            set
            {
                if (value.Equals(this.submittedDate)) return;
                this.submittedDate = value;
                this.OnPropertyChanged();
            }
        }

        public ReviewViewModel(Review model)
        {
            this.Comment = model.Comment;
            this.CoffeeRating = model.CoffeeRating;
            this.AtmosphereRating = model.AtmosphereRating;
            this.SubmittedBy = model.SubmittedBy;
            this.SubmittedDate = model.SubmittedDate;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
