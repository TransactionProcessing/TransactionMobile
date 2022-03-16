using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Microsoft.Extensions.Caching.Memory;
    using Models;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ISecurityServiceClient SecurityServiceClient;

        private readonly IMemoryCache CacheProvider;

        public AuthenticationService(ISecurityServiceClient securityServiceClient, IMemoryCache cacheProvider)
        {
            this.SecurityServiceClient = securityServiceClient;
            this.CacheProvider = cacheProvider;
        }

        public async Task<TokenResponseModel> GetToken(String username,
                                                       String password,
                                                       CancellationToken cancellationToken)
        {
            try
            {
                Configuration configuration = this.CacheProvider.Get<Configuration>("Configuration");

                username = "merchantuser@v28emulatormerchant.co.uk";
                password = "123456";

                TokenResponse token =
                    await this.SecurityServiceClient.GetToken(username, password, configuration.ClientId, configuration.ClientSecret, cancellationToken);

                return new TokenResponseModel
                       {
                           AccessToken = token.AccessToken,
                           ExpiryInMinutes = token.ExpiresIn,
                           RefreshToken = token.RefreshToken
                       };
            }
            catch(Exception ex)
            {
                return null;
            }

            
        }

        public async Task<TokenResponseModel> RefreshAccessToken(String refreshToken,
                                                                 CancellationToken cancellationToken)
        {
            var configuration = this.CacheProvider.Get<Configuration>("Configuration");
            TokenResponse token = await this.SecurityServiceClient.GetToken(configuration.ClientId,configuration.ClientSecret, refreshToken,cancellationToken);

            return new TokenResponseModel
                   {
                       AccessToken = token.AccessToken,
                       ExpiryInMinutes = token.ExpiresIn,
                       RefreshToken = token.RefreshToken
                   };
        }
    }
}
