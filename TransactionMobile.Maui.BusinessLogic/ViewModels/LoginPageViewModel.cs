namespace TransactionMobile.Maui.BusinessLogic.ViewModels
{
    using System.Windows.Input;
    using Maui.UIServices;
    using MediatR;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Primitives;
    using Models;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using Requests;
    using Services;
    using TransactionMobile.Maui.Database;
    using UIServices;

    public class LoginPageViewModel : BaseViewModel
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
                                  IDialogService dialogService)
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

        private void CacheUseTrainingMode() => this.ApplicationCache.SetUseTrainingMode(this.useTrainingMode);

        private async Task GetConfiguration() {
            String deviceIdentifier = this.DeviceService.GetIdentifier();
            GetConfigurationRequest getConfigurationRequest = GetConfigurationRequest.Create(deviceIdentifier);
            Configuration configuration = await this.Mediator.Send(getConfigurationRequest);

            if (configuration == null) {
                throw new ApplicationException("Error getting device configuration.");
            }

            // Cache the config object
            this.ApplicationCache.SetConfiguration(configuration);
        }

        private async Task GetUserToken() {
            LoginRequest loginRequest = LoginRequest.Create(this.UserName, this.Password);
            TokenResponseModel token = await this.Mediator.Send(loginRequest);

            if (token == null) {
                throw new ApplicationException($"Login failed for user {this.UserName}");
            }

            // Cache the token
            this.CacheAccessToken(token);
        }

        private async Task PerformLogonTransaction() {
            // Logon Transaction
            String deviceIdentifier = this.DeviceService.GetIdentifier();
            LogonTransactionRequest logonTransactionRequest = LogonTransactionRequest.Create(DateTime.Now, "1",
                                                                                             deviceIdentifier,
                                                                                             this.ApplicationInfoService.VersionString);
            PerformLogonResponseModel logonResponse = await this.Mediator.Send(logonTransactionRequest);

            if (logonResponse.IsSuccessful == false)
            {
                throw new ApplicationException($"Error during Logon Transaction. Error Msg: {logonResponse.ResponseMessage}");
            }
            
            // Set the user information
            this.ApplicationCache.SetEstateId(logonResponse.EstateId);
            this.ApplicationCache.SetMerchantId(logonResponse.MerchantId);
        }

        private async Task GetMerchantContractProducts() {
            // Get Contracts
            GetContractProductsRequest getContractProductsRequest = GetContractProductsRequest.Create();
            List<ContractProductModel> products = await this.Mediator.Send(getContractProductsRequest);

            if (products.Any() == false)
            {
                throw new ApplicationException($"Error getting contract products.");
            }

            this.CacheContractData(products);
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

        private async Task GetMerchantBalance() {
            // Get the merchant balance
            // TODO: Cache the result, but will add this to a timer call to keep up to date...
            GetMerchantBalanceRequest getMerchantBalanceRequest = GetMerchantBalanceRequest.Create();
            await this.Mediator.Send(getMerchantBalanceRequest);
        }


        private async Task LoginCommandExecute()
        {
            try {
                Shared.Logger.Logger.LogInformation("LoginCommandExecute called");

                this.CacheUseTrainingMode();

                await this.GetConfiguration();

                await this.GetUserToken();

                await this.PerformLogonTransaction();
                
                await this.GetMerchantContractProducts();
                
                await this.GetMerchantBalance();
                
                this.ApplicationCache.SetIsLoggedIn(true);

                await this.NavigationService.GoToHome();
            }
            catch(ApplicationException aex) {
                await this.DialogService.ShowWarningToast(aex.Message);
            }
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
                TokenResponseModel newToken = await this.Mediator.Send(request, CancellationToken.None);

                this.CacheAccessToken(newToken);
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