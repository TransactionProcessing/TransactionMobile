using Newtonsoft.Json;
using SecurityService.Client;
using SecurityService.DataTransferObjects.Responses;
using Shared.Results;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Services
{
    public interface IAuthenticationService
    {
        #region Methods

        Task<Result<TokenResponseModel>> GetToken(String username,
                                                  String password,
                                                  String clientId,
                                                  String clientSecret,
                                                  CancellationToken cancellationToken);

        Task<Result<TokenResponseModel>> RefreshAccessToken(String refreshToken,
                                                            String clientId,
                                                            String clientSecret,
                                                            CancellationToken cancellationToken);

        #endregion
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ISecurityServiceClient SecurityServiceClient;

        public AuthenticationService(ISecurityServiceClient securityServiceClient)
        {
            this.SecurityServiceClient = securityServiceClient;
        }

        public async Task<Result<TokenResponseModel>> GetToken(String username,
                                                               String password,
                                                               String clientId,
                                                               String clientSecret,
                                                               CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation($"About to request token for {username}");
                Logger.LogDebug($"Token Request details UserName: {username} Password: ****** ClientId: {clientId} ClientSecret: {clientSecret}");
                
                Result<TokenResponse> tokenResult =
                    await this.SecurityServiceClient.GetToken(username, password, clientId, clientSecret, cancellationToken);

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
                                                                         String clientId,
                                                                         String clientSecret,
                                                                         CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation($"About to request refresh token");
                Logger.LogDebug($"Refresh Token Request details Token: {refreshToken} ClientId: {clientId} ClientSecret: {clientSecret}");

                Result<TokenResponse> tokenResult = await this.SecurityServiceClient.GetToken(clientId, clientSecret, refreshToken, cancellationToken);

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