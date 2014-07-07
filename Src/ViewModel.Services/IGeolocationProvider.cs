using System.Threading;
using System.Threading.Tasks;

namespace CoffeeClientPrototype.ViewModel.Services
{
    public interface IGeolocationProvider
    {
        Task<Coordinate> GetLocationAsync(CancellationToken cancellationToken);
    }

    public sealed class Coordinate
    {
        public readonly static Coordinate Origin = new Coordinate(0, 0);

        public double Latitude { get; private set; }
        
        public double Longitude {  get; private set; }

        public Coordinate(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            var other = (Coordinate) obj;
            return this.Latitude.Equals(other.Latitude) && this.Longitude.Equals(other.Longitude);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Latitude.GetHashCode()*397) ^ this.Longitude.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("Latitude={0}; Longitude={1}", this.Latitude, this.Longitude);
        }
    }
}