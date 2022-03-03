namespace TransactionMobile.Maui.BusinessLogic.Services.DummyServices;

public class DummyAuthenticationService : IAuthenticationService
{
    #region Methods

    public async Task<String> GetToken(String username,
                                       String password,
                                       CancellationToken cancellationToken)
    {
        return "MyToken";
    }

    #endregion
}