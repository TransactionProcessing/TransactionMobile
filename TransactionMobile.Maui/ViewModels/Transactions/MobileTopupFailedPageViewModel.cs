namespace TransactionMobile.Maui.ViewModels.Transactions;

using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;

public class MobileTopupFailedPageViewModel : BaseViewModel
{
    #region Constructors

    public MobileTopupFailedPageViewModel()
    {
        this.CancelledCommand = new AsyncCommand(this.CancelledCommandExecute);
    }

    #endregion

    #region Properties

    public ICommand CancelledCommand { get; }

    #endregion

    #region Methods

    private async Task CancelledCommandExecute()
    {
        await Shell.Current.Navigation.PopToRootAsync();
    }

    #endregion
}