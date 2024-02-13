﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services.TrainingModeServices
{
    using Models;
    using RequestHandlers;
    using TransactionProcessorACL.DataTransferObjects.Responses;

    public class TrainingAuthenticationService : IAuthenticationService
    {
        public async Task<Result<TokenResponseModel>> GetToken(String username,
                                                               String password,
                                                               CancellationToken cancellationToken) {
            return new SuccessResult<TokenResponseModel>(new TokenResponseModel {
                                                                                    AccessToken = "Token",
                                                                                    RefreshToken = "RefreshToken",
                                                                                    ExpiryInMinutes = 1
                                                                                });
        }

        public async Task<Result<TokenResponseModel>> RefreshAccessToken(String refreshToken,
                                                                         CancellationToken cancellationToken) {
            return new SuccessResult<TokenResponseModel>(new TokenResponseModel
                                                         {
                                                             AccessToken = "Token",
                                                             RefreshToken = "RefreshToken",
                                                             ExpiryInMinutes = 1
                                                         });
        }
    }
}
