using System;
using System.Collections.Generic;
using CoffeeClientPrototype.Model;

namespace ViewModel.Tests
{
    public class BaseTestContext : IDisposable
    {
        public List<Cafe> Cafes { get; private set; }

        protected MockDataService DataService { get; private set; }

        public BaseTestContext()
        {
            this.Cafes = new List<Cafe>();
            DataService = new MockDataService(() => this.Cafes);
        }

        public void Dispose()
        {
        }
    }
}