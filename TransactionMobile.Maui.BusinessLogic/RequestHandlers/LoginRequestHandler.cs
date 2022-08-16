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
        private readonly Func<Boolean, IConfigurationService> ConfigurationServiceResolver;

        private readonly IApplicationCache ApplicationCache;

        #region Constructors
        public LoginRequestHandler(Func<Boolean,IAuthenticationService> authenticationServiceResolver,
                                   Func<Boolean, IConfigurationService> configurationServiceResolver,
                                   IApplicationCache applicationCache)
        {
            this.ConfigurationServiceResolver = configurationServiceResolver;
            this.ApplicationCache = applicationCache;
            this.AuthenticationServiceResolver = authenticationServiceResolver;
        }

        #endregion

        #region Properties

        public Func<Boolean, IAuthenticationService> AuthenticationServiceResolver { get; }

        #endregion

        #region Methods

        public async Task<TokenResponseModel> Handle(LoginRequest request,
                                                     CancellationToken cancellationToken) {
            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            IAuthenticationService authenticationService = this.AuthenticationServiceResolver(useTrainingMode);

            TokenResponseModel token = await authenticationService.GetToken(request.UserName, request.Password, cancellationToken);

            return token;
        }
        
        public async Task<Configuration> Handle(GetConfigurationRequest request,
                                                CancellationToken cancellationToken)
        {
            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            IConfigurationService configurationService = this.ConfigurationServiceResolver(useTrainingMode);

            return await configurationService.GetConfiguration(request.DeviceIdentifier, cancellationToken);
        }

        #endregion

        public async Task<TokenResponseModel> Handle(RefreshTokenRequest request,
                                                     CancellationToken cancellationToken)
        {
            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            IAuthenticationService authenticationService = this.AuthenticationServiceResolver(useTrainingMode);

            TokenResponseModel token = await authenticationService.RefreshAccessToken(request.RefreshToken, cancellationToken);

            return token;
        }
    }
}