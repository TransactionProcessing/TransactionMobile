﻿namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Maui.UIServices;
using MediatR;
using MvvmHelpers.Commands;
using Requests;
using Services;
using UIServices;

public class BillPaymentPayBillPageViewModel : ExtendedBaseViewModel, IQueryAttributable
{
    #region Fields
    
    private readonly IMediator Mediator;

    private BillDetails billdetails;

    #endregion

    #region Constructors

    public Action OnCustomerMobileNumberEntryCompleted { get; set; }

    public Action OnPaymentAmountEntryCompleted { get; set; }

    public void ApplyQueryAttributes(IDictionary<string, Object> query)
    {
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
        this.BillDetails = query["BillDetails"] as BillDetails;
    }

    public ProductDetails ProductDetails { get; set; }

    public BillPaymentPayBillPageViewModel(INavigationService navigationService,
                                           IApplicationCache applicationCache,
                                           IDialogService dialogService,
                                           IMediator mediator) : base(applicationCache, dialogService, navigationService)
    {
        this.Mediator = mediator;
        this.MakeBillPaymentCommand = new AsyncCommand(this.MakeBillPaymentCommandExecute);
        this.CustomerMobileNumberEntryCompletedCommand = new AsyncCommand(this.CustomerMobileNumberEntryCompletedExecute);
        this.PaymentAmountEntryCompletedCommand = new AsyncCommand(this.PaymentAmountEntryCompletedCommandExecute);
        this.Title = "Make Bill Payment";
    }

    private async Task PaymentAmountEntryCompletedCommandExecute() {
        Shared.Logger.Logger.LogInformation("PaymentAmountEntryCompletedCommandExecute called");
        this.OnPaymentAmountEntryCompleted();
    }

    private async Task CustomerMobileNumberEntryCompletedExecute() {
        Shared.Logger.Logger.LogInformation("CustomerMobileNumberEntryCompletedExecute called");
        this.OnCustomerMobileNumberEntryCompleted();
    }

    #endregion

    private async Task MakeBillPaymentCommandExecute() {
        Shared.Logger.Logger.LogInformation("MakeBillPaymentCommandExecute called");

        PerformBillPaymentMakePaymentRequest request = PerformBillPaymentMakePaymentRequest.Create(DateTime.Now,
                                                                                                   this.ProductDetails.ContractId,
                                                                                                   this.ProductDetails.ProductId,
                                                                                                   this.ProductDetails.OperatorIdentifier,
                                                                                                   this.BillDetails.AccountNumber,
                                                                                                   this.BillDetails.AccountName,
                                                                                                   this.CustomerMobileNumber,
                                                                                                   this.PaymentAmount);

        Boolean response = await this.Mediator.Send(request);

        if (response) {
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

    public ICommand MakeBillPaymentCommand { get; }
}