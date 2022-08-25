namespace TransactionMobile.Maui.UITests.Steps;

using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using TechTalk.SpecFlow;
using TransactionMobile.Maui.UiTests.Drivers;
using UiTests.Pages;

[Binding]
[Scope(Tag = "profile")]
public class ProfileSteps
{
    ProfilePage profilePage = new ProfilePage();

    ProfileAddressesPage profileAddressesPage = new ProfileAddressesPage();

    ProfileContactsPage profileContactsPage = new ProfileContactsPage();
    ProfileAccountInfoPage profileAccountInfoPage = new ProfileAccountInfoPage();

    [Then(@"the My Profile Page is displayed")]
    public async Task ThenTheMyProfilePageIsDisplayed()
    {
        await this.profilePage.AssertOnPage();
    }

    [When(@"I tap on Logout")]
    public async Task WhenITapOnLogout()
    {
        await this.profilePage.ClickLogoutButton();
    }

    [When(@"I tap on the Addresses button")]
    public async Task WhenITapOnTheAddressesButton() {
        await this.profilePage.ClickAddressesButton();
    }

    [Then(@"the Address List Page is displayed")]
    public async Task ThenTheAddressListPageIsDisplayed()
    {
        await this.profileAddressesPage.AssertOnPage();
    }

    [Then(@"the Primary Address is displayed")]
    public async Task ThenThePrimaryAddressIsDisplayed(Table table)
    {
        await this.profileAddressesPage.IsPrimaryAddressShown();

        String addressLine1 = await this.profileAddressesPage.GetAddressLineValue(1);
        String addressLine2 = await this.profileAddressesPage.GetAddressLineValue(2);
        String addressLine3 = await this.profileAddressesPage.GetAddressLineValue(3);
        String addressLine4 = await this.profileAddressesPage.GetAddressLineValue(4);
        String addressTown = await this.profileAddressesPage.GetAddressTownValue();
        String addressRegion = await this.profileAddressesPage.GetAddressRegionValue();
        String addressPostalCode = await this.profileAddressesPage.GetAddressPostalCodeValue();

        String? expectedAddressLine1 = table.Rows.Single()["AddressLine1"];
        String? expectedAddressLine2 = table.Rows.Single()["AddressLine2"];
        String? expectedAddressLine3 = table.Rows.Single()["AddressLine3"];
        String? expectedAddressLine4 = table.Rows.Single()["AddressLine4"];
        String? expectedAddressTown = table.Rows.Single()["AddressTown"];
        String? expectedAddressRegion = table.Rows.Single()["AddressRegion"];
        String? expectedAddressPostCode = table.Rows.Single()["AddressPostCode"];

        addressLine1.ShouldBe(expectedAddressLine1);
        addressLine2.ShouldBe(expectedAddressLine2);
        addressLine3.ShouldBe(expectedAddressLine3);
        addressLine4.ShouldBe(expectedAddressLine4);
        addressTown.ShouldBe(expectedAddressTown);
        addressRegion.ShouldBe(expectedAddressRegion);
        addressPostalCode.ShouldBe(expectedAddressPostCode);
    }
    
    [When(@"I tap on the Contacts button")]
    public async Task WhenITapOnTheContactsButton() {
        await this.profilePage.ClickContactsButton();
    }

    [Then(@"the Contact List Page is displayed")]
    public async Task ThenTheContactListPageIsDisplayed()
    {
        await this.profileContactsPage.AssertOnPage(); 
    }
    
    [Then(@"the Primary Contact is displayed")]
    public async Task ThenThePrimaryContactIsDisplayed(Table table)
    {
        await this.profileContactsPage.IsPrimaryContactShown();

        String contactName = await this.profileContactsPage.GetContactNameValue();
        String contactEmailAddress = await this.profileContactsPage.GetContactEmailAddressValue();
        String contactMobileNumber = await this.profileContactsPage.GetContactMobileNumberValue();
        
        String? expectedContactName = table.Rows.Single()["Name"];
        String? expectedContactEmailAddress = table.Rows.Single()["EmailAddress"];
        String? expectedContactMobileNumber = table.Rows.Single()["MobileNumber"];

        contactName.ShouldBe(expectedContactName);
        contactEmailAddress.ShouldBe(expectedContactEmailAddress);
        contactMobileNumber.ShouldBe(expectedContactMobileNumber);
    }

    [When(@"I tap on the Account Info button")]
    public async Task WhenITapOnTheAccountInfoButton() {
        await this.profilePage.ClickAccountInfoButton();
    }

    [Then(@"the Account Info Page is displayed")]
    public async Task ThenTheAccountInfoPageIsDisplayed()
    {
        await this.profileAccountInfoPage.AssertOnPage();
    }

    [Then(@"the Account Info is displayed")]
    public async Task ThenTheAccountInfoIsDisplayed(Table table)
    {
        String merchantName = await this.profileAccountInfoPage.GetMerchantNameValue();
        String balance = await this.profileAccountInfoPage.GetBalanceValue();
        String availableBalance = await this.profileAccountInfoPage.GetAvailableBalanceValue();
        String lastStatementDate = await this.profileAccountInfoPage.GetLastStatementDateValue();
        String nextStatementDate = await this.profileAccountInfoPage.GetNextStatementDateValue();
        String settlementSchedule = await this.profileAccountInfoPage.GetSettlementScheduleValue();

        String? expectedMerchantName = table.Rows.Single()["Name"];
        String? expectedBalance = table.Rows.Single()["Balance"];
        String? expectedAvailableBalance = table.Rows.Single()["AvailableBalance"];
        String? expectedLastStatementDate = table.Rows.Single()["LastStatementDate"];
        String? expectedNextStatementDate = table.Rows.Single()["NextStatementDate"];
        String? expectedSettlementSchedule = table.Rows.Single()["SettlementSchedule"];

        merchantName.ShouldBe(expectedMerchantName);
        balance.ShouldBe(expectedBalance);
        availableBalance.ShouldBe(expectedAvailableBalance);
        lastStatementDate.ShouldBe(expectedLastStatementDate);
        nextStatementDate.ShouldBe(expectedNextStatementDate);
        settlementSchedule.ShouldBe(expectedSettlementSchedule);
    }

}