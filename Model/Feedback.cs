using System;

namespace CoffeeClientPrototype.Model
{
    public class Feedback
    {
        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; }

        public Feedback()
        {
            this.CreatedDate = DateTime.Now;
        }
    }
}
