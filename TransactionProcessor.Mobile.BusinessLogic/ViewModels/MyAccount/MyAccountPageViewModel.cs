using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using MvvmHelpers.Commands;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount
{
    public partial class MyAccountPageViewModel : ExtendedBaseViewModel
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
                                      IDeviceService deviceService,
                                      IMediator mediator,
                                      INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService,navigationParameterService) {
            this.Mediator = mediator;
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

        [RelayCommand]
        private async Task OptionSelected(ItemSelected<ListViewItem> arg) {
            CorrelationIdProvider.NewId();
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