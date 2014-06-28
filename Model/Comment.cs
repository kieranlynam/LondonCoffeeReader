using System;

namespace CoffeeClientPrototype.Model
{
    public class Comment
    {
        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public Comment()
        {
            this.CreatedDate = DateTime.Now;
        }
    }
}
