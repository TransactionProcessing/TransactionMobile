namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Models;

    public interface IAuthenticationService
    {
        #region Methods

        Task<TokenResponseModel> GetToken(String username,
                                           String password,
                                           CancellationToken cancellationToken);

        Task<TokenResponseModel> RefreshAccessToken(String refreshToken,
                                                    CancellationToken cancellationToken);

        #endregion
    }
}