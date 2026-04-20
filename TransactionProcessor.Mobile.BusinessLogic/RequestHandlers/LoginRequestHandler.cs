using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using SimpleResults;
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

            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            IAuthenticationService authenticationService = this.AuthenticationServiceResolver(useTrainingMode);

            Result<TokenResponseModel> tokenResult = await authenticationService.GetToken(request.UserName, request.Password, cancellationToken);

            return tokenResult;
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
                DateTime expirationTime = DateTime.Now.AddMinutes(60);
                CancellationChangeToken expirationToken = new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMinutes(60)).Token);
                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.NeverRemove)
                    .SetAbsoluteExpiration(expirationTime)
                    .AddExpirationToken(expirationToken);
                this.ApplicationCache.SetConfiguration(configurationResult.Data, cacheEntryOptions);
            }

            return configurationResult;
        }

        #endregion

        public async Task<Result<TokenResponseModel>> Handle(LogonCommands.RefreshTokenCommand request,
                                                             CancellationToken cancellationToken)
        {
            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            IAuthenticationService authenticationService = this.AuthenticationServiceResolver(useTrainingMode);

            Result<TokenResponseModel> tokenResult = await authenticationService.RefreshAccessToken(request.RefreshToken, cancellationToken);

            return tokenResult;
        }
    }
}