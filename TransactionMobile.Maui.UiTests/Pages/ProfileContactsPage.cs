namespace TransactionMobile.Maui.UiTests.Pages;

using System;
using System.Threading.Tasks;
using UITests;

public class ProfileContactsPage : BasePage
{
    #region Fields

    public readonly String PrimaryContactLabel;

    private readonly String ContactEmailAddressLabel;

    private readonly String ContactMobileNumberLabel;

    private readonly String ContactNameLabel;

    #endregion

    #region Constructors

    public ProfileContactsPage() {
        this.PrimaryContactLabel = "PrimaryContactLabel";
        this.ContactNameLabel = "ContactNameLabel";
        this.ContactEmailAddressLabel = "ContactEmailAddressLabel";
        this.ContactMobileNumberLabel = "ContactMobileNumberLabel";
    }

    #endregion

    #region Properties

    protected override String Trait => "My Contacts";

    #endregion

    #region Methods

    public async Task<String> GetContactEmailAddressValue() {
        return await this.GetLabelValue(this.ContactEmailAddressLabel);
    }

    public async Task<String> GetContactMobileNumberValue() {
        return await this.GetLabelValue(this.ContactMobileNumberLabel);
    }

    public async Task<String> GetContactNameValue() {
        return await this.GetLabelValue(this.ContactNameLabel);
    }

    public async Task IsPrimaryContactShown() {
        await this.WaitForElementByAccessibilityId(this.PrimaryContactLabel);
    }

    #endregion
}