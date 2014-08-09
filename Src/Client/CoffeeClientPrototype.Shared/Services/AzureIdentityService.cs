using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoffeeClientPrototype.ViewModel.Services;
using Microsoft.WindowsAzure.MobileServices;

namespace CoffeeClientPrototype.Services
{
    public class AzureIdentityService : IIdentityService
    {
        private readonly MobileServiceClient serviceClient;

        public string Id { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                var user = await this.serviceClient
                                        .LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);

                this.Id = user.UserId;
                this.IsAuthenticated = true;
            }
            catch (InvalidOperationException)
            {
                this.Id = null;
                this.IsAuthenticated = false;
            }

            return this.IsAuthenticated;
        }

        public AzureIdentityService(MobileServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
        }
    }
}
