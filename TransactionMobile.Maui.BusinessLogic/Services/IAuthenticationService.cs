namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Common;
    using Models;
    using RequestHandlers;

    public interface IAuthenticationService
    {
        #region Methods

        Task<Result<TokenResponseModel>> GetToken(String username,
                                                  String password,
                                                  CancellationToken cancellationToken);

        Task<Result<TokenResponseModel>> RefreshAccessToken(String refreshToken,
                                                            CancellationToken cancellationToken);

        #endregion
    }
}