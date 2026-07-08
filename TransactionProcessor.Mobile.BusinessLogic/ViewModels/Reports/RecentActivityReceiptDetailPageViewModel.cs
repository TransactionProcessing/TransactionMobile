using CommunityToolkit.Mvvm.Input;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using SimpleResults;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

public sealed partial class RecentActivityReceiptDetailPageViewModel : ExtendedBaseViewModel
{
    private RecentActivityReceiptItemModel? selectedItem;
    private string? emailAddress;
    private bool isResendingReceipt;
    private readonly IReportsService reportsService;

    public RecentActivityReceiptDetailPageViewModel(INavigationService navigationService,
                                                    IApplicationCache applicationCache,
                                                    IDialogService dialogService,
                                                    IDeviceService deviceService,
                                                    INavigationParameterService navigationParameterService,
                                                    IReportsService reportsService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
    {
        this.Title = "Receipt Detail";
        this.reportsService = reportsService;
    }

    public RecentActivityReceiptItemModel? SelectedItem
    {
        get => this.selectedItem;
        private set
        {
            if (SetProperty(ref this.selectedItem, value))
            {
                this.OnPropertyChanged(nameof(this.Reference));
                this.OnPropertyChanged(nameof(this.TransactionType));
                this.OnPropertyChanged(nameof(this.Product));
                this.OnPropertyChanged(nameof(this.Operator));
                this.OnPropertyChanged(nameof(this.Status));
                this.OnPropertyChanged(nameof(this.Amount));
                this.OnPropertyChanged(nameof(this.TransactionDateTime));
                this.OnPropertyChanged(nameof(this.ReceiptReference));
                this.OnPropertyChanged(nameof(this.HasReceipt));
                this.ResendReceiptCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public string? EmailAddress
    {
        get => this.emailAddress;
        set
        {
            if (SetProperty(ref this.emailAddress, value))
            {
                this.ResendReceiptCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public bool IsResendingReceipt
    {
        get => this.isResendingReceipt;
        private set
        {
            if (SetProperty(ref this.isResendingReceipt, value))
            {
                this.ResendReceiptCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public string Reference => this.SelectedItem?.Reference ?? string.Empty;

    public string TransactionType => this.SelectedItem?.TransactionType ?? string.Empty;

    public string Product => this.SelectedItem?.Product ?? string.Empty;

    public string Operator => this.SelectedItem?.Operator ?? string.Empty;

    public string Status => this.SelectedItem?.Status ?? string.Empty;

    public string Amount => this.SelectedItem is null ? string.Empty : this.SelectedItem.Amount.ToString("N2");

    public string TransactionDateTime => this.SelectedItem is null ? string.Empty : this.SelectedItem.TransactionDateTime.ToString("dd MMM yyyy HH:mm");

    public string ReceiptReference => this.SelectedItem?.ReceiptReference ?? string.Empty;

    public bool HasReceipt => this.SelectedItem is not null;

    [RelayCommand(CanExecute = nameof(CanResendReceipt))]
    private async Task ResendReceipt()
    {
        if (this.CanResendReceipt() == false || this.IsValidEmailAddress(this.EmailAddress) == false)
        {
            await this.DialogService.ShowWarningToast("Enter a valid email address before resending the receipt.");
            return;
        }

        this.IsResendingReceipt = true;
        try
        {
            string recipientEmailAddress = this.EmailAddress!.Trim();
            Result<RecentActivityReceiptResendResultModel> result = await this.reportsService.ResendRecentActivityReceipt(this.Reference,
                                                                                                                           recipientEmailAddress,
                                                                                                                           CancellationToken.None);

            if (result.IsFailed)
            {
                await this.DialogService.ShowWarningToast(result.Message ?? "Unable to resend the receipt.");
                return;
            }

            string message = string.IsNullOrWhiteSpace(result.Data.Message)
                ? $"Receipt sent to {recipientEmailAddress}."
                : $"{result.Data.Message} Sent to {recipientEmailAddress}.";

            this.EmailAddress = string.Empty;
            await this.DialogService.ShowSuccessToast(message);
        }
        finally
        {
            this.IsResendingReceipt = false;
        }
    }

    private bool CanResendReceipt()
    {
        return this.HasReceipt
            && this.IsResendingReceipt == false
            && string.IsNullOrWhiteSpace(this.EmailAddress) == false;
    }

    private bool IsValidEmailAddress(string? emailAddress)
    {
        return string.IsNullOrWhiteSpace(emailAddress) == false
            && emailAddress.Contains('@', StringComparison.OrdinalIgnoreCase)
            && emailAddress.Contains('.', StringComparison.OrdinalIgnoreCase);
    }

    public override async Task Initialise(CancellationToken cancellationToken)
    {
        await base.Initialise(cancellationToken);

        IDictionary<string, object> parameters = this.NavigationParameterService.GetParameters();
        if (parameters.TryGetValue(nameof(RecentActivityReceiptItemModel), out object? item) && item is RecentActivityReceiptItemModel selectedItem)
        {
            this.SelectedItem = selectedItem;
        }
    }
}
