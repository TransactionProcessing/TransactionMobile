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
        this.GoToMobileTopupCommand = new AsyncCommand(async () => await this.NavigationService.GoToMobileTopupSelectOperatorPage());
        this.GoToBillPaymentCommand = new AsyncCommand(async () => await this.NavigationService.GoToBillPaymentSelectOperatorPage());
        this.GoToVoucherCommand = new AsyncCommand(async () => await this.NavigationService.GoToVoucherSelectOperatorPage());
        this.GoToAdminCommand = new AsyncCommand(async () => await this.NavigationService.GoToAdminPage());
        this.GoToTransactionsCommand = new AsyncCommand(async () => await this.NavigationService.GoToTransactions());
    }

    public ICommand GoToMobileTopupCommand { get; }
    public ICommand GoToBillPaymentCommand { get; }
    public ICommand GoToVoucherCommand { get; }
    public ICommand GoToAdminCommand { get; }
    public ICommand GoToTransactionsCommand { get; }
}