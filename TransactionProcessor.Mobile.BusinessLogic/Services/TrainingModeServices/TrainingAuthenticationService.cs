using System.Diagnostics.CodeAnalysis;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Services.TrainingModeServices
{
    [ExcludeFromCodeCoverage]
    public class TrainingAuthenticationService : IAuthenticationService
    {
        public async Task<Result<TokenResponseModel>> GetToken(String username,
                                                               String password,
                                                               String clientId,
                                                               String clientSecret,
                                                               CancellationToken cancellationToken) {
            return Result.Success(new TokenResponseModel {
                                                             AccessToken = "Token",
                                                             RefreshToken = "RefreshToken",
                                                             ExpiryInMinutes = 1
                                                         });
        }

        public async Task<Result<TokenResponseModel>> RefreshAccessToken(String refreshToken,
                                                                         String clientId,
                                                                         String clientSecret,
                                                                         CancellationToken cancellationToken) {
            return Result.Success(new TokenResponseModel
                                  {
                                      AccessToken = "Token",
                                      RefreshToken = "RefreshToken",
                                      ExpiryInMinutes = 1
                                  });
        }
    }
}
