namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Web;
using System.Windows.Input;
using Maui.UIServices;
using MediatR;
using Microsoft.Maui.Controls;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Requests;
using UIServices;
using Command = Microsoft.Maui.Controls.Command;

public class MobileTopupPerformTopupPageViewModel : BaseViewModel, IQueryAttributable
{
    #region Fields

    private String customerEmailAddress;

    private String customerMobileNumber;

    private readonly IMediator Mediator;

    private readonly INavigationService NavigationService;

    private Decimal topupAmount;

    #endregion

    #region Constructors

    public void ApplyQueryAttributes(IDictionary<string, Object> query)
    {
        this.ContractId = HttpUtility.UrlDecode(query[nameof(ContractId)].ToString());
        this.ProductId = HttpUtility.UrlDecode(query[nameof(ProductId)].ToString());
        this.OperatorIdentifier = HttpUtility.UrlDecode(query[nameof(OperatorIdentifier)].ToString());
        this.TopupAmount = Decimal.Parse(HttpUtility.UrlDecode(query[nameof(TopupAmount)].ToString()));
    }

    public MobileTopupPerformTopupPageViewModel(IMediator mediator, INavigationService navigationService)
    {
        this.Mediator = mediator;
        this.NavigationService = navigationService;
        this.PerformTopupCommand = new AsyncCommand(this.PerformTopupCommandExecute);
        this.CustomerMobileNumberEntryCompletedCommand = new Command(this.CustomerMobileNumberEntryCompletedCommandExecute);
        this.TopupAmountEntryCompletedCommand = new Command(this.TopupAmountEntryCompletedCommandExecute);
        this.CustomerEmailAddressEntryCompletedCommand = new Command(this.CustomerEmailAddressEntryCompletedCommandExecute);
        this.Title = "Enter Topup Details";
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

    public String CustomerMobileNumber
    {
        get => this.customerMobileNumber;
        set => this.SetProperty(ref this.customerMobileNumber, value);
    }

    public ICommand CustomerMobileNumberEntryCompletedCommand { get; }

    public Action OnCustomerEmailAddressEntryCompleted { get; set; }

    public Action OnCustomerMobileNumberEntryCompleted { get; set; }

    public Action OnTopupAmountEntryCompleted { get; set; }

    public String OperatorIdentifier { get; set; }

    public ICommand PerformTopupCommand { get; }

    public String ProductId { get; set; }

    public Decimal TopupAmount
    {
        get => this.topupAmount;
        set => this.SetProperty(ref this.topupAmount, value);
    }

    public ICommand TopupAmountEntryCompletedCommand { get; }

    #endregion

    #region Methods

    private void CustomerEmailAddressEntryCompletedCommandExecute()
    {
        Shared.Logger.Logger.LogInformation("CustomerEmailAddressEntryCompletedCommandExecute called");
        this.OnCustomerEmailAddressEntryCompleted();
    }

    private void CustomerMobileNumberEntryCompletedCommandExecute()
    {
        Shared.Logger.Logger.LogInformation("CustomerMobileNumberEntryCompletedCommandExecute called");
        this.OnCustomerMobileNumberEntryCompleted();
    }

    private async Task PerformTopupCommandExecute()
    {
        Shared.Logger.Logger.LogInformation("PerformTopupCommandExecute called");
        // TODO: Create Command and Send
        PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(DateTime.Now,
                                                                             "1",
                                                                             "",
                                                                             "",
                                                                             Guid.Parse(this.ContractId),
                                                                             Guid.Parse(this.ProductId),
                                                                             this.OperatorIdentifier,
                                                                             this.CustomerMobileNumber,
                                                                             this.TopupAmount,
                                                                             this.CustomerEmailAddress);

        Boolean response = await this.Mediator.Send(request);

        if (response)
        {
            await this.NavigationService.GoToMobileTopupSuccessPage();

        }
        else
        {
            await this.NavigationService.GoToMobileTopupFailedPage();
        }
    }

    private void TopupAmountEntryCompletedCommandExecute()
    {
        Shared.Logger.Logger.LogInformation("TopupAmountEntryCompletedCommandExecute called");
        this.OnTopupAmountEntryCompleted();
    }

    #endregion
}