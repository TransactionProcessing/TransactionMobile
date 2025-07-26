using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using MvvmHelpers.Commands;
using SimpleResults;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
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

        private String userName;

        private String password;

        private String configHostUrl;

        private Boolean useTrainingMode;

        #region Constructors

        public LoginPageViewModel(IMediator mediator, INavigationService navigationService, IApplicationCache applicationCache,
                                  IDeviceService deviceService,IApplicationInfoService applicationInfoService,
                                  IDialogService dialogService,
                                  INavigationParameterService navigationParameterService) : base(applicationCache,dialogService,navigationService, deviceService, navigationParameterService)
        {
            this.ApplicationInfoService = applicationInfoService;
            this.Mediator = mediator;
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
            if (String.IsNullOrEmpty(this.ConfigHostUrl) == false) {
                this.ApplicationCache.SetConfigHostUrl(this.ConfigHostUrl);
            }

            GetConfigurationRequest getConfigurationRequest = GetConfigurationRequest.Create(this.DeviceService.GetIdentifier());
            Result<Configuration> configurationResult = await this.Mediator.Send(getConfigurationRequest);

            if (configurationResult.IsSuccess) {
                // Cache the config object
                this.ApplicationCache.SetConfiguration(configurationResult.Data);
            }

            return configurationResult;
        }

        private async Task<Result<TokenResponseModel>> GetUserToken() {
            LoginRequest loginRequest = LoginRequest.Create(this.UserName, this.Password);
            Result<TokenResponseModel> tokenResult = await this.Mediator.Send(loginRequest);

            if (tokenResult.IsSuccess) {
                // Cache the token
                this.CacheAccessToken(tokenResult.Data);
            }

            return tokenResult;
        }

        private async Task<Result<PerformLogonResponseModel>> PerformLogonTransaction() {
            // Logon Transaction
            LogonTransactionRequest logonTransactionRequest = LogonTransactionRequest.Create(DateTime.Now);
            Result<PerformLogonResponseModel> logonResult = await this.Mediator.Send(logonTransactionRequest);

            if (logonResult.IsSuccess) {
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

            if (productsResult.IsSuccess) {
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

        [RelayCommand]
        private async Task Logon(){
            this.CacheUseTrainingMode();

            Stopwatch sw = Stopwatch.StartNew();
            this.WriteTimingTrace(sw, "Start of Login");
            try {
                Logger.LogInformation("Logon called");
                
                Result<Configuration> configurationResult = await this.GetConfiguration();
                this.HandleResult(configurationResult);

                await this.WriteTimingTrace(sw, "After GetConfiguration");
                Result<TokenResponseModel> getTokenResult = await this.GetUserToken();
                this.HandleResult(getTokenResult);

                await this.WriteTimingTrace(sw, "After GetUserToken");
                Result<PerformLogonResponseModel> logonResult = await this.PerformLogonTransaction();
                this.HandleResult(logonResult);

                await this.WriteTimingTrace(sw, "After PerformLogonTransaction");
                Result<List<ContractProductModel>> getMerchantContractProductsResult = await this.GetMerchantContractProducts();
                this.HandleResult(getMerchantContractProductsResult);

                await this.WriteTimingTrace(sw, "After GetMerchantContractProducts");
                Result<Decimal> getMerchantBalanceResult =  await this.GetMerchantBalance();
                this.HandleResult(getMerchantBalanceResult);

                await this.WriteTimingTrace(sw, "After GetMerchantBalance");
                this.ApplicationCache.SetIsLoggedIn(true);

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

                RefreshTokenRequest request = RefreshTokenRequest.Create(token.RefreshToken);
                Result<TokenResponseModel> newTokenResult = await this.Mediator.Send(request, CancellationToken.None);

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