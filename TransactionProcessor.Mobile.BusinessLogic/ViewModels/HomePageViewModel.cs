using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels;

[ExcludeFromCodeCoverage]
public class HomePageViewModel : ExtendedBaseViewModel
{
    public HomePageViewModel(IApplicationCache applicationCache,
                             IDialogService dialogService,
                             IDeviceService deviceService,
                             INavigationService navigationService,
                             INavigationParameterService navigationParameterService) :base(applicationCache,dialogService, navigationService, deviceService,navigationParameterService)
    {
        this.GoToTransactionsCommand = new AsyncCommand(async () => await this.NavigationService.GoToTransactions());
        this.MobileTopupCommand = new AsyncCommand(async () => await this.NavigationService.GoToMobileTopupSelectOperatorPage());
        this.BillPaymentCommand = new AsyncCommand(async () => await this.NavigationService.GoToBillPaymentSelectOperatorPage());
        this.VoucherCommand = new AsyncCommand(async () => await this.NavigationService.GoToVoucherSelectOperatorPage());
    }

    public ICommand GoToTransactionsCommand { get; }
    public ICommand MobileTopupCommand { get; }
    public ICommand BillPaymentCommand { get; }
    public ICommand VoucherCommand { get; }
}