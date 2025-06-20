using System.Web;
using System.Windows.Input;
using MediatR;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public class MobileTopupPerformTopupPageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private String customerEmailAddress;

    private String customerMobileNumber;

    private readonly IMediator Mediator;

    private Decimal topupAmount;

    public ProductDetails ProductDetails { get; set; }

    #endregion

    #region Constructors

    public async Task Initialise(CancellationToken cancellationToken)
    {

        var query = this.NavigationParameterService.GetParameters();
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
        this.TopupAmount = Decimal.Parse(HttpUtility.UrlDecode(query[nameof(this.TopupAmount)].ToString()));
    }

    public MobileTopupPerformTopupPageViewModel(IMediator mediator,
                                                INavigationService navigationService,
                                                IApplicationCache applicationCache,
                                                IDialogService dialogService,
                                                IDeviceService deviceService,
                                                INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
    {
        this.Mediator = mediator;
        this.PerformTopupCommand = new AsyncCommand(this.PerformTopupCommandExecute);
        this.CustomerMobileNumberEntryCompletedCommand = new AsyncCommand(this.CustomerMobileNumberEntryCompletedCommandExecute);
        this.TopupAmountEntryCompletedCommand = new AsyncCommand(this.TopupAmountEntryCompletedCommandExecute);
        this.CustomerEmailAddressEntryCompletedCommand = new AsyncCommand(this.CustomerEmailAddressEntryCompletedCommandExecute);
        this.Title = "Enter Topup Details";
    }

    #endregion

    #region Properties
    
    public String CustomerEmailAddress
    {
        get => this.customerEmailAddress;
        set => this.SetProperty(ref this.customerEmailAddress, value);
    }

    public ICommand CustomerEmailAddressEntryCompletedCommand { get; }

    public String CustomerMobileNumber
    {
        get => this.customerMobileNumber;
        set => this.SetProperty(ref this.customerMobileNumber, value);
    }

    public ICommand CustomerMobileNumberEntryCompletedCommand { get; }

    public Action OnCustomerEmailAddressEntryCompleted { get; set; }

    public Action OnCustomerMobileNumberEntryCompleted { get; set; }

    public Action OnTopupAmountEntryCompleted { get; set; }

    public ICommand PerformTopupCommand { get; }

    public Decimal TopupAmount
    {
        get => this.topupAmount;
        set => this.SetProperty(ref this.topupAmount, value);
    }

    public ICommand TopupAmountEntryCompletedCommand { get; }

    #endregion

    #region Methods

    private async Task CustomerEmailAddressEntryCompletedCommandExecute()
    {
        Logger.LogInformation("CustomerEmailAddressEntryCompletedCommandExecute called");
        this.OnCustomerEmailAddressEntryCompleted();
    }

    private async Task CustomerMobileNumberEntryCompletedCommandExecute()
    {
        Logger.LogInformation("CustomerMobileNumberEntryCompletedCommandExecute called");
        this.OnCustomerMobileNumberEntryCompleted();
    }

    private async Task PerformTopupCommandExecute()
    {
        Logger.LogInformation("PerformTopupCommandExecute called");
        // Create Command and Send
        PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(DateTime.Now,
                                                                             this.ProductDetails.ContractId,
                                                                             this.ProductDetails.ProductId,
                                                                             this.ProductDetails.OperatorId,
                                                                             this.CustomerMobileNumber,
                                                                             this.TopupAmount,
                                                                             this.CustomerEmailAddress);

        var response = await this.Mediator.Send(request);

        if (response.IsSuccess && response.Data.IsSuccessful)
        {
            await this.NavigationService.GoToMobileTopupSuccessPage();
        }
        else
        {
            await this.NavigationService.GoToMobileTopupFailedPage();
        }
    }

    private async Task TopupAmountEntryCompletedCommandExecute()
    {
        Logger.LogInformation("TopupAmountEntryCompletedCommandExecute called");
        this.OnTopupAmountEntryCompleted();
    }

    #endregion
}