namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Web;
using System.Windows.Input;
using Common;
using Maui.UIServices;
using MediatR;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Requests;
using Services;
using UIServices;
using Command = Microsoft.Maui.Controls.Command;

public class VoucherPerformIssuePageViewModel : ExtendedBaseViewModel, IQueryAttributable
{
    #region Fields

    private String customerEmailAddress;

    private String recipientMobileNumber;

    private String recipientEmailAddress;

    private readonly IMediator Mediator;
    
    private Decimal voucherAmount;

    public ProductDetails ProductDetails { get; private set; }

    #endregion

    #region Constructors

    public void ApplyQueryAttributes(IDictionary<string, Object> query)
    {
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
        this.VoucherAmount = Decimal.Parse(HttpUtility.UrlDecode(query[nameof(this.VoucherAmount)].ToString()));
    }

    public VoucherPerformIssuePageViewModel(INavigationService navigationService,
                                            IApplicationCache applicationCache,
                                            IDialogService dialogService,
                                            IMediator mediator) : base(applicationCache, dialogService, navigationService)
    {
        this.Mediator = mediator;
        this.IssueVoucherCommand = new AsyncCommand(this.IssueVoucherCommandExecute);
        this.RecipientMobileNumberEntryCompletedCommand = new Command(this.RecipientMobileNumberEntryCompletedCommandExecute);
        this.RecipientEmailAddressEntryCompletedCommand = new Command(this.RecipientEmailAddressEntryCompletedCommandExecute);
        this.VoucherAmountEntryCompletedCommand = new Command(this.VoucherAmountEntryCompletedCommandExecute);
        this.CustomerEmailAddressEntryCompletedCommand = new Command(this.CustomerEmailAddressEntryCompletedCommandExecute);
        this.Title = "Enter Voucher Issue Details";
    }

    #endregion

    #region Properties
    
    public String CustomerEmailAddress
    {
        get => this.customerEmailAddress;
        set => this.SetProperty(ref this.customerEmailAddress, value);
    }

    public ICommand CustomerEmailAddressEntryCompletedCommand { get; }

    public String RecipientMobileNumber
    {
        get => this.recipientMobileNumber;
        set => this.SetProperty(ref this.recipientMobileNumber, value);
    }

    public String RecipientEmailAddress
    {
        get => this.recipientEmailAddress;
        set => this.SetProperty(ref this.recipientEmailAddress, value);
    }

    public ICommand RecipientMobileNumberEntryCompletedCommand { get; }

    public ICommand RecipientEmailAddressEntryCompletedCommand { get; }

    public Action OnCustomerEmailAddressEntryCompleted { get; set; }

    public Action OnRecipientEmailAddressEntryCompleted { get; set; }

    public Action OnRecipientMobileNumberEntryCompleted { get; set; }

    public Action OnVoucherAmountEntryCompleted { get; set; }
    
    public ICommand IssueVoucherCommand { get; }

    public Decimal VoucherAmount
    {
        get => this.voucherAmount;
        set => this.SetProperty(ref this.voucherAmount, value);
    }

    public ICommand VoucherAmountEntryCompletedCommand { get; }

    #endregion

    #region Methods

    private void CustomerEmailAddressEntryCompletedCommandExecute()
    {
        Logger.LogInformation("CustomerEmailAddressEntryCompletedCommandExecute called");
        this.OnCustomerEmailAddressEntryCompleted();
    }

    private void RecipientMobileNumberEntryCompletedCommandExecute()
    {
        Logger.LogInformation("RecipientMobileNumberEntryCompletedCommandExecute called");
        this.OnRecipientMobileNumberEntryCompleted();
    }

    private void RecipientEmailAddressEntryCompletedCommandExecute()
    {
        Logger.LogInformation("RecipientEmailAddressEntryCompletedCommandExecute called");
        this.OnRecipientEmailAddressEntryCompleted();
    }

    private async Task IssueVoucherCommandExecute()
    {
        Logger.LogInformation("IssueVoucherCommandExecute called");
        // TODO: Create Command and Send
        PerformVoucherIssueRequest request = PerformVoucherIssueRequest.Create(DateTime.Now,
                                                                               this.ProductDetails.ContractId,
                                                                               this.ProductDetails.ProductId,
                                                                               this.ProductDetails.OperatorIdentifier,
                                                                               this.RecipientMobileNumber,
                                                                               this.recipientMobileNumber,
                                                                               this.VoucherAmount,
                                                                               this.CustomerEmailAddress);

        var result = await this.Mediator.Send(request);

        if (result.Success && result.Data.IsSuccessfulTransaction())
        {
            await this.NavigationService.GoToVoucherIssueSuccessPage();

        }
        else
        {
            await this.NavigationService.GoToVoucherIssueFailedPage();
        }
    }

    private void VoucherAmountEntryCompletedCommandExecute()
    {
        Logger.LogInformation("VoucherAmountEntryCompletedCommandExecute called");
        this.OnVoucherAmountEntryCompleted();
    }

    #endregion
}