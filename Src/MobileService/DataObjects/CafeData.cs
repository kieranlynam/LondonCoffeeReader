using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace londoncoffeeService.DataObjects
{
    [Table("Cafes")]
    public class CafeData : EntityData
    {
        public string Name { get; set; }

        public double CoffeeRating { get; set; }
        
        public double AtmosphereRating { get; set; }

        public int NumberOfVotes { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string Address { get; set; }

        public string PostCode { get; set; }
    }
}