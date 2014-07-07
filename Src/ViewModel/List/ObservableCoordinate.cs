using GalaSoft.MvvmLight;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class ObservableCoordinate : ObservableObject
    {
        private double latitude;
        private double longitude;

        public double Latitude
        {
            get { return this.latitude; }
            set { this.Set(ref this.latitude, value); }
        }

        public double Longitude
        {
            get { return this.longitude; }
            set { this.Set(ref this.longitude, value); }
        }
    }
}
