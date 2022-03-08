namespace TransactionMobile.Maui.BusinessLogic.Services.DummyServices;

using Models;

public class DummyAuthenticationService : IAuthenticationService
{
    #region Methods

    public async Task<TokenResponseModel> GetToken(String username,
                                                   String password,
                                                   CancellationToken cancellationToken)
    {
        return new TokenResponseModel
               {
                   AccessToken = "Token",
                   RefreshToken = "RefreshToken",
                   ExpiryInMinutes = 1
               };
    }

    public async Task<TokenResponseModel> RefreshAccessToken(String refreshToken,
                                                             CancellationToken cancellationToken)
    {
        return new TokenResponseModel
               {
                   AccessToken = "Token",
                   RefreshToken = "RefreshToken",
                   ExpiryInMinutes = 1
               };
    }

    #endregion
}