using System;

namespace CoffeeClientPrototype.Model
{
    public class Review
    {
        public string Comment { get; set; }

        public DateTime SubmittedDate { get; set; }

        public Review()
        {
            this.SubmittedDate = DateTime.Now;
        }
    }
}
