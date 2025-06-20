using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;

public class MyAccountDetailsPageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private Decimal availableBalance;

    private Decimal balance;

    private DateTime lastStatementDate;

    private String merchantName;

    private DateTime nextStatementDate;

    private String settlementSchedule;

    #endregion

    #region Constructors

    public MyAccountDetailsPageViewModel(INavigationService navigationService,
                                         IApplicationCache applicationCache,
                                         IDialogService dialogService,
                                         IDeviceService deviceService,
                                         INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService) {
        this.Title = "My Details";
    }

    #endregion

    #region Properties

    public Decimal AvailableBalance {
        get => this.availableBalance;
        set => this.SetProperty(ref this.availableBalance, value);
    }

    public Decimal Balance {
        get => this.balance;
        set => this.SetProperty(ref this.balance, value);
    }

    public DateTime LastStatementDate {
        get => this.lastStatementDate;
        set => this.SetProperty(ref this.lastStatementDate, value);
    }

    public String MerchantName {
        get => this.merchantName;
        set => this.SetProperty(ref this.merchantName, value);
    }

    public DateTime NextStatementDate {
        get => this.nextStatementDate;
        set => this.SetProperty(ref this.nextStatementDate, value);
    }

    public String SettlementSchedule {
        get => this.settlementSchedule;
        set => this.SetProperty(ref this.settlementSchedule, value);
    }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken none) {
        MerchantDetailsModel merchantDetails = this.ApplicationCache.GetMerchantDetails();

        this.Balance = merchantDetails.Balance;
        this.AvailableBalance = merchantDetails.AvailableBalance;
        this.NextStatementDate = merchantDetails.NextStatementDate;
        this.LastStatementDate = merchantDetails.LastStatementDate;
        this.MerchantName = merchantDetails.MerchantName;
        this.SettlementSchedule = merchantDetails.SettlementSchedule;
    }

    #endregion
}