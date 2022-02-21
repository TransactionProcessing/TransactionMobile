namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Web;
using System.Windows.Input;
using MediatR;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Requests;
using UIServices;
using Command = Microsoft.Maui.Controls.Command;

public class VoucherPerformIssuePageViewModel : BaseViewModel, IQueryAttributable
{
    #region Fields

    private String customerEmailAddress;

    private String recipientMobileNumber;

    private String recipientEmailAddress;

    private readonly IMediator Mediator;

    private readonly INavigationService NavigationService;

    private Decimal voucherAmount;

    #endregion

    #region Constructors

    public void ApplyQueryAttributes(IDictionary<string, Object> query)
    {
        this.ContractId = HttpUtility.UrlDecode(query[nameof(this.ContractId)].ToString());
        this.ProductId = HttpUtility.UrlDecode(query[nameof(this.ProductId)].ToString());
        this.OperatorIdentifier = HttpUtility.UrlDecode(query[nameof(this.OperatorIdentifier)].ToString());
        this.VoucherAmount = Decimal.Parse(HttpUtility.UrlDecode(query[nameof(this.VoucherAmount)].ToString()));
    }

    public VoucherPerformIssuePageViewModel(IMediator mediator, INavigationService navigationService)
    {
        this.Mediator = mediator;
        this.NavigationService = navigationService;
        this.IssueVoucherCommand = new AsyncCommand(this.IssueVoucherCommandExecute);
        this.RecipientMobileNumberEntryCompletedCommand = new Command(this.RecipientMobileNumberEntryCompletedCommandExecute);
        this.RecipientEmailAddressEntryCompletedCommand = new Command(this.RecipientEmailAddressEntryCompletedCommandExecute);
        this.VoucherAmountEntryCompletedCommand = new Command(this.VoucherAmountEntryCompletedCommandExecute);
        this.CustomerEmailAddressEntryCompletedCommand = new Command(this.CustomerEmailAddressEntryCompletedCommandExecute);
        this.Title = "Enter Voucher Issue Details";
    }

    #endregion

    #region Properties

    public String ContractId { get; set; }

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

    public String OperatorIdentifier { get; set; }

    public ICommand IssueVoucherCommand { get; }

    public String ProductId { get; set; }

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
        this.OnCustomerEmailAddressEntryCompleted();
    }

    private void RecipientMobileNumberEntryCompletedCommandExecute()
    {
        this.OnRecipientMobileNumberEntryCompleted();
    }

    private void RecipientEmailAddressEntryCompletedCommandExecute()
    {
        this.OnRecipientEmailAddressEntryCompleted();
    }

    private async Task IssueVoucherCommandExecute()
    {
        // TODO: Create Command and Send
        PerformVoucherIssueRequest request = PerformVoucherIssueRequest.Create(DateTime.Now,
                                                                               "1",
                                                                               "",
                                                                               "",
                                                                               Guid.Parse(this.ContractId),
                                                                               Guid.Parse(this.ProductId),
                                                                               this.OperatorIdentifier,
                                                                               this.RecipientMobileNumber,
                                                                               this.recipientMobileNumber,
                                                                               this.VoucherAmount,
                                                                               this.CustomerEmailAddress);

        Boolean response = await this.Mediator.Send(request);

        if (response)
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
        this.OnVoucherAmountEntryCompleted();
    }

    #endregion
}