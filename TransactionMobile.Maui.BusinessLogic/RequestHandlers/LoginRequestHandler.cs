namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers
{
    using MediatR;
    using Models;
    using Requests;
    using Services;

    public class LoginRequestHandler : IRequestHandler<LoginRequest, TokenResponseModel>,
                                       IRequestHandler<RefreshTokenRequest, TokenResponseModel>,
                                       IRequestHandler<GetConfigurationRequest, Configuration>
    {
        private readonly IConfigurationService ConfigurationService;

        #region Constructors
        public LoginRequestHandler(IAuthenticationService authenticationService,
                                   IConfigurationService configurationService)
        {
            this.ConfigurationService = configurationService;

            this.AuthenticationService = authenticationService;
        }

        #endregion

        #region Properties

        public IAuthenticationService AuthenticationService { get; }

        #endregion

        #region Methods

        public async Task<TokenResponseModel> Handle(LoginRequest request,
                                                     CancellationToken cancellationToken)
        {
            TokenResponseModel token = await this.AuthenticationService.GetToken(request.UserName, request.Password, cancellationToken);

            return token;
        }
        
        public async Task<Configuration> Handle(GetConfigurationRequest request,
                                                CancellationToken cancellationToken)
        {
            return await this.ConfigurationService.GetConfiguration(request.DeviceIdentifier, cancellationToken);
        }

        #endregion

        public async Task<TokenResponseModel> Handle(RefreshTokenRequest request,
                                                     CancellationToken cancellationToken)
        {
            TokenResponseModel token = await this.AuthenticationService.RefreshAccessToken(request.RefreshToken, cancellationToken);

            return token;
        }
    }
}