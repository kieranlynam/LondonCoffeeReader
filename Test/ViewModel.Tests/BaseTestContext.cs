using System;
using System.Collections.Generic;
using CoffeeClientPrototype.Model;

namespace ViewModel.Tests
{
    public class BaseTestContext : IDisposable
    {
        public List<Cafe> Cafes { get { return this.DataService.Cafes; } }

        public MockDataService DataService { get; private set; }

        public MockIdentityService IdentityService { get; private set; }

        public BaseTestContext()
        {
            this.DataService = new MockDataService();
            this.IdentityService = new MockIdentityService();
        }

        public void Dispose()
        {
        }
    }
}