namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Common;
using Logging;
using Maui.UIServices;
using MediatR;
using Models;
using MvvmHelpers.Commands;
using RequestHandlers;
using Requests;
using Services;
using SimpleResults;
using UIServices;

public class BillPaymentGetAccountPageViewModel : ExtendedBaseViewModel, IQueryAttributable
{
    #region Fields

    private String customerAccountNumber;

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public BillPaymentGetAccountPageViewModel(INavigationService navigationService,
                                              IApplicationCache applicationCache,
                                              IDialogService dialogService,
                                              IDeviceService deviceService,
                                              IMediator mediator) : base(applicationCache, dialogService, navigationService, deviceService) {
        this.Mediator = mediator;
        this.GetAccountCommand = new AsyncCommand(this.GetAccountCommandExecute);
        this.Title = "Get Customer Account";
    }

    #endregion

    #region Properties

    public String CustomerAccountNumber {
        get => this.customerAccountNumber;
        set => this.SetProperty(ref this.customerAccountNumber, value);
    }

    public ICommand GetAccountCommand { get; }

    public ProductDetails ProductDetails { get; set; }

    #endregion

    #region Methods

    public void ApplyQueryAttributes(IDictionary<String, Object> query) {
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
    }

    private async Task GetAccountCommandExecute() {
        Logger.LogInformation("GetAccountCommandExecute called");

        PerformBillPaymentGetAccountRequest request = PerformBillPaymentGetAccountRequest.Create(DateTime.Now,
                                                                                                 this.ProductDetails.ContractId,
                                                                                                 this.ProductDetails.ProductId,
                                                                                                 this.ProductDetails.OperatorIdentifier,
                                                                                                 this.CustomerAccountNumber);

        Result<PerformBillPaymentGetAccountResponseModel> result = await this.Mediator.Send(request);

        if (result.IsSuccess && result.Data.IsSuccessful) {
            ProductDetails productDetails = new ProductDetails {
                                                                   ContractId = this.ProductDetails.ContractId,
                                                                   ProductId = this.ProductDetails.ProductId,
                                                                   OperatorIdentifier = this.ProductDetails.OperatorIdentifier
                                                               };

            await this.NavigationService.GoToBillPaymentPayBillPage(productDetails, result.Data.BillDetails);
        }
        else {
            await this.NavigationService.GoToBillPaymentFailedPage();
        }
    }

    #endregion
}

public class BillPaymentGetMeterPageViewModel : ExtendedBaseViewModel, IQueryAttributable
{
    #region Fields

    private String meterNumber;

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public BillPaymentGetMeterPageViewModel(INavigationService navigationService,
                                              IApplicationCache applicationCache,
                                              IDialogService dialogService,
                                              IDeviceService deviceService,
                                              IMediator mediator) : base(applicationCache, dialogService, navigationService, deviceService)
    {
        this.Mediator = mediator;
        this.GetMeterCommand = new AsyncCommand(this.GetMeterCommandExecute);
        this.Title = "Get Meter";
    }

    #endregion

    #region Properties

    public String MeterNumber
    {
        get => this.meterNumber;
        set => this.SetProperty(ref this.meterNumber, value);
    }

    public ICommand GetMeterCommand { get; }

    public ProductDetails ProductDetails { get; set; }

    #endregion

    #region Methods

    public void ApplyQueryAttributes(IDictionary<String, Object> query)
    {
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
    }

    private async Task GetMeterCommandExecute()
    {
        Logger.LogInformation("GetMeterCommandExecute called");

        PerformBillPaymentGetMeterRequest request = PerformBillPaymentGetMeterRequest.Create(DateTime.Now,
                                                                                                 this.ProductDetails.ContractId,
                                                                                                 this.ProductDetails.ProductId,
                                                                                                 this.ProductDetails.OperatorIdentifier,
                                                                                                 this.MeterNumber);

        Result<PerformBillPaymentGetMeterResponseModel> result = await this.Mediator.Send(request);

        if (result.IsSuccess && result.Data.IsSuccessful)
        {
            ProductDetails productDetails = new ProductDetails
            {
                ContractId = this.ProductDetails.ContractId,
                ProductId = this.ProductDetails.ProductId,
                OperatorIdentifier = this.ProductDetails.OperatorIdentifier
            };

            await this.NavigationService.GoToBillPaymentPayBillPage(productDetails, result.Data.MeterDetails);
        }
        else
        {
            await this.NavigationService.GoToBillPaymentFailedPage();
        }
    }

    #endregion
}


public class BillDetails
{
    #region Properties

    public String AccountName { get; set; }

    public String AccountNumber { get; set; }

    public String Balance { get; set; }

    public String DueDate { get; set; }

    #endregion
}

public class ProductDetails
{
    #region Properties

    public Guid ContractId { get; set; }

    public String OperatorIdentifier { get; set; }

    public Guid ProductId { get; set; }

    #endregion
}

public class MeterDetails
{
    #region Properties

    public String CustomerName { get; set; }

    public String MeterNumber { get; set; }

    #endregion
}