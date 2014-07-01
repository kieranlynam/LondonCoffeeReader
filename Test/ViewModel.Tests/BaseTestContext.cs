using System;
using System.Collections.Generic;
using System.Threading;
using CoffeeClientPrototype.Model;

namespace ViewModel.Tests
{
    public class BaseTestContext : IDisposable
    {
        private readonly SynchronizationContext savedSynchronizationContext;

        public List<Cafe> Cafes { get { return this.DataService.Cafes; } }

        public MockDataService DataService { get; private set; }

        public MockIdentityService IdentityService { get; private set; }

        public BaseTestContext()
        {
            this.DataService = new MockDataService();
            this.IdentityService = new MockIdentityService();
            this.savedSynchronizationContext = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        public void Dispose()
        {
            SynchronizationContext.SetSynchronizationContext(this.savedSynchronizationContext);
        }
    }
}