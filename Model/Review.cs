using System;

namespace CoffeeClientPrototype.Model
{
    public class Review
    {
        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; }

        public Review()
        {
            this.CreatedDate = DateTime.Now;
        }
    }
}
