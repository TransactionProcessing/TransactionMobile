using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services.TrainingModeServices
{
    using Common;
    using Models;
    using RequestHandlers;
    using SimpleResults;
    using TransactionProcessorACL.DataTransferObjects.Responses;

    [ExcludeFromCodeCoverage]
    public class TrainingAuthenticationService : IAuthenticationService
    {
        public async Task<Result<TokenResponseModel>> GetToken(String username,
                                                               String password,
                                                               CancellationToken cancellationToken) {
            return Result.Success(new TokenResponseModel {
                                                             AccessToken = "Token",
                                                             RefreshToken = "RefreshToken",
                                                             ExpiryInMinutes = 1
                                                         });
        }

        public async Task<Result<TokenResponseModel>> RefreshAccessToken(String refreshToken,
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
