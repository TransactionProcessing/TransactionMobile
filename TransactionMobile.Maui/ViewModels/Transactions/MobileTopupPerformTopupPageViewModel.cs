namespace TransactionMobile.Maui.ViewModels.Transactions;

using System.Windows.Input;
using BusinessLogic.Requests;
using MediatR;
using MvvmHelpers;
using MvvmHelpers.Commands;
using UIServices;

[QueryProperty(nameof(MobileTopupPerformTopupPageViewModel.ContractId), nameof(MobileTopupPerformTopupPageViewModel.ContractId))]
[QueryProperty(nameof(MobileTopupPerformTopupPageViewModel.ProductId), nameof(MobileTopupPerformTopupPageViewModel.ProductId))]
[QueryProperty(nameof(MobileTopupPerformTopupPageViewModel.OperatorIdentifer), nameof(MobileTopupPerformTopupPageViewModel.OperatorIdentifer))]
[QueryProperty(nameof(MobileTopupPerformTopupPageViewModel.TopupAmount), nameof(MobileTopupPerformTopupPageViewModel.TopupAmount))]
public class MobileTopupPerformTopupPageViewModel : BaseViewModel
{
    #region Fields

    private String customerEmailAddress;

    private String customerMobileNumber;

    private readonly IMediator Mediator;

    private readonly INavigationService NavigationService;

    private Decimal topupAmount;

    #endregion

    #region Constructors

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

    public String OperatorIdentifer { get; set; }

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
        this.OnCustomerEmailAddressEntryCompleted();
    }

    private void CustomerMobileNumberEntryCompletedCommandExecute()
    {
        this.OnCustomerMobileNumberEntryCompleted();
    }

    private async Task PerformTopupCommandExecute()
    {
        // TODO: Create Command and Send
        PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(DateTime.Now,
                                                                             "1",
                                                                             "",
                                                                             "",
                                                                             Guid.Parse(this.ContractId),
                                                                             Guid.Parse(this.ProductId),
                                                                             this.OperatorIdentifer,
                                                                             this.CustomerEmailAddress,
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
        this.OnTopupAmountEntryCompleted();
    }

    #endregion
}