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

        internal MockDataService DataService { get; private set; }
        
        internal MockBookmarkService BookmarkService { get; private set; }

        internal MockIdentityService IdentityService { get; private set; }

        internal MockNavigationService NavigationService { get; private set; }

        public BaseTestContext()
        {
            this.DataService = new MockDataService();
            this.BookmarkService = new MockBookmarkService();
            this.IdentityService = new MockIdentityService();
            this.NavigationService = new MockNavigationService();
            this.savedSynchronizationContext = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        public void Dispose()
        {
            SynchronizationContext.SetSynchronizationContext(this.savedSynchronizationContext);
        }
    }
}