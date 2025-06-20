using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.RequestHandlers
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, Result<TokenResponseModel>>,
                                       IRequestHandler<RefreshTokenRequest, Result<TokenResponseModel>>,
                                       IRequestHandler<GetConfigurationRequest, Result<Configuration>>
    {
        private readonly Func<Boolean, IConfigurationService> ConfigurationServiceResolver;
        private readonly IApplicationCache ApplicationCache;
        public Func<Boolean, IAuthenticationService> AuthenticationServiceResolver { get; }

        #region Constructors
        public LoginRequestHandler(Func<Boolean, IAuthenticationService> authenticationServiceResolver,
                                   Func<Boolean, IConfigurationService> configurationServiceResolver,
                                   IApplicationCache applicationCache)
        {
            this.ConfigurationServiceResolver = configurationServiceResolver;
            this.AuthenticationServiceResolver = authenticationServiceResolver;
            this.ApplicationCache = applicationCache;
        }

        #endregion
        

        #region Methods

        public async Task<Result<TokenResponseModel>> Handle(LoginRequest request,
                                                             CancellationToken cancellationToken) {

            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            IAuthenticationService authenticationService = this.AuthenticationServiceResolver(useTrainingMode);

            Result<TokenResponseModel> tokenResult = await authenticationService.GetToken(request.UserName, request.Password, cancellationToken);

            return tokenResult;
        }

        public async Task<Result<Configuration>> Handle(GetConfigurationRequest request,
                                                        CancellationToken cancellationToken) {

            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            IConfigurationService configurationService = this.ConfigurationServiceResolver(useTrainingMode);

            return await configurationService.GetConfiguration(request.DeviceIdentifier, cancellationToken);
        }

        #endregion

        public async Task<Result<TokenResponseModel>> Handle(RefreshTokenRequest request,
                                                             CancellationToken cancellationToken)
        {
            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            IAuthenticationService authenticationService = this.AuthenticationServiceResolver(useTrainingMode);

            Result<TokenResponseModel> tokenResult = await authenticationService.RefreshAccessToken(request.RefreshToken, cancellationToken);

            return tokenResult;
        }
    }
}