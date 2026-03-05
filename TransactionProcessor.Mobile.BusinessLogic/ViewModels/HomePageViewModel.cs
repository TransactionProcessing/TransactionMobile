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
        this.GoToReportsCommand = new AsyncCommand(async () => await this.NavigationService.GoToReports());
        this.GoToMyAccountCommand = new AsyncCommand(async () => await this.NavigationService.GoToMyAccount());
        this.GoToSupportCommand = new AsyncCommand(async () => await this.NavigationService.GoToSupport());
    }

    public ICommand GoToTransactionsCommand { get; }
    public ICommand GoToReportsCommand { get; }
    public ICommand GoToMyAccountCommand { get; }
    public ICommand GoToSupportCommand { get; }
}