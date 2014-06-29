using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Annotations;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class CafeListItem : INotifyPropertyChanged
    {
        private string name;
        private double rating;
        private int numberOfVotes;
        private double latitude;
        private double longitude;
        private Photo photo;

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

        public Photo Photo
        {
            get { return this.photo; }
            set
            {
                if (Equals(value, photo)) return;
                this.photo = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand Navigate { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CafeListItem(Cafe model, INavigationService navigationService)
        {
            this.Navigate = new NavigateToCafeDetailsCommand(model, navigationService);
            this.Populate(model);
            this.PopulatePhoto(model);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Populate(Cafe model)
        {
            this.name = model.Name;
            this.longitude = model.Longitude;
            this.latitude = model.Latitude;
            this.rating = (model.CoffeeRating + model.AtmosphereRating) / 2;
            this.numberOfVotes = model.NumberOfVotes;
        }

        private void PopulatePhoto(Cafe model)
        {
            this.photo = model.Photos
                                .OrderByDescending(p => p.NumberOfVotes)
                                .FirstOrDefault();
        }
    }
}
