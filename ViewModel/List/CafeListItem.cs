using System.ComponentModel;
using System.Runtime.CompilerServices;
using CoffeeClientPrototype.ViewModel.Annotations;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class CafeListItem : INotifyPropertyChanged
    {
        private string name;
        private double rating;
        private int numberOfVotes;
        private double latitude;
        private double longitude;

        public string Name
        {
            get { return this.name; }
            set
            {
                if (value == this.name) return;
                this.name = value;
                this.OnPropertyChanged();
            }
        }

        public double Rating
        {
            get { return this.rating; }
            set
            {
                if (value.Equals(this.rating)) return;
                this.rating = value;
                this.OnPropertyChanged();
            }
        }

        public int NumberOfVotes
        {
            get { return this.numberOfVotes; }
            set
            {
                if (value == this.numberOfVotes) return;
                this.numberOfVotes = value;
                this.OnPropertyChanged();
            }
        }

        public double Latitude
        {
            get { return this.latitude; }
            set
            {
                if (value.Equals(this.latitude)) return;
                this.latitude = value;
                this.OnPropertyChanged();
            }
        }

        public double Longitude
        {
            get { return this.longitude; }
            set
            {
                if (value.Equals(this.longitude)) return;
                this.longitude = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
