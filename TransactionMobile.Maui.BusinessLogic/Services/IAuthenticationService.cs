namespace TransactionMobile.Maui.BusinessLogic.Services
{
    public interface IAuthenticationService
    {
        #region Methods

        Task<String> GetToken(String username,
                              String password,
                              CancellationToken cancellationToken);

        #endregion
    }
}