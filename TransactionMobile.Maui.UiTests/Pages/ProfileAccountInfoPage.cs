namespace TransactionMobile.Maui.UiTests.Pages;

using System;
using System.Threading.Tasks;
using UITests;

public class ProfileAccountInfoPage : BasePage
{
    #region Fields

    public readonly String AvailableBalanceLabel;

    public readonly String BalanceLabel;

    public readonly String LastStatementDateLabel;

    public readonly String MerchantNameLabel;

    public readonly String NextStatementDateLabel;

    public readonly String SettlementScheduleLabel;

    #endregion

    #region Constructors

    public ProfileAccountInfoPage() {
        this.MerchantNameLabel = "MerchantNameLabel";
        this.BalanceLabel = "BalanceLabel";
        this.AvailableBalanceLabel = "AvailableBalanceLabel";
        this.LastStatementDateLabel = "LastStatementDateLabel";
        this.NextStatementDateLabel = "NextStatementDateLabel";
        this.SettlementScheduleLabel = "SettlementScheduleLabel";
    }

    #endregion

    #region Properties

    protected override String Trait => "My Details";

    #endregion

    #region Methods

    public async Task<String> GetAvailableBalanceValue() {
        return await this.GetLabelValue(this.AvailableBalanceLabel);
    }

    public async Task<String> GetBalanceValue() {
        return await this.GetLabelValue(this.BalanceLabel);
    }

    public async Task<String> GetLastStatementDateValue() {
        return await this.GetLabelValue(this.LastStatementDateLabel);
    }

    public async Task<String> GetMerchantNameValue() {
        return await this.GetLabelValue(this.MerchantNameLabel);
    }

    public async Task<String> GetNextStatementDateValue() {
        return await this.GetLabelValue(this.NextStatementDateLabel);
    }

    public async Task<String> GetSettlementScheduleValue() {
        return await this.GetLabelValue(this.SettlementScheduleLabel);
    }

    #endregion
}