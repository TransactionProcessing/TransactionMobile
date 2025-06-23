using System.Web;
using System.Windows.Input;
using MediatR;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public class VoucherPerformIssuePageViewModel : ExtendedBaseViewModel
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

    public async Task Initialise(CancellationToken cancellationToken)
    {
        Logger.LogInformation("in Initialise");
        var query = this.NavigationParameterService.GetParameters();
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
        this.VoucherAmount = Decimal.Parse(HttpUtility.UrlDecode(query[nameof(this.VoucherAmount)].ToString()));
    }

    public VoucherPerformIssuePageViewModel(INavigationService navigationService,
                                            IApplicationCache applicationCache,
                                            IDialogService dialogService,
                                            IDeviceService deviceService,
                                            IMediator mediator,
                                            INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
    {
        this.Mediator = mediator;
        this.IssueVoucherCommand = new AsyncCommand(this.IssueVoucherCommandExecute);
        this.RecipientMobileNumberEntryCompletedCommand = new AsyncCommand(this.RecipientMobileNumberEntryCompletedCommandExecute);
        this.RecipientEmailAddressEntryCompletedCommand = new AsyncCommand(this.RecipientEmailAddressEntryCompletedCommandExecute);
        this.VoucherAmountEntryCompletedCommand = new AsyncCommand(this.VoucherAmountEntryCompletedCommandExecute);
        this.CustomerEmailAddressEntryCompletedCommand = new AsyncCommand(this.CustomerEmailAddressEntryCompletedCommandExecute);
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

    private async Task CustomerEmailAddressEntryCompletedCommandExecute() {
        Logger.LogInformation("CustomerEmailAddressEntryCompletedCommandExecute called");
        this.OnCustomerEmailAddressEntryCompleted();
    }

    private async Task RecipientMobileNumberEntryCompletedCommandExecute() {
        Logger.LogInformation("RecipientMobileNumberEntryCompletedCommandExecute called");
        this.OnRecipientMobileNumberEntryCompleted();
    }

    private async Task RecipientEmailAddressEntryCompletedCommandExecute()
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
                                                                               this.ProductDetails.OperatorId,
                                                                               this.RecipientMobileNumber,
                                                                               this.recipientMobileNumber,
                                                                               this.VoucherAmount,
                                                                               this.CustomerEmailAddress);


        Logger.LogInformation("about to call Send ");
        var result = await this.Mediator.Send(request);
        Logger.LogInformation("about to check result ");
        if (result.IsSuccess && result.Data.IsSuccessful)
        {
            Logger.LogInformation("about to go to success");
            await this.NavigationService.GoToVoucherIssueSuccessPage();

        }
        else
        {
            Logger.LogInformation("about to go to failed");
            await this.NavigationService.GoToVoucherIssueFailedPage();
        }
    }

    private async Task VoucherAmountEntryCompletedCommandExecute()
    {
        Logger.LogInformation("VoucherAmountEntryCompletedCommandExecute called");
        this.OnVoucherAmountEntryCompleted();
    }

    #endregion
}