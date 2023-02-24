namespace TransactionMobile.Maui.BusinessLogic.ViewModels.MyAccount
{
    using System.Windows.Input;
    using Common;
    using Logging;
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
    using UIServices;

    public class MyAccountPageViewModel : ExtendedBaseViewModel
    {
        #region Fields

        private DateTime lastLogin;

        private readonly IMediator Mediator;

        private String merchantName;

        #endregion

        #region Constructors

        public MyAccountPageViewModel(INavigationService navigationService,
                                      IApplicationCache applicationCache,
                                      IDialogService dialogService,
                                      IMediator mediator) : base(applicationCache, dialogService, navigationService) {
            this.Mediator = mediator;
            this.OptionSelectedCommand = new AsyncCommand<ItemSelected<ListViewItem>>(this.OptionSelectedCommandExecute);
            this.Title = "My Account";
        }

        #endregion

        #region Properties

        public DateTime LastLogin {
            get => this.lastLogin;
            set => this.SetProperty(ref this.lastLogin, value);
        }

        public String MerchantName {
            get => this.merchantName;
            set => this.SetProperty(ref this.merchantName, value);
        }

        public List<ListViewItem> MyAccountOptions { get; set; }

        public ICommand OptionSelectedCommand { get; set; }

        #endregion

        #region Methods

        public async Task Initialise(CancellationToken cancellationToken) {
            this.MyAccountOptions = new List<ListViewItem> {
                                                               new ListViewItem {
                                                                                    Title = "Addresses"
                                                                                },
                                                               new ListViewItem {
                                                                                    Title = "Contacts"
                                                                                },
                                                               new ListViewItem {
                                                                                    Title = "Account Info"
                                                                                },
                                                               new ListViewItem {
                                                                                    Title = "Logout"
                                                                                }
                                                           };

            GetMerchantDetailsRequest request = GetMerchantDetailsRequest.Create();

            Result<MerchantDetailsModel> merchantDetailsResult = await this.Mediator.Send(request, cancellationToken);
            // TODO: handle failure result
            this.MerchantName = merchantDetailsResult.Data.MerchantName;

            DateTime expirationTime = DateTime.Now.AddMinutes(60);
            CancellationChangeToken expirationToken = new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMinutes(60)).Token);
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                                                        // Pin to cache.
                                                        .SetPriority(CacheItemPriority.NeverRemove)
                                                        // Set the actual expiration time
                                                        .SetAbsoluteExpiration(expirationTime)
                                                        // Force eviction to run
                                                        .AddExpirationToken(expirationToken);

            this.ApplicationCache.SetMerchantDetails(merchantDetailsResult.Data, cacheEntryOptions);

            this.LastLogin = DateTime.Now; // TODO: might cache this in the application
        }

        private async Task LogoutCommandExecute() {
            Logger.LogInformation("LogoutCommand called");
            this.ApplicationCache.SetAccessToken(null);
            this.ApplicationCache.SetIsLoggedIn(false);

            await this.NavigationService.GoToLoginPage();
        }

        private async Task OptionSelectedCommandExecute(ItemSelected<ListViewItem> arg) {
            AccountOptions selectedOption = (AccountOptions)arg.SelectedItemIndex;

            Task navigationTask = selectedOption switch {
                AccountOptions.Addresses => this.NavigationService.GoToMyAccountAddresses(),
                AccountOptions.Contacts => this.NavigationService.GoToMyAccountContacts(),
                AccountOptions.AccountInfo => this.NavigationService.GoToMyAccountDetails(),
                AccountOptions.Logout => this.LogoutCommandExecute(),
                _ => Task.Factory.StartNew(() => Logger.LogWarning($"Unsupported option selected {selectedOption}"))
            };

            await navigationTask;
        }

        #endregion

        #region Others

        public enum AccountOptions
        {
            Addresses = 0,

            Contacts = 1,

            AccountInfo = 2,

            Logout = 3
        }

        #endregion
    }
}