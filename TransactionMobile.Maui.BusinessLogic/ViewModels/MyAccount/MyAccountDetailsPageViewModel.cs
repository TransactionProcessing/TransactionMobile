namespace TransactionMobile.Maui.BusinessLogic.ViewModels.MyAccount;

using Maui.UIServices;
using MediatR;
using MvvmHelpers;
using Services;
using TransactionMobile.Maui.BusinessLogic.Models;

public class MyAccountDetailsPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;

    private readonly IApplicationCache ApplicationCache;

    private readonly IMediator Mediator;

    #region Constructors

    public MyAccountDetailsPageViewModel(INavigationService navigationService, IApplicationCache applicationCache,
                                         IMediator mediator)
    {
        this.NavigationService = navigationService;
        this.ApplicationCache = applicationCache;
        this.Mediator = mediator;
        this.Title = "My Details";
    }


    #endregion

    #region Properties

    #endregion

    public async Task Initialise(CancellationToken none)
    {
        MerchantDetailsModel merchantDetails = this.ApplicationCache.GetMerchantDetails();

        this.Balance = merchantDetails.Balance;
        this.AvailableBalance = merchantDetails.AvailableBalance;
        this.NextStatementDate = merchantDetails.NextStatementDate;
        this.LastStatementDate = merchantDetails.LastStatementDate;
        this.MerchantName = merchantDetails.MerchantName;
        this.SettlementSchedule = merchantDetails.SettlementSchedule;
    }

    private Decimal balance;
    private Decimal availableBalance;
    private String merchantName;
    private DateTime nextStatementDate;
    private DateTime lastStatementDate;
    private String settlementSchedule;

    public Decimal Balance {
        get => this.balance;
        set => this.SetProperty(ref this.balance, value);
    }
    public Decimal AvailableBalance
    {
        get => this.availableBalance;
        set => this.SetProperty(ref this.availableBalance, value);
    }
    public DateTime NextStatementDate
    {
        get => this.nextStatementDate;
        set => this.SetProperty(ref this.nextStatementDate, value);
    }
    public DateTime LastStatementDate
    {
        get => this.lastStatementDate;
        set => this.SetProperty(ref this.lastStatementDate, value);
    }
    public String SettlementSchedule
    {
        get => this.settlementSchedule;
        set => this.SetProperty(ref this.settlementSchedule, value);
    }

    public String MerchantName
    {
        get => this.merchantName;
        set => this.SetProperty(ref this.merchantName, value);
    }
}