using System.Collections.Generic;
using System.Linq;
using CoffeeClientPrototype.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModel.Tests
{
    internal class MockNavigationService : INavigationService
    {
        private readonly Stack<NavigationEntry> navigationStack;

        public NavigationEntry Current
        {
            get { return this.navigationStack.Any() ? this.navigationStack.Peek() : null; }
        }

        public IEnumerable<NavigationEntry> History
        {
            get { return this.navigationStack; }
        }

        public MockNavigationService()
        {
            this.navigationStack = new Stack<NavigationEntry>();
        }

        public void Navigate(string destination, IDictionary<string, object> parameters = null)
        {
            this.navigationStack.Push(new NavigationEntry(destination, parameters));
        }
    }

    internal class NavigationEntry
    {
        private static readonly IDictionary<string, object> EmptyParameters = new Dictionary<string, object>();

        public string Location { get; private set; }

        public IDictionary<string, object> Parameters { get; private set; }

        public NavigationEntry(string location, IDictionary<string, object> parameters)
        {
            this.Location = location;
            this.Parameters = parameters ?? EmptyParameters;
        }
    }

    [TestClass]
    public class MockNavigationServiceTests
    {
        [TestMethod]
        public void CurrentIsNullAtFirst()
        {
            var service = new MockNavigationService();
            Assert.IsNull(service.Current);
        }

        [TestMethod]
        public void HistoryIsEmptyAtFirst()
        {
            var service = new MockNavigationService();
            Assert.IsFalse(service.History.Any());
        }

        [TestMethod]
        public void CurrentSetAfterNavigation()
        {
            var service = new MockNavigationService();

            service.Navigate("ONE");
            Assert.AreEqual("ONE", service.Current.Location);

            service.Navigate("TWO");
            Assert.AreEqual("TWO", service.Current.Location);

            service.Navigate("THREE", new Dictionary<string, object> { { "Param", "Value" }});
            Assert.AreEqual("Value", service.Current.Parameters["Param"]);
        
            service.Navigate("FOUR");
            Assert.IsFalse(service.Current.Parameters.Any());
        }

        [TestMethod]
        public void HistoryExtendedAfterNavigation()
        {
            var service = new MockNavigationService();

            service.Navigate("ONE");
            Assert.AreEqual("ONE", service.History.First().Location);
            Assert.AreEqual(1, service.History.Count());

            service.Navigate("TWO");
            Assert.AreEqual("TWO", service.History.First().Location);
            Assert.AreEqual(2, service.History.Count());

            service.Navigate("THREE", new Dictionary<string, object> { { "Param", "Value" } });
            Assert.AreEqual("Value", service.History.First().Parameters["Param"]);
            Assert.AreEqual(3, service.History.Count());
        }
    }
}