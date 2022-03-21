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

        private readonly IMemoryCacheService MemoryCacheService;
        
        private readonly IDeviceService DeviceService;

        private readonly IApplicationInfoService ApplicationInfoService;

        private String userName;

        private String password;

        #region Constructors
        
        public LoginPageViewModel(IMediator mediator, INavigationService navigationService, IMemoryCacheService memoryCacheService,
                                  IDeviceService deviceService,IApplicationInfoService applicationInfoService)
        {
            this.NavigationService = navigationService;
            this.MemoryCacheService = memoryCacheService;
            this.DeviceService = deviceService;
            this.ApplicationInfoService = applicationInfoService;
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

        #endregion

        #region Methods

        private async Task LoginCommandExecute()
        {
            Shared.Logger.Logger.LogInformation("LoginCommandExecute called");
            
            // TODO: this method needs refactored
            String deviceIdentifier = this.DeviceService.GetIdentifier();
            GetConfigurationRequest getConfigurationRequest = GetConfigurationRequest.Create(deviceIdentifier);
            Configuration configuration = await this.Mediator.Send(getConfigurationRequest);
            
            // Cache the config object
            this.MemoryCacheService.Set("Configuration", configuration);

            LoginRequest loginRequest = LoginRequest.Create(this.UserName, this.Password);
            TokenResponseModel token = await this.Mediator.Send(loginRequest);

            //if (token == null)
            //{
            // TODO: Some kind of error handling
            //}
            // Cache the token
            this.CacheAccessToken(token);

            // Logon Transaction
            LogonTransactionRequest logonTransactionRequest = LogonTransactionRequest.Create(DateTime.Now, "1", 
                                                                                             deviceIdentifier,
                                                                                             this.ApplicationInfoService.VersionString);
            PerformLogonResponseModel logonResponse = await this.Mediator.Send(logonTransactionRequest);

            if (logonResponse.IsSuccessful == false)
            {
                // TODO: Throw an error here
            }

            // Set the user information
            this.MemoryCacheService.Set("EstateId", logonResponse.EstateId);
            this.MemoryCacheService.Set("MerchantId", logonResponse.MerchantId);

            // Get Contracts
            GetContractProductsRequest getContractProductsRequest = GetContractProductsRequest.Create();
            await this.Mediator.Send(getContractProductsRequest);
            
            // Get the merchant balance
            // TODO: Cache the result, but will add this to a timer call to keep up to date...
            GetMerchantBalanceRequest getMerchantBalanceRequest = GetMerchantBalanceRequest.Create();
            await this.Mediator.Send(getMerchantBalanceRequest);

            await this.NavigationService.GoToHome();
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

            this.MemoryCacheService.Set("AccessToken", token, cacheEntryOptions);
        }
        
        #endregion
    }
}