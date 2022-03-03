namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers
{
    using MediatR;
    using Models;
    using Requests;
    using Services;

    public class LoginRequestHandler : IRequestHandler<LoginRequest, String>,
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

        public async Task<String> Handle(LoginRequest request,
                                         CancellationToken cancellationToken)
        {
            var token = await this.AuthenticationService.GetToken(request.UserName, request.Password, cancellationToken);

            return token;
        }
        
        public async Task<Configuration> Handle(GetConfigurationRequest request,
                                                CancellationToken cancellationToken)
        {
            return await this.ConfigurationService.GetConfiguration(request.DeviceIdentifier, cancellationToken);
        }

        #endregion
    }
}