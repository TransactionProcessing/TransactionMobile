﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace TransactionMobile.Maui.UiTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Login")]
    [NUnit.Framework.CategoryAttribute("background")]
    [NUnit.Framework.CategoryAttribute("login")]
    [NUnit.Framework.CategoryAttribute("toolbar")]
    [NUnit.Framework.CategoryAttribute("profile")]
    [NUnit.Framework.CategoryAttribute("base")]
    [NUnit.Framework.CategoryAttribute("shared")]
    public partial class LoginFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "background",
                "login",
                "toolbar",
                "profile",
                "base",
                "shared"};
        
#line 1 "Login.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Login", null, ProgrammingLanguage.CSharp, featureTags);
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 4
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "RoleName"});
            table1.AddRow(new string[] {
                        "Merchant"});
#line 6
 testRunner.Given("the following security roles exist", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Description"});
            table2.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST Scope",
                        "A scope for Estate Managememt REST"});
            table2.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST  Scope",
                        "A scope for Transaction Processor REST"});
            table2.AddRow(new string[] {
                        "transactionProcessorACL",
                        "Transaction Processor ACL REST  Scope",
                        "A scope for Transaction Processor ACL REST"});
            table2.AddRow(new string[] {
                        "voucherManagement",
                        "Voucher Management REST  Scope",
                        "A scope for Voucher Management REST"});
#line 10
 testRunner.Given("I create the following api scopes", ((string)(null)), table2, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table3.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST",
                        "Secret1",
                        "estateManagement",
                        "merchantId, estateId, role"});
            table3.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST",
                        "Secret1",
                        "transactionProcessor",
                        ""});
            table3.AddRow(new string[] {
                        "transactionProcessorACL",
                        "Transaction Processor ACL REST",
                        "Secret1",
                        "transactionProcessorACL",
                        "merchantId, estateId, role"});
            table3.AddRow(new string[] {
                        "voucherManagement",
                        "Voucher Management REST",
                        "Secret1",
                        "voucherManagement",
                        ""});
#line 17
 testRunner.Given("the following api resources exist", ((string)(null)), table3, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId",
                        "ClientName",
                        "Secret",
                        "AllowedScopes",
                        "AllowedGrantTypes"});
            table4.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "estateManagement,transactionProcessor,transactionProcessorACL,voucherManagement",
                        "client_credentials"});
            table4.AddRow(new string[] {
                        "mobileAppClient",
                        "Mobile App Client",
                        "Secret1",
                        "transactionProcessorACL",
                        "password"});
#line 24
 testRunner.Given("the following clients exist", ((string)(null)), table4, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId"});
            table5.AddRow(new string[] {
                        "serviceClient"});
#line 29
 testRunner.Given("I have a token to access the estate management and transaction processor acl reso" +
                    "urces", ((string)(null)), table5, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName"});
            table6.AddRow(new string[] {
                        "Test Estate 1"});
#line 33
 testRunner.Given("I have created the following estates", ((string)(null)), table6, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "RequireCustomMerchantNumber",
                        "RequireCustomTerminalNumber"});
            table7.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "True",
                        "True"});
            table7.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "True",
                        "True"});
#line 37
 testRunner.Given("I have created the following operators", ((string)(null)), table7, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription"});
            table8.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract"});
            table8.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "Hospital 1 Contract"});
#line 42
 testRunner.Given("I create a contract with the following values", ((string)(null)), table8, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "DisplayText",
                        "Value"});
            table9.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Custom",
                        ""});
            table9.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "Hospital 1 Contract",
                        "10 KES",
                        "10 KES",
                        ""});
#line 47
 testRunner.When("I create the following Products", ((string)(null)), table9, "When ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "CalculationType",
                        "FeeDescription",
                        "Value"});
            table10.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Fixed",
                        "Merchant Commission",
                        "2.50"});
#line 52
 testRunner.When("I add the following Transaction Fees", ((string)(null)), table10, "When ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "MerchantName",
                        "AddressLine1",
                        "Town",
                        "Region",
                        "Country",
                        "ContactName",
                        "EmailAddress",
                        "EstateName"});
            table11.AddRow(new string[] {
                        "Test Merchant 1",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant1.co.uk",
                        "Test Estate 1"});
#line 56
 testRunner.Given("I create the following merchants", ((string)(null)), table11, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "OperatorName",
                        "MerchantName",
                        "MerchantNumber",
                        "TerminalNumber",
                        "EstateName"});
            table12.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate 1"});
            table12.AddRow(new string[] {
                        "Voucher",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate 1"});
#line 60
 testRunner.Given("I have assigned the following  operator to the merchants", ((string)(null)), table12, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "MerchantName",
                        "EstateName"});
            table13.AddRow(new string[] {
                        "Test Merchant 1",
                        "Test Estate 1"});
#line 65
 testRunner.Given("I have assigned the following devices to the merchants", ((string)(null)), table13, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "EmailAddress",
                        "Password",
                        "GivenName",
                        "FamilyName",
                        "EstateName",
                        "MerchantName"});
            table14.AddRow(new string[] {
                        "merchantuser@testmerchant1.co.uk",
                        "123456",
                        "TestMerchant",
                        "User1",
                        "Test Estate 1",
                        "Test Merchant 1"});
#line 69
 testRunner.Given("I have created the following security users", ((string)(null)), table14, "Given ");
#line hidden
#line 73
 testRunner.Given("I have created a config for my application", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 75
 testRunner.Given("I have created a config for my device", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Logout")]
        [NUnit.Framework.CategoryAttribute("PRTest")]
        public void Logout()
        {
            string[] tagsOfScenario = new string[] {
                    "PRTest"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Logout", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 86
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
this.FeatureBackground();
#line hidden
#line 87
 testRunner.Given("I am on the Login Screen", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 88
 testRunner.And("my device is registered", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 89
 testRunner.When("I enter \'merchantuser@testmerchant1.co.uk\' as the Email Address", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 90
 testRunner.And("I enter \'123456\' as the Password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 91
 testRunner.And("I tap on Login", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 92
 testRunner.Then("the Merchant Home Page is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
