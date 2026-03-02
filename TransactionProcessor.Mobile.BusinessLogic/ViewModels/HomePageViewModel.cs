using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.Input;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels;

public partial class HomePageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private String merchantName;
    private String availableBalance;

    #endregion

    #region Constructors

    public HomePageViewModel(IApplicationCache applicationCache,
                             IDialogService dialogService,
                             IDeviceService deviceService,
                             INavigationService navigationService,
                             INavigationParameterService navigationParameterService)
        : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
    {
        this.Title = "Home";
    }

    #endregion

    #region Properties

    public String MerchantName
    {
        get => this.merchantName;
        set => this.SetProperty(ref this.merchantName, value);
    }

    public String AvailableBalance
    {
        get => this.availableBalance;
        set => this.SetProperty(ref this.availableBalance, value);
    }

    #endregion

    #region Methods

    public override async Task Initialise(CancellationToken cancellationToken)
    {
        await base.Initialise(cancellationToken);

        MerchantDetailsModel merchantDetails = this.ApplicationCache.GetMerchantDetails();
        if (merchantDetails != null)
        {
            this.MerchantName = merchantDetails.MerchantName;
            this.AvailableBalance = merchantDetails.AvailableBalance.ToString("C2");
        }
        else
        {
            this.MerchantName = String.Empty;
            this.AvailableBalance = "-";
        }
    }

    [RelayCommand]
    public async Task MobileTopup()
    {
        CorrelationIdProvider.NewId();
        await this.NavigationService.GoToMobileTopupSelectOperatorPage();
    }

    [RelayCommand]
    public async Task BillPayment()
    {
        CorrelationIdProvider.NewId();
        await this.NavigationService.GoToBillPaymentSelectOperatorPage();
    }

    [RelayCommand]
    public async Task Voucher()
    {
        CorrelationIdProvider.NewId();
        await this.NavigationService.GoToVoucherSelectOperatorPage();
    }

    [RelayCommand]
    public async Task AllTransactions()
    {
        await this.NavigationService.GoToTransactionsPage();
    }

    #endregion
}
