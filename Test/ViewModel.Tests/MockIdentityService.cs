using System;
using System.Threading.Tasks;
using CoffeeClientPrototype.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModel.Tests
{
    public class MockIdentityService : IIdentityService
    {
        public string Id { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public event EventHandler<AuthenticateEventArgs> AuthenticationRequested; 

        public Task<bool> AuthenticateAsync()
        {
            if (this.AuthenticationRequested == null)
            {
                throw new NotSupportedException("Must provide a AuthenticationRequested event handler.");
            }

            var args = new AuthenticateEventArgs();
            this.AuthenticationRequested(this, args);

            if (!args.IsSuccessful.HasValue)
            {
                throw new NotSupportedException("Must specify Success/Failure in event handler");
            }

            this.IsAuthenticated = args.IsSuccessful.Value;
            this.Id = args.Id;

            return Task.FromResult(this.IsAuthenticated);
        }

        public class AuthenticateEventArgs : EventArgs
        {
            public string Id { get; private set; }

            public bool? IsSuccessful { get; private set; }

            public void Success(string id)
            {
                this.Id = id;
                this.IsSuccessful = true;
            }
            
            public void Fail()
            {
                this.IsSuccessful = false;
            }
        }
        public void SetCurrentIdentity(string identity)
        {
            this.IsAuthenticated = true;
            this.Id = identity;
        }

        public void ClearCurrentIdentity()
        {
            this.IsAuthenticated = false;
            this.Id = null;
        }
    }

    [TestClass]
    public class MockIdentityServiceTests
    {
        [TestMethod]
        public void NotAuthenticatedByDefault()
        {
            var service = new MockIdentityService();

            Assert.IsFalse(service.IsAuthenticated);
            Assert.IsNull(service.Id);
        }

        [TestMethod]
        public void SetCurrentIdentityHelper()
        {
            var service = new MockIdentityService();

            service.SetCurrentIdentity("Ashley");

            Assert.IsTrue(service.IsAuthenticated);
            Assert.AreEqual("Ashley", service.Id);
        }

        [TestMethod]
        public void ClearCurrentIdentityHelper()
        {
            var service = new MockIdentityService();
            service.SetCurrentIdentity("Ashley");

            service.ClearCurrentIdentity();

            Assert.IsFalse(service.IsAuthenticated);
            Assert.IsNull(service.Id);
        }

        [TestMethod]
        public async Task SuccessfulAuthentication()
        {
            var service = new MockIdentityService();

            service.AuthenticationRequested +=
                (sender, args) => args.Success("Johnny");

            var result = await service.AuthenticateAsync();

            Assert.IsTrue(result);
            Assert.IsTrue(service.IsAuthenticated);
            Assert.AreEqual("Johnny", service.Id);
        }

        [TestMethod]
        public async Task FailedAuthentication()
        {
            var service = new MockIdentityService();

            service.AuthenticationRequested +=
                (sender, args) => args.Fail();

            var result = await service.AuthenticateAsync();

            Assert.IsFalse(result);
            Assert.IsFalse(service.IsAuthenticated);
            Assert.IsNull(service.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AuthenticationEventHandlerRequired()
        {
            var service = new MockIdentityService();

            service.AuthenticateAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public async Task AuthenticationEventArgsProvideResult()
        {
            var service = new MockIdentityService();

            service.AuthenticationRequested +=
                (sender, args) => { };

            await service.AuthenticateAsync();
        }
    }
}
