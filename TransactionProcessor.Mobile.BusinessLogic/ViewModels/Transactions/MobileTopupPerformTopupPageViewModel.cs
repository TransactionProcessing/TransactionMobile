using System.Web;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public partial class MobileTopupPerformTopupPageViewModel : ExtendedBaseViewModel
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
        this.Title = "Enter Topup Details";
    }

    #endregion

    #region Properties

    public Action OnCustomerEmailAddressEntryCompleted { get; set; }

    public Action OnCustomerMobileNumberEntryCompleted { get; set; }

    public Action OnTopupAmountEntryCompleted { get; set; }

    public String CustomerEmailAddress
    {
        get => this.customerEmailAddress;
        set => this.SetProperty(ref this.customerEmailAddress, value);
    }
    
    public String CustomerMobileNumber
    {
        get => this.customerMobileNumber;
        set => this.SetProperty(ref this.customerMobileNumber, value);
    }
    
    public Decimal TopupAmount
    {
        get => this.topupAmount;
        set => this.SetProperty(ref this.topupAmount, value);
    }

    #endregion

    #region Methods

    [RelayCommand]
    private async Task CustomerEmailAddressEntryCompleted()
    {
        Logger.LogInformation("CustomerEmailAddressEntryCompleted called");
        this.OnCustomerEmailAddressEntryCompleted();
    }

    [RelayCommand]
    private async Task CustomerMobileNumberEntryCompleted()
    {
        Logger.LogInformation("CustomerMobileNumberEntryCompleted called");
        this.OnCustomerMobileNumberEntryCompleted();
    }

    [RelayCommand]
    private async Task PerformTopup()
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

    [RelayCommand]
    private async Task TopupAmountEntryCompleted()
    {
        Logger.LogInformation("TopupAmountEntryCompleted called");
        this.OnTopupAmountEntryCompleted();
    }

    #endregion
}