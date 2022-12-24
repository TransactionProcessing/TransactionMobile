using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Logging;
    using Microsoft.Extensions.Caching.Memory;
    using Models;
    using Newtonsoft.Json;
    using RequestHandlers;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;
    using ViewModels;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILoggerService Logger;

        private readonly ISecurityServiceClient SecurityServiceClient;

        private readonly IApplicationCache ApplicationCache;

        public AuthenticationService(ILoggerService logger, ISecurityServiceClient securityServiceClient, IApplicationCache applicationCache)
        {
            this.Logger = logger;
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

                await Logger.LogInformation($"About to request token for {username}");
                await Logger.LogDebug($"Token Request details UserName: {username} Password: {password} ClientId: {configuration.ClientId} ClientSecret: {configuration.ClientSecret}");
                
                TokenResponse token =
                    await this.SecurityServiceClient.GetToken(username, password, configuration.ClientId, configuration.ClientSecret, cancellationToken);

                await Logger.LogInformation($"Token for {username} requested successfully");
                await Logger.LogDebug($"Token Response: [{JsonConvert.SerializeObject(token)}]");

                return new SuccessResult<TokenResponseModel>(new TokenResponseModel
                       {
                           AccessToken = token.AccessToken,
                           ExpiryInMinutes = token.ExpiresIn,
                           RefreshToken = token.RefreshToken
                       });
            }
            catch(Exception ex)
            {
                await Logger.LogError($"Error getting Token", ex);
                return new ErrorResult<TokenResponseModel>("Error getting Token");
            }

            
        }

        public async Task<Result<TokenResponseModel>> RefreshAccessToken(String refreshToken,
                                                                         CancellationToken cancellationToken)
        {
            Configuration configuration = this.ApplicationCache.GetConfiguration();
            try
            {
                await Logger.LogInformation($"About to request refresh token");
                await Logger.LogDebug($"Refresh Token Request details Token: {refreshToken} ClientId: {configuration.ClientId} ClientSecret: {configuration.ClientSecret}");

                TokenResponse token = await this.SecurityServiceClient.GetToken(configuration.ClientId, configuration.ClientSecret, refreshToken, cancellationToken);

                await Logger.LogInformation($"Refresh Token requested successfully");
                await Logger.LogDebug($"Token Response: [{JsonConvert.SerializeObject(token)}]");

                return new SuccessResult<TokenResponseModel>(new TokenResponseModel
                                                             {
                                                                 AccessToken = token.AccessToken,
                                                                 ExpiryInMinutes = token.ExpiresIn,
                                                                 RefreshToken = token.RefreshToken
                                                             });
            }
            catch (Exception ex)
            {
                await Logger.LogError($"Error refreshing Token", ex);
                return new ErrorResult<TokenResponseModel>("Error getting Token");
            }
        }
    }
}
