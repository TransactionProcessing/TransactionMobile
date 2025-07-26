using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MvvmHelpers.Commands;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public partial class BillPaymentPayBillPageViewModel : ExtendedBaseViewModel
{
    #region Fields
    
    private readonly IMediator Mediator;

    private BillDetails billdetails;
    private MeterDetails meterdetails;

    #endregion

    #region Constructors

    public Action OnCustomerMobileNumberEntryCompleted { get; set; }

    public Action OnPaymentAmountEntryCompleted { get; set; }

    public async Task Initialise(CancellationToken cancellationToken){
        IDictionary<String, Object> query = this.NavigationParameterService.GetParameters();
        this.IsPostPayVisible = false;
        this.IsPrePayVisible = false;

        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
        if (query.ContainsKey("BillDetails") == true){
            this.BillDetails = query["BillDetails"] as BillDetails;
            this.IsPostPayVisible = true;
        }

        if (query.ContainsKey("MeterDetails") == true){
            this.IsPrePayVisible = true;
            this.MeterDetails = query["MeterDetails"] as MeterDetails;
        }
    }

    public ProductDetails ProductDetails { get; set; }

    public BillPaymentPayBillPageViewModel(INavigationService navigationService,
                                           IApplicationCache applicationCache,
                                           IDialogService dialogService,
                                           IMediator mediator,
                                           IDeviceService deviceService,
                                           INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
    {
        this.Mediator = mediator;
        this.Title = "Make Bill Payment";
    }
    [RelayCommand]
    private async Task PaymentAmountEntryCompleted() {
        Logger.LogInformation("PaymentAmountEntryCompletedCommandExecute called");
        this.OnPaymentAmountEntryCompleted();
    }

    [RelayCommand]
    private async Task CustomerMobileNumberEntryCompleted() {
        Logger.LogInformation("CustomerMobileNumberEntryCompletedExecute called");
        this.OnCustomerMobileNumberEntryCompleted();
    }

    #endregion

    [RelayCommand]
    private async Task MakeBillPayment() {
        Logger.LogInformation("MakeBillPaymentCommandExecute called");
        IRequest<Result<PerformBillPaymentMakePaymentResponseModel>> request = null;
        
        if (this.BillDetails != null){
            request = PerformBillPaymentMakePostPaymentRequest.Create(DateTime.Now,
                                                                      this.ProductDetails.ContractId,
                                                                      this.ProductDetails.ProductId,
                                                                      this.ProductDetails.OperatorId,
                                                                      this.BillDetails.AccountNumber,
                                                                      this.BillDetails.AccountName,
                                                                      this.CustomerMobileNumber,
                                                                      this.PaymentAmount);


        }
        else if (this.MeterDetails != null){
            request = PerformBillPaymentMakePrePaymentRequest.Create(DateTime.Now,
                                                                                                             this.ProductDetails.ContractId,
                                                                                                             this.ProductDetails.ProductId,
                                                                                                             this.ProductDetails.OperatorId,
                                                                                                             this.MeterDetails.MeterNumber,
                                                                                                             this.MeterDetails.CustomerName,
                                                                                                             this.PaymentAmount);
        }

        

        Result<PerformBillPaymentMakePaymentResponseModel> result = await this.Mediator.Send(request);

        if (result.IsSuccess && result.Data.IsSuccessful) {
            await this.NavigationService.GoToBillPaymentSuccessPage();
        }
        else {
            await this.NavigationService.GoToBillPaymentFailedPage();
        }
    }
    
    private Decimal paymentAmount;
    public Decimal PaymentAmount
    {
        get => this.paymentAmount;
        set => this.SetProperty(ref this.paymentAmount, value);
    }

    private String customerMobileNumber;
    public String CustomerMobileNumber {
        get => this.customerMobileNumber;
        set => this.SetProperty(ref this.customerMobileNumber, value);
    }

    public BillDetails BillDetails
    {
        get => this.billdetails;
        set => this.SetProperty(ref this.billdetails, value);
    }

    public MeterDetails MeterDetails
    {
        get => this.meterdetails;
        set => this.SetProperty(ref this.meterdetails, value);
    }
    
    private Boolean isPostPayVisible;
    public Boolean IsPostPayVisible
    {
        get => this.isPostPayVisible;
        set => this.SetProperty(ref this.isPostPayVisible, value);
    }

    private Boolean isPrePayVisible;
    public Boolean IsPrePayVisible
    {
        get => this.isPrePayVisible;
        set => this.SetProperty(ref this.isPrePayVisible, value);
    }
}