using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Microsoft.Extensions.Caching.Memory;
    using Models;
    using Newtonsoft.Json;
    using RequestHandlers;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ISecurityServiceClient SecurityServiceClient;

        private readonly IApplicationCache ApplicationCache;

        public AuthenticationService(ISecurityServiceClient securityServiceClient, IApplicationCache applicationCache)
        {
            this.SecurityServiceClient = securityServiceClient;
            this.ApplicationCache = applicationCache;
        }

        public async Task<Result<TokenResponseModel>> GetToken(String username,
                                                               String password,
                                                               CancellationToken cancellationToken)
        {
            try
            {
                Configuration configuration = this.ApplicationCache.GetConfiguration();

                //username = "merchantuser@v28emulatormerchant.co.uk";
                //password = "123456";

                Shared.Logger.Logger.LogInformation($"About to request token for {username}");
                Shared.Logger.Logger.LogDebug($"Token Request details UserName: {username} Password: {password} ClientId: {configuration.ClientId} ClientSecret: {configuration.ClientSecret}");
                
                TokenResponse token =
                    await this.SecurityServiceClient.GetToken(username, password, configuration.ClientId, configuration.ClientSecret, cancellationToken);

                Shared.Logger.Logger.LogInformation($"Token for {username} requested successfully");
                Shared.Logger.Logger.LogDebug($"Token Response: [{JsonConvert.SerializeObject(token)}]");

                return new SuccessResult<TokenResponseModel>(new TokenResponseModel
                       {
                           AccessToken = token.AccessToken,
                           ExpiryInMinutes = token.ExpiresIn,
                           RefreshToken = token.RefreshToken
                       });
            }
            catch(Exception ex)
            {
                Shared.Logger.Logger.LogError(new Exception($"Error getting Token", ex));
                return new ErrorResult<TokenResponseModel>("Error getting Token");
            }

            
        }

        public async Task<Result<TokenResponseModel>> RefreshAccessToken(String refreshToken,
                                                                         CancellationToken cancellationToken)
        {
            Configuration configuration = this.ApplicationCache.GetConfiguration();
            try
            {
                Shared.Logger.Logger.LogInformation($"About to request refresh token");
                Shared.Logger.Logger.LogDebug($"Refresh Token Request details Token: {refreshToken} ClientId: {configuration.ClientId} ClientSecret: {configuration.ClientSecret}");

                TokenResponse token = await this.SecurityServiceClient.GetToken(configuration.ClientId, configuration.ClientSecret, refreshToken, cancellationToken);

                Shared.Logger.Logger.LogInformation($"Refresh Token requested successfully");
                Shared.Logger.Logger.LogDebug($"Token Response: [{JsonConvert.SerializeObject(token)}]");

                return new SuccessResult<TokenResponseModel>(new TokenResponseModel
                                                             {
                                                                 AccessToken = token.AccessToken,
                                                                 ExpiryInMinutes = token.ExpiresIn,
                                                                 RefreshToken = token.RefreshToken
                                                             });
            }
            catch (Exception ex)
            {
                Shared.Logger.Logger.LogError(new Exception($"Error refreshing Token", ex));
                return new ErrorResult<TokenResponseModel>("Error getting Token");
            }
        }
    }
}
