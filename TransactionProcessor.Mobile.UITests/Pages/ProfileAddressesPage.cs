﻿using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class ProfileAddressesPage : BasePage2
{
    #region Fields

    public readonly String PrimaryAddressLabel;

    private readonly String AddressLine1Label;

    private readonly String AddressLine2Label;

    private readonly String AddressLine3Label;

    private readonly String AddressLine4Label;

    private readonly String AddressPostalCodeLabel;

    private readonly String AddressRegionLabel;

    private readonly String AddressTownLabel;

    #endregion

    #region Constructors

    public ProfileAddressesPage(TestingContext testingContext) : base(testingContext)
    {
        this.PrimaryAddressLabel = "PrimaryAddressLabel";
        this.AddressLine1Label = "AddressLine1Label";
        this.AddressLine2Label = "AddressLine2Label";
        this.AddressLine3Label = "AddressLine3Label";
        this.AddressLine4Label = "AddressLine4Label";
        this.AddressRegionLabel = "AddressRegionLabel";
        this.AddressTownLabel = "AddressTownLabel";
        this.AddressPostalCodeLabel = "AddressPostCodeLabel";
    }

    #endregion

    #region Properties

    protected override String Trait => AppiumDriverWrapper.MobileTestPlatform switch
    {
        MobileTestPlatform.iOS => "My Addresses",
        _ => "MyAddresses"
    };

    #endregion

    #region Methods

    public async Task<String> GetAddressLineValue(Int32 lineNumber) {
        return lineNumber switch {
            1 => await this.GetLabelValue(this.AddressLine1Label),
            2 => await this.GetLabelValue(this.AddressLine2Label),
            3 => await this.GetLabelValue(this.AddressLine3Label),
            4 => await this.GetLabelValue(this.AddressLine4Label)
        };
    }

    public async Task<String> GetAddressPostalCodeValue() {
        return await this.GetLabelValue(this.AddressPostalCodeLabel);
    }

    public async Task<String> GetAddressRegionValue() {
        return await this.GetLabelValue(this.AddressRegionLabel);
    }

    public async Task<String> GetAddressTownValue() {
        return await this.GetLabelValue(this.AddressTownLabel);
    }

    public async Task IsPrimaryAddressShown() {
        await this.WaitForElementByAccessibilityId(this.PrimaryAddressLabel);
    }

    #endregion
}