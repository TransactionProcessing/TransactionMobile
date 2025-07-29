using CommunityToolkit.Mvvm.Input;
using MvvmHelpers.Commands;
using System.Windows.Input;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions
{
    public partial class TransactionsPageViewModel : ExtendedBaseViewModel
    {
        #region Constructors

        public TransactionsPageViewModel(INavigationService navigationService,
                                         IApplicationCache applicationCache,
                                         IDialogService dialogService,
                                         IDeviceService deviceService,
                                         INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
        {
            this.Title = "Select Transaction Type";
        }

        #endregion

        #region Methods

        [RelayCommand]
        public async Task Admin()
        {
            Logger.LogInformation("Admin called");
            await this.NavigationService.GoToAdminPage();
        }

        [RelayCommand]
        public async Task BillPayment()
        {
            Logger.LogInformation("Bill Payment called");
            await this.NavigationService.GoToBillPaymentSelectOperatorPage();
        }

        [RelayCommand]
        public async Task MobileTopup()
        {
            Logger.LogInformation("Mobile Topup called");
            await this.NavigationService.GoToMobileTopupSelectOperatorPage();
        }

        [RelayCommand]
        public async Task MobileWallet()
        {
            Logger.LogInformation("MobileWallet called");
            await this.NavigationService.GoToHome();
        }

        [RelayCommand]
        public async Task Voucher()
        {
            Logger.LogInformation("Voucher called");
            await this.NavigationService.GoToVoucherSelectOperatorPage();
        }

        #endregion
    }
}