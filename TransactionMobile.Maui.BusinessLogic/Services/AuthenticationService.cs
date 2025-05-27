using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Results;

namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Common;
    using Logging;
    using Microsoft.Extensions.Caching.Memory;
    using Models;
    using Newtonsoft.Json;
    using RequestHandlers;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;
    using SimpleResults;
    using ViewModels;

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

                Logger.LogInformation($"About to request token for {username}");
                Logger.LogDebug($"Token Request details UserName: {username} Password: {password} ClientId: {configuration.ClientId} ClientSecret: {configuration.ClientSecret}");
                
                Result<TokenResponse> tokenResult =
                    await this.SecurityServiceClient.GetToken(username, password, configuration.ClientId, configuration.ClientSecret, cancellationToken);

                if (tokenResult.IsFailed)
                {
                    return ResultHelpers.CreateFailure(tokenResult);
                }

                Logger.LogInformation($"Token for {username} requested successfully");
                Logger.LogDebug($"Token Response: [{JsonConvert.SerializeObject(tokenResult.Data.AccessToken)}]");

                return Result.Success(new TokenResponseModel
                       {
                           AccessToken = tokenResult.Data.AccessToken,
                           ExpiryInMinutes = tokenResult.Data.ExpiresIn,
                           RefreshToken = tokenResult.Data.RefreshToken
                       });
            }
            catch(Exception ex)
            {
                Logger.LogError($"Error getting Token", ex);
                return ResultExtensions.FailureExtended("Error getting Token", ex);
            }

            
        }

        public async Task<Result<TokenResponseModel>> RefreshAccessToken(String refreshToken,
                                                                         CancellationToken cancellationToken)
        {
            Configuration configuration = this.ApplicationCache.GetConfiguration();
            try
            {
                Logger.LogInformation($"About to request refresh token");
                Logger.LogDebug($"Refresh Token Request details Token: {refreshToken} ClientId: {configuration.ClientId} ClientSecret: {configuration.ClientSecret}");

                Result<TokenResponse> tokenResult = await this.SecurityServiceClient.GetToken(configuration.ClientId, configuration.ClientSecret, refreshToken, cancellationToken);

                if (tokenResult.IsFailed)
                {
                    return ResultHelpers.CreateFailure(tokenResult);
                }

                Logger.LogInformation($"Refresh Token requested successfully");
                Logger.LogDebug($"Token Response: [{JsonConvert.SerializeObject(tokenResult.Data.AccessToken)}]");

                return Result.Success(new TokenResponseModel
                                                             {
                                                                 AccessToken = tokenResult.Data.AccessToken,
                                                                 ExpiryInMinutes = tokenResult.Data.ExpiresIn,
                                                                 RefreshToken = tokenResult.Data.RefreshToken
                                                             });
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error refreshing Token", ex);
                return ResultExtensions.FailureExtended("Error refreshing Token", ex);
            }
        }
    }
}
