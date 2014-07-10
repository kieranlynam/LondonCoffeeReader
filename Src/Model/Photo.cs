using System;

namespace CoffeeClientPrototype.Model
{
    public class Photo
    {
        public Uri ImageUri { get; set; }

        public int NumberOfVotes { get; set; }

        public string SubmittedBy { get; set; }

        public DateTime SubmittedDate { get; set; }
    }
}
