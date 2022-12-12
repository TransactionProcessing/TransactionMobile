namespace TransactionMobile.Maui.BusinessLogic.ViewModels
{
    using System.Diagnostics;
    using System.Windows.Input;
    using Maui.UIServices;
    using MediatR;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Primitives;
    using Models;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using RequestHandlers;
    using Requests;
    using Services;
    using Shared.Logger;
    using TransactionMobile.Maui.Database;
    using UIServices;

    public class LoginPageViewModel : ExtendedBaseViewModel
    {
        private readonly INavigationService NavigationService;

        private readonly IApplicationCache ApplicationCache;

        private readonly IDeviceService DeviceService;

        private readonly IApplicationInfoService ApplicationInfoService;

        private readonly IDialogService DialogService;

        private String userName;

        private String password;

        private Boolean useTrainingMode;

        #region Constructors

        public LoginPageViewModel(IMediator mediator, INavigationService navigationService, IApplicationCache applicationCache,
                                  IDeviceService deviceService,IApplicationInfoService applicationInfoService,
                                  IDialogService dialogService) : base(applicationCache,dialogService,navigationService)
        {
            this.NavigationService = navigationService;
            this.ApplicationCache = applicationCache;
            this.DeviceService = deviceService;
            this.ApplicationInfoService = applicationInfoService;
            this.DialogService = dialogService;
            this.LoginCommand = new AsyncCommand(this.LoginCommandExecute);
            this.Mediator = mediator;
        }

        #endregion

        #region Properties

        public ICommand LoginCommand { get; }

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

        #endregion

        #region Methods

        private async Task<Result<Configuration>> GetConfiguration() {
            String deviceIdentifier = this.DeviceService.GetIdentifier();
            GetConfigurationRequest getConfigurationRequest = GetConfigurationRequest.Create(deviceIdentifier);
            var configurationResult = await this.Mediator.Send(getConfigurationRequest);

            if (configurationResult.Success) {
                // Cache the config object
                this.ApplicationCache.SetConfiguration(configurationResult.Data);
            }

            return configurationResult;
        }

        private async Task<Result<TokenResponseModel>> GetUserToken() {
            LoginRequest loginRequest = LoginRequest.Create(this.UserName, this.Password);
            Result<TokenResponseModel> tokenResult = await this.Mediator.Send(loginRequest);

            if (tokenResult.Success) {
                // Cache the token
                this.CacheAccessToken(tokenResult.Data);
            }

            return tokenResult;
        }

        private async Task<Result<PerformLogonResponseModel>> PerformLogonTransaction() {
            // Logon Transaction
            LogonTransactionRequest logonTransactionRequest = LogonTransactionRequest.Create(DateTime.Now);
            Result<PerformLogonResponseModel> logonResult = await this.Mediator.Send(logonTransactionRequest);

            if (logonResult.Success) {
                // Set the user information
                this.ApplicationCache.SetEstateId(logonResult.Data.EstateId);
                this.ApplicationCache.SetMerchantId(logonResult.Data.MerchantId);
            }

            return logonResult;
        }

        private async Task<Result<List<ContractProductModel>>> GetMerchantContractProducts() {
            // Get Contracts
            GetContractProductsRequest getContractProductsRequest = GetContractProductsRequest.Create();
            Result<List<ContractProductModel>> productsResult = await this.Mediator.Send(getContractProductsRequest);

            if (productsResult.Success) {
                this.CacheContractData(productsResult.Data);
            }

            return productsResult;
        }

        private void CacheContractData(List<ContractProductModel> contractProductModels)
        {
            DateTime expirationTime = DateTime.Now.AddMinutes(60);
            CancellationChangeToken expirationToken = new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMinutes(60)).Token);
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                                                        // Pin to cache.
                                                        .SetPriority(CacheItemPriority.NeverRemove)
                                                        // Set the actual expiration time
                                                        .SetAbsoluteExpiration(expirationTime)
                                                        // Force eviction to run
                                                        .AddExpirationToken(expirationToken);

            this.ApplicationCache.SetContractProducts(contractProductModels, cacheEntryOptions);
        }

        private async Task<Result<Decimal>> GetMerchantBalance() {
            // Get the merchant balance
            // TODO: Cache the result, but will add this to a timer call to keep up to date...
            GetMerchantBalanceRequest getMerchantBalanceRequest = GetMerchantBalanceRequest.Create();
            Result<Decimal> getMerchantBalanceResult = await this.Mediator.Send(getMerchantBalanceRequest);

            return getMerchantBalanceResult;
        }

        private async Task LoginCommandExecute()
        {
            Stopwatch sw = Stopwatch.StartNew();
            WriteTimingTrace(sw, "Start of LoginCommandExecute");
            try {
                Shared.Logger.Logger.LogInformation("LoginCommandExecute called");
                
                Result<Configuration> configurationResult = await this.GetConfiguration();
                this.HandleResult(configurationResult);
                
                WriteTimingTrace(sw, "After GetConfiguration");
                Result<TokenResponseModel> getTokenResult = await this.GetUserToken();
                this.HandleResult(getTokenResult);

                WriteTimingTrace(sw, "After GetUserToken");
                Result<PerformLogonResponseModel> logonResult = await this.PerformLogonTransaction();
                this.HandleResult(logonResult);

                WriteTimingTrace(sw, "After PerformLogonTransaction");
                Result<List<ContractProductModel>> getMerchantContractProductsResult = await this.GetMerchantContractProducts();
                this.HandleResult(getMerchantContractProductsResult);

                WriteTimingTrace(sw, "After GetMerchantContractProducts");
                Result<Decimal> getMerchantBalanceResult =  await this.GetMerchantBalance();
                this.HandleResult(getMerchantBalanceResult);

                WriteTimingTrace(sw, "After GetMerchantBalance");
                this.ApplicationCache.SetIsLoggedIn(true);

                WriteTimingTrace(sw, "After SetIsLoggedIn");
                await this.NavigationService.GoToHome();
            }
            catch(ApplicationException aex) {
                Logger.LogError(aex);
                await this.DialogService.ShowWarningToast(aex.Message);
            }
        }
        
        private void WriteTimingTrace(Stopwatch sw, String message) {
            sw.Stop();
            Shared.Logger.Logger.LogWarning($"{message} - Elapsed ms [{sw.ElapsedMilliseconds}]");
            sw.Start();
        }

        private async void AccessTokenExpired(Object key,
                                        Object value,
                                        EvictionReason reason,
                                        Object state)
        {
            if (reason == EvictionReason.Expired || reason == EvictionReason.TokenExpired)
            {
                // access token has expired need to make another call to get new one
                TokenResponseModel token = value as TokenResponseModel;

                RefreshTokenRequest request = RefreshTokenRequest.Create(token.RefreshToken);
                Result<TokenResponseModel> newTokenResult = await this.Mediator.Send(request, CancellationToken.None);

                if (newTokenResult.Success) {
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