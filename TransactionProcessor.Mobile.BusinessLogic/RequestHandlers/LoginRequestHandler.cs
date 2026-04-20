using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.RequestHandlers
{
    public class LoginRequestHandler : IRequestHandler<LogonCommands.GetTokenCommand, Result<TokenResponseModel>>,
                                       IRequestHandler<LogonCommands.RefreshTokenCommand, Result<TokenResponseModel>>,
                                       IRequestHandler<LogonQueries.GetConfigurationQuery, Result<Configuration>>
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

        public async Task<Result<TokenResponseModel>> Handle(LogonCommands.GetTokenCommand request,
                                                             CancellationToken cancellationToken) {

            Configuration configuration = this.ApplicationCache.GetConfiguration();
            if (configuration == null) {
                return Result.Failure<TokenResponseModel>("App configuration is not available. Please restart the application and try again.");
            }

            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
            IAuthenticationService authenticationService = this.AuthenticationServiceResolver(useTrainingMode);

            return await authenticationService.GetToken(request.UserName, request.Password, configuration.ClientId, configuration.ClientSecret, cancellationToken);
        }

        public async Task<Result<Configuration>> Handle(LogonQueries.GetConfigurationQuery request,
                                                        CancellationToken cancellationToken) {

            Configuration cachedConfiguration = this.ApplicationCache.GetConfiguration();
            if (cachedConfiguration != null) {
                return Result.Success(cachedConfiguration);
            }

            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
            IConfigurationService configurationService = this.ConfigurationServiceResolver(useTrainingMode);

            Result<Configuration> configurationResult = await configurationService.GetConfiguration(request.DeviceIdentifier, cancellationToken);

            if (configurationResult.IsSuccess) {
                // Configuration TTL is set longer than the token lifetime so that the
                // AccessToken eviction callback can always read ClientId/ClientSecret
                // from the config when it fires a token refresh.
                this.ApplicationCache.SetConfiguration(configurationResult.Data, CacheEntryOptionsFactory.WithAbsoluteExpiry(120));
            }

            return configurationResult;
        }

        #endregion

        public async Task<Result<TokenResponseModel>> Handle(LogonCommands.RefreshTokenCommand request,
                                                             CancellationToken cancellationToken)
        {
            Configuration configuration = this.ApplicationCache.GetConfiguration();
            if (configuration == null) {
                return Result.Failure<TokenResponseModel>("App configuration is not available. Token refresh failed.");
            }

            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
            IAuthenticationService authenticationService = this.AuthenticationServiceResolver(useTrainingMode);

            return await authenticationService.RefreshAccessToken(request.RefreshToken, configuration.ClientId, configuration.ClientSecret, cancellationToken);
        }
    }
}