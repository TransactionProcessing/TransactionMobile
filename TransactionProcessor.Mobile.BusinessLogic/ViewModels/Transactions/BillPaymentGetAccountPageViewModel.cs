﻿using System.Windows.Input;
using MediatR;
using MvvmHelpers.Commands;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public class BillPaymentGetAccountPageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private String customerAccountNumber;

    private readonly IMediator Mediator;
    private readonly INavigationParameterService NavigationParameterService;

    #endregion

    #region Constructors

    public BillPaymentGetAccountPageViewModel(INavigationService navigationService,
                                              IApplicationCache applicationCache,
                                              IDialogService dialogService,
                                              IDeviceService deviceService,
                                              IMediator mediator,
                                              INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService) {
        this.Mediator = mediator;
        this.NavigationParameterService = navigationParameterService;
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

    public async Task Initialise(CancellationToken cancellationToken) {
        var query = this.NavigationParameterService.GetParameters();
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
    }

    private async Task GetAccountCommandExecute() {
        Logger.LogInformation("GetAccountCommandExecute called");

        PerformBillPaymentGetAccountRequest request = PerformBillPaymentGetAccountRequest.Create(DateTime.Now,
                                                                                                 this.ProductDetails.ContractId,
                                                                                                 this.ProductDetails.ProductId,
                                                                                                 this.ProductDetails.OperatorId,
                                                                                                 this.CustomerAccountNumber);

        Result<PerformBillPaymentGetAccountResponseModel> result = await this.Mediator.Send(request);

        if (result.IsSuccess && result.Data.IsSuccessful) {
            ProductDetails productDetails = new()
            {
                ContractId = this.ProductDetails.ContractId,
                ProductId = this.ProductDetails.ProductId,
                OperatorId = this.ProductDetails.OperatorId
            };
            BillDetails billDetails = new()
            {
                AccountName = result.Data.BillDetails.AccountName,
                AccountNumber = result.Data.BillDetails.AccountNumber,
                Balance = result.Data.BillDetails.Balance,
                DueDate = result.Data.BillDetails.DueDate
            };

            await this.NavigationService.GoToBillPaymentPayBillPage(productDetails, billDetails);
        }
        else {
            await this.NavigationService.GoToBillPaymentFailedPage();
        }
    }

    #endregion
}

public class BillPaymentGetMeterPageViewModel : ExtendedBaseViewModel
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
                                              IMediator mediator,
                                              INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
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

    public async Task Initialise(CancellationToken cancellationToken)
    {
        var query = this.NavigationParameterService.GetParameters();
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
    }
    private async Task GetMeterCommandExecute()
    {
        Logger.LogInformation("GetMeterCommandExecute called");

        PerformBillPaymentGetMeterRequest request = PerformBillPaymentGetMeterRequest.Create(DateTime.Now,
                                                                                                 this.ProductDetails.ContractId,
                                                                                                 this.ProductDetails.ProductId,
                                                                                                 this.ProductDetails.OperatorId,
                                                                                                 this.MeterNumber);

        Result<PerformBillPaymentGetMeterResponseModel> result = await this.Mediator.Send(request);

        if (result.IsSuccess && result.Data.IsSuccessful)
        {
            ProductDetails productDetails = new ProductDetails
            {
                ContractId = this.ProductDetails.ContractId,
                ProductId = this.ProductDetails.ProductId,
                OperatorId = this.ProductDetails.OperatorId
            };

            MeterDetails meterDetails = new MeterDetails
            {
                CustomerName = result.Data.MeterDetails.CustomerName,
                MeterNumber = result.Data.MeterDetails.MeterNumber
            };

            await this.NavigationService.GoToBillPaymentPayBillPage(productDetails, meterDetails);
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

    public Guid OperatorId { get; set; }

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