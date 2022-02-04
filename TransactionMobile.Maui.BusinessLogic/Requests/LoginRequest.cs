namespace TransactionMobile.Maui.BusinessLogic.Requests
{
    using MediatR;

    public class LoginRequest : IRequest<String>
    {
        #region Properties

        public String Password { get; set; }

        public String UserName { get; set; }

        #endregion
    }
}