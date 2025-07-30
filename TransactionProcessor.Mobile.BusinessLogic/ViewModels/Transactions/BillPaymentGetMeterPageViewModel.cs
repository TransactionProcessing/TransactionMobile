using CommunityToolkit.Mvvm.Input;
using MediatR;
using MvvmHelpers.Commands;
using SimpleResults;
using System.Windows.Input;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public partial class BillPaymentGetMeterPageViewModel : ExtendedBaseViewModel
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
        this.Title = "Get Meter";
    }

    #endregion

    #region Properties

    public String MeterNumber
    {
        get => this.meterNumber;
        set => this.SetProperty(ref this.meterNumber, value);
    }
    
    public ProductDetails ProductDetails { get; set; }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken)
    {
        var query = this.NavigationParameterService.GetParameters();
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
    }

    [RelayCommand]  
    private async Task GetMeter()
    {
        Logger.LogInformation("GetMeter called");

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