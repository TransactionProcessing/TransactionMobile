﻿using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class ProfileAccountInfoPage : BasePage2
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

    public ProfileAccountInfoPage(TestingContext testingContext) : base(testingContext){
        this.MerchantNameLabel = "MerchantNameLabel";
        this.BalanceLabel = "BalanceLabel";
        this.AvailableBalanceLabel = "AvailableBalanceLabel";
        this.LastStatementDateLabel = "LastStatementDateLabel";
        this.NextStatementDateLabel = "NextStatementDateLabel";
        this.SettlementScheduleLabel = "SettlementScheduleLabel";
    }

    #endregion

    #region Properties

    protected override String Trait => AppiumDriverWrapper.MobileTestPlatform switch
    {
        MobileTestPlatform.iOS => "My Details",
        _ => "MyDetails"
    };

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