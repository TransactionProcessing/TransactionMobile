namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Common;
using Logging;
using Maui.UIServices;
using MediatR;
using Microsoft.Maui.Controls;
using Models;
using MvvmHelpers.Commands;
using RequestHandlers;
using Requests;
using Services;
using SimpleResults;
using TransactionProcessorACL.DataTransferObjects.Responses;
using UIServices;

public class BillPaymentPayBillPageViewModel : ExtendedBaseViewModel, IQueryAttributable
{
    #region Fields
    
    private readonly IMediator Mediator;

    private BillDetails billdetails;
    private MeterDetails meterdetails;

    #endregion

    #region Constructors

    public Action OnCustomerMobileNumberEntryCompleted { get; set; }

    public Action OnPaymentAmountEntryCompleted { get; set; }

    public void ApplyQueryAttributes(IDictionary<string, Object> query){
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
                                           IDeviceService deviceService) : base(applicationCache, dialogService, navigationService, deviceService)
    {
        this.Mediator = mediator;
        this.MakeBillPaymentCommand = new AsyncCommand(this.MakeBillPaymentCommandExecute);
        this.CustomerMobileNumberEntryCompletedCommand = new AsyncCommand(this.CustomerMobileNumberEntryCompletedExecute);
        this.PaymentAmountEntryCompletedCommand = new AsyncCommand(this.PaymentAmountEntryCompletedCommandExecute);
        this.Title = "Make Bill Payment";
    }
    private async Task PaymentAmountEntryCompletedCommandExecute() {
        Logger.LogInformation("PaymentAmountEntryCompletedCommandExecute called");
        this.OnPaymentAmountEntryCompleted();
    }

    private async Task CustomerMobileNumberEntryCompletedExecute() {
        Logger.LogInformation("CustomerMobileNumberEntryCompletedExecute called");
        this.OnCustomerMobileNumberEntryCompleted();
    }

    #endregion

    private async Task MakeBillPaymentCommandExecute() {
        Logger.LogInformation("MakeBillPaymentCommandExecute called");
        IRequest<Result<PerformBillPaymentMakePaymentResponseModel>> request = null;
        
        if (this.BillDetails != null){
            request = PerformBillPaymentMakePostPaymentRequest.Create(DateTime.Now,
                                                                      this.ProductDetails.ContractId,
                                                                      this.ProductDetails.ProductId,
                                                                      this.ProductDetails.OperatorIdentifier,
                                                                      this.BillDetails.AccountNumber,
                                                                      this.BillDetails.AccountName,
                                                                      this.CustomerMobileNumber,
                                                                      this.PaymentAmount);


        }
        else if (this.MeterDetails != null){
            request = PerformBillPaymentMakePrePaymentRequest.Create(DateTime.Now,
                                                                                                             this.ProductDetails.ContractId,
                                                                                                             this.ProductDetails.ProductId,
                                                                                                             this.ProductDetails.OperatorIdentifier,
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

    public ICommand CustomerMobileNumberEntryCompletedCommand { get; }

    public ICommand PaymentAmountEntryCompletedCommand { get; }
    
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

    public ICommand MakeBillPaymentCommand { get; }

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