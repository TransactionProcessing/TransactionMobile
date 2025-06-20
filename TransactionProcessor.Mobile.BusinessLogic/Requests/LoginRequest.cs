using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests
{
    public class LoginRequest : IRequest<Result<TokenResponseModel>>
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