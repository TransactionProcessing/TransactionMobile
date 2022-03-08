namespace TransactionMobile.Maui.BusinessLogic.Requests
{
    using MediatR;
    using Models;

    public class LoginRequest : IRequest<TokenResponseModel>
    {
        #region Constructors

        private LoginRequest(String username,
                             String password)
        {
            this.UserName = username;
            this.Password = password;
        }

        #endregion

        #region Properties

        public String Password { get; }

        public String UserName { get; }

        #endregion

        #region Methods

        public static LoginRequest Create(String username,
                                          String password)
        {
            return new LoginRequest(username, password);
        }

        #endregion
    }
}