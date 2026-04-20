using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using MvvmHelpers.Commands;
using SimpleResults;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels
{
    public partial class LoginPageViewModel : ExtendedBaseViewModel
    {
        private readonly IApplicationInfoService ApplicationInfoService;
        private readonly IApplicationUpdateLauncherService ApplicationUpdateLauncherService;
        private readonly IBalanceRefresher BalanceRefresher;
        private readonly IUpdateService UpdateService;
        private readonly ISentryService SentryService;

        private String userName;

        private String password;

        private String configHostUrl;

        private Boolean useTrainingMode;

        #region Constructors

        public LoginPageViewModel(IMediator mediator, INavigationService navigationService, IApplicationCache applicationCache,
                                  IDeviceService deviceService,IApplicationInfoService applicationInfoService,
                                  IDialogService dialogService,
                                  INavigationParameterService navigationParameterService,
                                  IUpdateService updateService,
                                  IApplicationUpdateLauncherService applicationUpdateLauncherService,
                                  IBalanceRefresher balanceRefresher,
                                  ISentryService sentryService) : base(applicationCache,dialogService,navigationService, deviceService, navigationParameterService)
        {
            this.ApplicationInfoService = applicationInfoService;
            this.ApplicationUpdateLauncherService = applicationUpdateLauncherService;
            this.BalanceRefresher = balanceRefresher;
            this.Mediator = mediator;
            this.UpdateService = updateService;
            this.SentryService = sentryService;
        }
        
        #endregion

        #region Properties
        
        public IMediator Mediator { get; }

        public String UserName
        {
            get => this.userName;
            set => this.SetProperty(ref this.userName, value);
        }

        public String Password
        {
            get => this.password;
            set => this.SetProperty(ref this.password, value);
        }

        public Boolean UseTrainingMode
        {
            get => this.useTrainingMode;
            set => this.SetProperty(ref this.useTrainingMode, value);
        }

        public String DeviceIdentifier => this.DeviceService.GetIdentifier();

        public String ConfigHostUrl
        {
            get => this.configHostUrl;
            set => this.SetProperty(ref this.configHostUrl, value);
        }

        #endregion

        #region Methods
        private void CacheUseTrainingMode() => this.ApplicationCache.SetUseTrainingMode(this.useTrainingMode);

        private async Task<Result<Configuration>> GetConfiguration() {
            if (!String.IsNullOrEmpty(this.ConfigHostUrl)) {
                this.ApplicationCache.SetConfigHostUrl(this.ConfigHostUrl);
            }

            LogonQueries.GetConfigurationQuery getConfigurationRequest = new (this.DeviceService.GetIdentifier());
            Result<Configuration> configurationResult = await this.Mediator.Send(getConfigurationRequest);

            if (configurationResult.IsSuccess) {
                // Initialise Sentry with the DSN from the configuration service
                this.SentryService.InitializeSentry(configurationResult.Data.SentryDsn);
            }

            return configurationResult;
        }

        private async Task<Result<TokenResponseModel>> GetUserToken() {
            LogonCommands.GetTokenCommand command = new(this.UserName, this.Password);
            Result<TokenResponseModel> tokenResult = await this.Mediator.Send(command);

            if (tokenResult.IsSuccess) {
                // Cache the token
                this.CacheAccessToken(tokenResult.Data);
            }

            return tokenResult;
        }

        private async Task<Result<PerformLogonResponseModel>> PerformLogonTransaction() {
            // Logon Transaction
            TransactionCommands.PerformLogonCommand command = new(DateTime.Now);
            return await this.Mediator.Send(command);
        }

        private async Task<Result<List<ContractProductModel>>> GetMerchantContractProducts() {
            // Get Contracts
            MerchantQueries.GetContractProductsQuery getContractProductsRequest = new MerchantQueries.GetContractProductsQuery();
            return await this.Mediator.Send(getContractProductsRequest);
        }
        
        private async Task<bool> CheckForUpdates(Configuration configuration) {
            if (!this.ShouldCheckForUpdates(configuration)) {
                return true;
            }

            Result<ApplicationUpdateCheckResponse> updateCheckResult = await this.UpdateService.CheckForUpdates(this.ApplicationInfoService.VersionString,
                                                                                                                 this.ApplicationInfoService.PackageName,
                                                                                                                 this.DeviceService.GetPlatform(),
                                                                                                                 this.DeviceService.GetIdentifier(),
                                                                                                                 CancellationToken.None);

            if (!this.IsUpdateRequired(updateCheckResult)) {
                return true;
            }

            ApplicationUpdateCheckResponse updateResponse = updateCheckResult.Data;
            String message = this.BuildUpdateMessage(updateResponse);
            Boolean startUpdate = await this.DialogService.ShowDialog("Application Update Required", message, "Install", "Cancel");

            if (!startUpdate) {
                throw new ApplicationException("An application update is required before you can continue.");
            }

            String downloadUri = this.GetRequiredDownloadUri(updateResponse);

            await this.LaunchRequiredUpdate(downloadUri);
            return false;
        }

        /// <summary>
        /// Determines whether automatic update checks should run for the current configuration.
        /// </summary>
        private Boolean ShouldCheckForUpdates(Configuration configuration) => configuration?.EnableAutoUpdates is true;

        /// <summary>
        /// Evaluates the update check result and returns whether a mandatory update is required.
        /// Logs a warning when the update check fails.
        /// </summary>
        private Boolean IsUpdateRequired(Result<ApplicationUpdateCheckResponse> updateCheckResult)
        {
            if (updateCheckResult.IsFailed) {
                Logger.LogWarning($"Application update check failed: {updateCheckResult.Message}");
                return false;
            }

            return updateCheckResult.Data.UpdateRequired;
        }

        /// <summary>
        /// Builds the user-facing mandatory update message, preferring the server-provided value.
        /// </summary>
        private String BuildUpdateMessage(ApplicationUpdateCheckResponse updateResponse) =>
            String.IsNullOrWhiteSpace(updateResponse.Message)
                ? $"Version {updateResponse.LatestVersion ?? "latest"} is available and must be installed before you can continue. The installer will open and the app will close."
                : updateResponse.Message;

        /// <summary>
        /// Returns the configured download URI for a mandatory update or throws when none is available.
        /// </summary>
        private String GetRequiredDownloadUri(ApplicationUpdateCheckResponse updateResponse)
        {
            if (String.IsNullOrWhiteSpace(updateResponse.DownloadUri)) {
                throw new ApplicationException("An application update is required, but no download location is configured.");
            }

            return updateResponse.DownloadUri;
        }

        /// <summary>
        /// Starts the mandatory update flow, notifies the user, and closes the app after launching the installer.
        /// </summary>
        private async Task LaunchRequiredUpdate(String downloadUri)
        {
            this.IsBusy = true;

            try
            {
                await this.DialogService.ShowInformationToast("Downloading the required update...");
                await this.ApplicationUpdateLauncherService.LaunchUpdateAsync(downloadUri, CancellationToken.None);
                await this.NavigationService.QuitApplication();
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Logon(){
            CorrelationIdProvider.NewId();

            this.CacheUseTrainingMode();

            Stopwatch sw = Stopwatch.StartNew();
            this.WriteTimingTrace(sw, "Start of Login");
            try {
                Logger.LogInformation("Logon called");
                
                Result<Configuration> configurationResult = await this.GetConfiguration();
                this.HandleResult(configurationResult);
                if (!await this.CheckForUpdates(configurationResult.Data)) {
                    return;
                }

                await this.WriteTimingTrace(sw, "After GetConfiguration");
                Result<TokenResponseModel> getTokenResult = await this.GetUserToken();
                this.HandleResult(getTokenResult);

                await this.WriteTimingTrace(sw, "After GetUserToken");
                Result<PerformLogonResponseModel> logonResult = await this.PerformLogonTransaction();
                this.HandleResult(logonResult);

                await this.WriteTimingTrace(sw, "After PerformLogonTransaction");
                Result<List<ContractProductModel>> getMerchantContractProductsResult = await this.GetMerchantContractProducts();
                this.HandleResult(getMerchantContractProductsResult);

                //await this.WriteTimingTrace(sw, "After GetMerchantContractProducts");
                //Result<Decimal> getMerchantBalanceResult =  await this.GetMerchantBalance();
                //this.HandleResult(getMerchantBalanceResult);

                //await this.WriteTimingTrace(sw, "After GetMerchantBalance");
                this.ApplicationCache.SetIsLoggedIn(true);
                
                this.BalanceRefresher.StartRefreshing();
                
                await this.WriteTimingTrace(sw, "After SetIsLoggedIn");
                await this.NavigationService.GoToHome();
            }
            catch(ApplicationException aex) {
                Logger.LogError("Error during logon", aex);
                await this.DialogService.ShowWarningToast(aex.Message);
            }
        }
        
        private async Task WriteTimingTrace(Stopwatch sw, String message) {
            sw.Stop();
            Logger.LogWarning($"{message} - Elapsed ms [{sw.ElapsedMilliseconds}]");
            sw.Start();
        }

        [ExcludeFromCodeCoverage]
        private async void AccessTokenExpired(Object key,
                                        Object value,
                                        EvictionReason reason,
                                        Object state)
        {
            if (reason == EvictionReason.Expired || reason == EvictionReason.TokenExpired)
            {
                // access token has expired need to make another call to get new one
                TokenResponseModel token = value as TokenResponseModel;

                LogonCommands.RefreshTokenCommand command = new (token.RefreshToken);
                Result<TokenResponseModel> newTokenResult = await this.Mediator.Send(command, CancellationToken.None);

                if (newTokenResult.IsSuccess) {
                    this.CacheAccessToken(newTokenResult.Data);
                }
            }
        }

        private void CacheAccessToken(TokenResponseModel token)
        {
            DateTime expirationTime = DateTime.Now.AddMinutes(token.ExpiryInMinutes).AddSeconds(-30);
            CancellationChangeToken expirationToken = new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMinutes(token.ExpiryInMinutes).Add(TimeSpan.FromSeconds(-30))).Token);
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                                                        // Pin to cache.
                                                        .SetPriority(CacheItemPriority.NeverRemove)
                                                        // Set the actual expiration time
                                                        .SetAbsoluteExpiration(expirationTime)
                                                        // Force eviction to run
                                                        .AddExpirationToken(expirationToken)
                                                        // Add eviction callback
                                                        .RegisterPostEvictionCallback(callback:this.AccessTokenExpired);

            this.ApplicationCache.SetAccessToken(token, cacheEntryOptions);
        }
        
        #endregion
    }
}
