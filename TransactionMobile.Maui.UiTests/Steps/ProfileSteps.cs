namespace TransactionMobile.Maui.UITests.Steps;

using System;
using System.Linq;
using System.Threading.Tasks;
using Shared.IntegrationTesting;
using Shouldly;
using TechTalk.SpecFlow;
using TransactionMobile.Maui.UiTests.Drivers;
using UiTests.Common;
using UiTests.Pages;

[Binding]
[Scope(Tag = "profile")]
public class ProfileSteps{
    private readonly TestingContext TestingContext;

    private ProfilePage profilePage;

    private ProfileAddressesPage profileAddressesPage;

    ProfileContactsPage profileContactsPage;
    ProfileAccountInfoPage profileAccountInfoPage;

    public ProfileSteps(TestingContext testingContext){
        this.TestingContext = testingContext;
        this.profilePage = new ProfilePage(testingContext);
        this.profileAccountInfoPage = new ProfileAccountInfoPage(testingContext);
        this.profileContactsPage = new ProfileContactsPage(testingContext);
        this.profileAddressesPage = new ProfileAddressesPage(testingContext);
    }

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
    public async Task ThenThePrimaryAddressIsDisplayed(Table table){
        await this.profileAddressesPage.IsPrimaryAddressShown();

        TableRow? tableRow = table.Rows.Single();

        String expectedAddressLine1= SpecflowTableHelper.GetStringRowValue(tableRow, "AddressLine1");
        String expectedAddressLine2 = SpecflowTableHelper.GetStringRowValue(tableRow, "AddressLine2");
        String expectedAddressLine3 = SpecflowTableHelper.GetStringRowValue(tableRow, "AddressLine3");
        String expectedAddressLine4 = SpecflowTableHelper.GetStringRowValue(tableRow, "AddressLine4");
        String expectedAddressTown = SpecflowTableHelper.GetStringRowValue(tableRow, "AddressTown");
        String expectedAddressRegion = SpecflowTableHelper.GetStringRowValue(tableRow, "AddressRegion");
        String expectedAddressPostCode = SpecflowTableHelper.GetStringRowValue(tableRow, "AddressPostCode");

        if (String.IsNullOrEmpty(expectedAddressLine1) == false){
            String addressLine1 = await this.profileAddressesPage.GetAddressLineValue(1);
            addressLine1.ShouldBe(expectedAddressLine1);
        }

        if (String.IsNullOrEmpty(expectedAddressLine2) == false){
            String addressLine2 = await this.profileAddressesPage.GetAddressLineValue(2);
            addressLine2.ShouldBe(expectedAddressLine2);
        }

        if (String.IsNullOrEmpty(expectedAddressLine3) == false){
            String addressLine3 = await this.profileAddressesPage.GetAddressLineValue(3);
            addressLine3.ShouldBe(expectedAddressLine3);
        }

        if (String.IsNullOrEmpty(expectedAddressLine4) == false){
            String addressLine4 = await this.profileAddressesPage.GetAddressLineValue(4);
            addressLine4.ShouldBe(expectedAddressLine4);
        }

        if (String.IsNullOrEmpty(expectedAddressTown) == false){
            String addressTown = await this.profileAddressesPage.GetAddressTownValue();
            addressTown.ShouldBe(expectedAddressTown);
        }

        if (String.IsNullOrEmpty(expectedAddressRegion) == false){
            String addressRegion = await this.profileAddressesPage.GetAddressRegionValue();
            addressRegion.ShouldBe(expectedAddressRegion);
        }

        if (String.IsNullOrEmpty(expectedAddressPostCode) == false){
            String addressPostalCode = await this.profileAddressesPage.GetAddressPostalCodeValue();
            addressPostalCode.ShouldBe(expectedAddressPostCode);
        }
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
        
        TableRow? tableRow = table.Rows.Single();
        String? expectedContactName = SpecflowTableHelper.GetStringRowValue(tableRow, "Name");
        String? expectedContactEmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress");
        String? expectedContactMobileNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "MobileNumber");

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
        //String lastStatementDate = await this.profileAccountInfoPage.GetLastStatementDateValue();
        //String nextStatementDate = await this.profileAccountInfoPage.GetNextStatementDateValue();
        //String settlementSchedule = await this.profileAccountInfoPage.GetSettlementScheduleValue();

        TableRow? tableRow = table.Rows.Single();
        String? expectedMerchantName = SpecflowTableHelper.GetStringRowValue(tableRow,"Name");
        String? expectedBalance = SpecflowTableHelper.GetStringRowValue(tableRow, "Balance");
        String? expectedAvailableBalance = SpecflowTableHelper.GetStringRowValue(tableRow, "AvailableBalance");
        //String? expectedLastStatementDate = table.Rows.Single()["LastStatementDate"];
        //String? expectedNextStatementDate = table.Rows.Single()["NextStatementDate"];
        //String? expectedSettlementSchedule = table.Rows.Single()["SettlementSchedule"];

        merchantName.ShouldBe(expectedMerchantName);
        balance.ShouldBe(expectedBalance);
        availableBalance.ShouldBe(expectedAvailableBalance);
        //lastStatementDate.ShouldBe(expectedLastStatementDate);
        //nextStatementDate.ShouldBe(expectedNextStatementDate);
        //settlementSchedule.ShouldBe(expectedSettlementSchedule);
    }

}