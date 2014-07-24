using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace londoncoffeeService.DataObjects
{
    [Table("Reviews")]
    public class ReviewData : EntityData
    {
        public string Comment { get; set; }

        public double? CoffeeRating { get; set; }

        public double? AtmosphereRating { get; set; }

        public string SubmittedBy { get; set; }

        public DateTime SubmittedDate { get; set; }
    }
}