namespace TransactionMobile.Maui.BusinessLogic.ViewModels
{
    using System.Windows.Input;
    using Maui.UIServices;
    using MediatR;
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Distribute;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Maui.Devices;
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

        private Boolean useTrainingMode;

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

        public Boolean UseTrainingMode
        {
            get => this.useTrainingMode;
            set => this.SetProperty(ref this.useTrainingMode, value);
        }

        #endregion

        #region Methods

        private async Task LoginCommandExecute()
        {
            Shared.Logger.Logger.LogInformation("LoginCommandExecute called");

            this.MemoryCacheService.Set("UseTrainingMode", this.useTrainingMode);

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

            // TODO: Need to set the application as in training mode somehow

            this.MemoryCacheService.Set("IsLoggedIn", true);

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

    public class HomePageViewModel : BaseViewModel
    {
        private readonly IMemoryCacheService MemoryCacheService;

        private readonly IDialogService DialogService;

        public HomePageViewModel(IMemoryCacheService memoryCacheService,
                                 IDialogService dialogService) {
            this.MemoryCacheService = memoryCacheService;
            this.DialogService = dialogService;
        }

        public async Task Initialise(CancellationToken cancellationToken) {

            this.MemoryCacheService.TryGetValue("Configuration", out Configuration configuration);

            if (configuration.EnableAutoUpdates) {
                await Distribute.SetEnabledAsync(true);
                Distribute.CheckForUpdate();
                Distribute.ReleaseAvailable = OnReleaseAvailable;
                Distribute.UpdateTrack = UpdateTrack.Public;
            }
            else {
                Distribute.DisableAutomaticCheckForUpdate();
            }

            if (this.IsIOS() == false) {
                // TODO: Move the keys to config service
                AppCenter.Configure("android=f920cc96-de56-42fe-87d4-b49105761205;" + "ios=dd940171-ca8c-4219-9851-f83769464f37;" +
                                    "uwp=3ad27ea3-3f24-4579-a88a-530025bd00d4;" + "macos=244fdee2-f897-431a-8bab-5081fc90b329;");
                AppCenter.Start(typeof(Distribute));
            }
        }

        private bool IsIOS() =>
            DeviceInfo.Current.Platform == DevicePlatform.iOS;

        private Boolean OnReleaseAvailable(ReleaseDetails releaseDetails) {
            // Look at releaseDetails public properties to get version information, release notes text or release notes URL
            String versionName = releaseDetails.ShortVersion;
            String versionCodeOrBuildNumber = releaseDetails.Version;
            String releaseNotes = releaseDetails.ReleaseNotes;
            Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;

            // custom dialog
            String title = "Version " + versionName + " available!";
            Task answer;

            // On mandatory update, user can't postpone
            if (releaseDetails.MandatoryUpdate) {
                answer = this.DialogService.ShowDialog(title, releaseNotes, "Download and Install");
            }
            else {
                answer = this.DialogService.ShowDialog(title, releaseNotes, "Download and Install", "Later");
            }

            answer.ContinueWith(task => {
                                    // If mandatory or if answer was positive
                                    if (releaseDetails.MandatoryUpdate || (task as Task<Boolean>).Result) {
                                        // Notify SDK that user selected update
                                        Distribute.NotifyUpdateAction(UpdateAction.Update);
                                    }
                                    else {
                                        // Notify SDK that user selected postpone (for 1 day)
                                        // This method call is ignored by the SDK if the update is mandatory
                                        Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                                    }
                                });

            // Return true if you're using your own dialog, false otherwise
            return true;
        }

    }
}