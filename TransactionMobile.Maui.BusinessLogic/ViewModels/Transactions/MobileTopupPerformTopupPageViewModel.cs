namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Web;
using System.Windows.Input;
using Maui.UIServices;
using MediatR;
using Microsoft.Maui.Controls;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Requests;
using Services;
using UIServices;
using Command = Microsoft.Maui.Controls.Command;

public class MobileTopupPerformTopupPageViewModel : ExtendedBaseViewModel, IQueryAttributable
{
    #region Fields

    private String customerEmailAddress;

    private String customerMobileNumber;

    private readonly IMediator Mediator;

    private Decimal topupAmount;

    public ProductDetails ProductDetails { get; set; }

    #endregion

    #region Constructors

    public void ApplyQueryAttributes(IDictionary<string, Object> query)
    {
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
        this.TopupAmount = Decimal.Parse(HttpUtility.UrlDecode(query[nameof(TopupAmount)].ToString()));
    }

    public MobileTopupPerformTopupPageViewModel(IMediator mediator,
                                                INavigationService navigationService,
        IApplicationCache applicationCache,
                      IDialogService dialogService) : base(applicationCache, dialogService, navigationService)
    {
        this.Mediator = mediator;
        this.PerformTopupCommand = new AsyncCommand(this.PerformTopupCommandExecute);
        this.CustomerMobileNumberEntryCompletedCommand = new Command(this.CustomerMobileNumberEntryCompletedCommandExecute);
        this.TopupAmountEntryCompletedCommand = new Command(this.TopupAmountEntryCompletedCommandExecute);
        this.CustomerEmailAddressEntryCompletedCommand = new Command(this.CustomerEmailAddressEntryCompletedCommandExecute);
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
        // Create Command and Send
        PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(DateTime.Now,
                                                                             this.ProductDetails.ContractId,
                                                                             this.ProductDetails.ProductId,
                                                                             this.ProductDetails.OperatorIdentifier,
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