using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class ProfileContactsPage : BasePage2
{
    #region Fields

    public readonly String PrimaryContactLabel;

    private readonly String ContactEmailAddressLabel;

    private readonly String ContactMobileNumberLabel;

    private readonly String ContactNameLabel;

    #endregion

    #region Constructors

    public ProfileContactsPage(TestingContext testingContext) : base(testingContext)
    {
        this.PrimaryContactLabel = "PrimaryContactLabel";
        this.ContactNameLabel = "ContactNameLabel";
        this.ContactEmailAddressLabel = "ContactEmailAddressLabel";
        this.ContactMobileNumberLabel = "ContactMobileNumberLabel";
    }

    #endregion

    #region Properties

    //protected override String Trait => "MyContacts";
    protected override String Trait => "MyContacts";

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