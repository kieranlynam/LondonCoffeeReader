using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using Microsoft.WindowsAzure.MobileServices;

namespace CoffeeClientPrototype.Services
{
    public class AzureIdentityService : IIdentityService
    {
        private readonly MobileServiceClient serviceClient;
        private readonly PasswordVault vault;

        public string Id { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public AzureIdentityService(MobileServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
            this.vault = new PasswordVault();

            PasswordCredential credential;
            if (TryGetCachedCredential(out credential))
            {
                this.Id = credential.UserName;
            }
        }

        public async Task<bool> AuthenticateAsync()
        {
            this.Id = null;
            this.IsAuthenticated = false;

            PasswordCredential credential;

            if (TryGetCachedCredential(out credential))
            {
                var user = new MobileServiceUser(credential.UserName);
                credential.RetrievePassword();
                user.MobileServiceAuthenticationToken = credential.Password;

                this.serviceClient.CurrentUser = user;

                if (await HasTokenExpired())
                {
                    vault.Remove(credential);
                }
                else
                {
                    this.Id = user.UserId;
                    this.IsAuthenticated = true;
                }
            }

            if (!this.IsAuthenticated)
            {
                try
                {
                    var user = await this.serviceClient
                        .LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);

                    credential = new PasswordCredential(
                        MobileServiceAuthenticationProvider.MicrosoftAccount.ToString(),
                        user.UserId,
                        user.MobileServiceAuthenticationToken);
                    vault.Add(credential);

                    this.Id = user.UserId;
                    this.IsAuthenticated = true;
                }
                catch (MobileServiceInvalidOperationException)
                {
                }
            }

            return this.IsAuthenticated;
        }

        private bool TryGetCachedCredential(out PasswordCredential credential)
        {
            try
            {
                credential = vault
                    .FindAllByResource(MobileServiceAuthenticationProvider.MicrosoftAccount.ToString())
                    .FirstOrDefault();
                return true;
            }
            catch
            {
                credential = null;
                return false;
            }
        }

        private async Task<bool> HasTokenExpired()
        {
            try
            {
                // Try to return an item now to determine if the cached credential has expired.
                await this.serviceClient
                    .GetTable<Cafe>()
                    .Take(1)
                    .ToListAsync();
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
