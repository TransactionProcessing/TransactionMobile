﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:2.0.0.0
//      Reqnroll Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
using Reqnroll;
namespace TransactionProcessor.Mobile.UITests.Features
{
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("EndToEndTests")]
    [NUnit.Framework.FixtureLifeCycleAttribute(NUnit.Framework.LifeCycle.InstancePerTestCase)]
    [NUnit.Framework.CategoryAttribute("background")]
    [NUnit.Framework.CategoryAttribute("login")]
    [NUnit.Framework.CategoryAttribute("toolbar")]
    [NUnit.Framework.CategoryAttribute("profile")]
    [NUnit.Framework.CategoryAttribute("base")]
    [NUnit.Framework.CategoryAttribute("sharedapp")]
    [NUnit.Framework.CategoryAttribute("shared")]
    [NUnit.Framework.CategoryAttribute("transactions")]
    public partial class EndToEndTestsFeature
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "background",
                "login",
                "toolbar",
                "profile",
                "base",
                "sharedapp",
                "shared",
                "transactions"};
        
        private static global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new global::System.Globalization.CultureInfo("en-US"), "Features", "EndToEndTests", null, global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
        
#line 1 "EndToEndTests.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public static async global::System.Threading.Tasks.Task FeatureSetupAsync()
        {
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public static async global::System.Threading.Tasks.Task FeatureTearDownAsync()
        {
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public async global::System.Threading.Tasks.Task TestInitializeAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(featureHint: featureInfo);
            try
            {
                if (((testRunner.FeatureContext != null) 
                            && (testRunner.FeatureContext.FeatureInfo.Equals(featureInfo) == false)))
                {
                    await testRunner.OnFeatureEndAsync();
                }
            }
            finally
            {
                if (((testRunner.FeatureContext != null) 
                            && testRunner.FeatureContext.BeforeFeatureHookFailed))
                {
                    throw new global::Reqnroll.ReqnrollException("Scenario skipped because of previous before feature hook error");
                }
                if ((testRunner.FeatureContext == null))
                {
                    await testRunner.OnFeatureStartAsync(featureInfo);
                }
            }
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public async global::System.Threading.Tasks.Task TestTearDownAsync()
        {
            if ((testRunner == null))
            {
                return;
            }
            try
            {
                await testRunner.OnScenarioEndAsync();
            }
            finally
            {
                global::Reqnroll.TestRunnerManager.ReleaseTestRunner(testRunner);
                testRunner = null;
            }
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public async global::System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async global::System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        public virtual async global::System.Threading.Tasks.Task FeatureBackgroundAsync()
        {
#line 4
#line hidden
            global::Reqnroll.Table table1 = new global::Reqnroll.Table(new string[] {
                        "AccountNumber",
                        "AccountName",
                        "DueDate",
                        "Amount"});
            table1.AddRow(new string[] {
                        "12345678",
                        "Test Account 1",
                        "Today",
                        "100.00"});
#line 6
 await testRunner.GivenAsync("the following bills are available at the PataPawa PostPaid Host", ((string)(null)), table1, "Given ");
#line hidden
            global::Reqnroll.Table table2 = new global::Reqnroll.Table(new string[] {
                        "Username",
                        "Password"});
            table2.AddRow(new string[] {
                        "operatora",
                        "1234567898"});
#line 10
 await testRunner.GivenAsync("the following users are available at the PataPawa PrePay Host", ((string)(null)), table2, "Given ");
#line hidden
            global::Reqnroll.Table table3 = new global::Reqnroll.Table(new string[] {
                        "MeterNumber",
                        "CustomerName"});
            table3.AddRow(new string[] {
                        "00000001",
                        "Customer 1"});
            table3.AddRow(new string[] {
                        "00000002",
                        "Customer 2"});
            table3.AddRow(new string[] {
                        "00000003",
                        "Customer 3"});
#line 14
 await testRunner.GivenAsync("the following meters are available at the PataPawa PrePay Host", ((string)(null)), table3, "Given ");
#line hidden
            global::Reqnroll.Table table4 = new global::Reqnroll.Table(new string[] {
                        "Role Name"});
            table4.AddRow(new string[] {
                        "Merchant"});
#line 20
 await testRunner.GivenAsync("the following security roles exist", ((string)(null)), table4, "Given ");
#line hidden
            global::Reqnroll.Table table5 = new global::Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Description"});
            table5.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST  Scope",
                        "A scope for Transaction Processor REST"});
            table5.AddRow(new string[] {
                        "transactionProcessorACL",
                        "Transaction Processor ACL REST  Scope",
                        "A scope for Transaction Processor ACL REST"});
            table5.AddRow(new string[] {
                        "voucherManagement",
                        "Voucher Management REST  Scope",
                        "A scope for Voucher Management REST"});
#line 24
 await testRunner.GivenAsync("I create the following api scopes", ((string)(null)), table5, "Given ");
#line hidden
            global::Reqnroll.Table table6 = new global::Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table6.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST",
                        "Secret1",
                        "transactionProcessor",
                        "merchantId, estateId, role"});
            table6.AddRow(new string[] {
                        "transactionProcessorACL",
                        "Transaction Processor ACL REST",
                        "Secret1",
                        "transactionProcessorACL",
                        "merchantId, estateId, role"});
            table6.AddRow(new string[] {
                        "voucherManagement",
                        "Voucher Management REST",
                        "Secret1",
                        "voucherManagement",
                        ""});
#line 30
 await testRunner.GivenAsync("the following api resources exist", ((string)(null)), table6, "Given ");
#line hidden
            global::Reqnroll.Table table7 = new global::Reqnroll.Table(new string[] {
                        "ClientId",
                        "ClientName",
                        "Secret",
                        "Scopes",
                        "GrantTypes"});
            table7.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "transactionProcessor,transactionProcessorACL,voucherManagement",
                        "client_credentials"});
            table7.AddRow(new string[] {
                        "mobileAppClient",
                        "Mobile App Client",
                        "Secret1",
                        "transactionProcessorACL,transactionProcessor",
                        "password"});
#line 36
 await testRunner.GivenAsync("the following clients exist", ((string)(null)), table7, "Given ");
#line hidden
            global::Reqnroll.Table table8 = new global::Reqnroll.Table(new string[] {
                        "ClientId"});
            table8.AddRow(new string[] {
                        "serviceClient"});
#line 41
 await testRunner.GivenAsync("I have a token to access the estate management and transaction processor acl reso" +
                    "urces", ((string)(null)), table8, "Given ");
#line hidden
            global::Reqnroll.Table table9 = new global::Reqnroll.Table(new string[] {
                        "EstateName"});
            table9.AddRow(new string[] {
                        "Test Estate 1"});
#line 45
 await testRunner.GivenAsync("I have created the following estates", ((string)(null)), table9, "Given ");
#line hidden
            global::Reqnroll.Table table10 = new global::Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "RequireCustomMerchantNumber",
                        "RequireCustomTerminalNumber"});
            table10.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "True",
                        "True"});
            table10.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "True",
                        "True"});
            table10.AddRow(new string[] {
                        "Test Estate 1",
                        "PataPawa PostPay",
                        "True",
                        "True"});
            table10.AddRow(new string[] {
                        "Test Estate 1",
                        "PataPawa PrePay",
                        "True",
                        "True"});
#line 49
 await testRunner.GivenAsync("I have created the following operators", ((string)(null)), table10, "Given ");
#line hidden
            global::Reqnroll.Table table11 = new global::Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName"});
            table11.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom"});
            table11.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher"});
            table11.AddRow(new string[] {
                        "Test Estate 1",
                        "PataPawa PostPay"});
            table11.AddRow(new string[] {
                        "Test Estate 1",
                        "PataPawa PrePay"});
#line 56
 await testRunner.AndAsync("I have assigned the following operators to the estates", ((string)(null)), table11, "And ");
#line hidden
            global::Reqnroll.Table table12 = new global::Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription"});
            table12.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract"});
            table12.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "Hospital 1 Contract"});
            table12.AddRow(new string[] {
                        "Test Estate 1",
                        "PataPawa PostPay",
                        "PataPawa PostPay Contract"});
            table12.AddRow(new string[] {
                        "Test Estate 1",
                        "PataPawa PrePay",
                        "PataPawa PrePay Contract"});
#line 64
 await testRunner.GivenAsync("I create a contract with the following values", ((string)(null)), table12, "Given ");
#line hidden
            global::Reqnroll.Table table13 = new global::Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "DisplayText",
                        "Value",
                        "ProductType"});
            table13.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Custom",
                        "",
                        "MobileTopup"});
            table13.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "Hospital 1 Contract",
                        "10 KES",
                        "10 KES",
                        "10.00",
                        "Voucher"});
            table13.AddRow(new string[] {
                        "Test Estate 1",
                        "PataPawa PostPay",
                        "PataPawa PostPay Contract",
                        "Post Pay Bill Pay",
                        "Bill Pay (Post)",
                        "",
                        "BillPayment"});
            table13.AddRow(new string[] {
                        "Test Estate 1",
                        "PataPawa PrePay",
                        "PataPawa PrePay Contract",
                        "Pre Pay Bill Pay",
                        "Bill Pay (Pre)",
                        "",
                        "BillPayment"});
#line 71
 await testRunner.WhenAsync("I create the following Products", ((string)(null)), table13, "When ");
#line hidden
            global::Reqnroll.Table table14 = new global::Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "CalculationType",
                        "FeeDescription",
                        "Value"});
            table14.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Fixed",
                        "Merchant Commission",
                        "2.50"});
            table14.AddRow(new string[] {
                        "Test Estate 1",
                        "PataPawa PostPay",
                        "PataPawa PostPay Contract",
                        "Post Pay Bill Pay",
                        "Percentage",
                        "Merchant Commission",
                        "0.50"});
            table14.AddRow(new string[] {
                        "Test Estate 1",
                        "PataPawa PrePay",
                        "PataPawa PrePay Contract",
                        "Pre Pay Bill Pay",
                        "Percentage",
                        "Merchant Commission",
                        "0.50"});
#line 78
 await testRunner.WhenAsync("I add the following Transaction Fees", ((string)(null)), table14, "When ");
#line hidden
            global::Reqnroll.Table table15 = new global::Reqnroll.Table(new string[] {
                        "MerchantName",
                        "AddressLine1",
                        "AddressLine2",
                        "AddressLine3",
                        "AddressLine4",
                        "Town",
                        "Region",
                        "Country",
                        "ContactName",
                        "EmailAddress",
                        "EstateName"});
            table15.AddRow(new string[] {
                        "Test Merchant 1",
                        "test address line 1",
                        "test address line 2",
                        "test address line 3",
                        "test address line 4",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant1.co.uk",
                        "Test Estate 1"});
#line 84
 await testRunner.GivenAsync("I create the following merchants", ((string)(null)), table15, "Given ");
#line hidden
            global::Reqnroll.Table table16 = new global::Reqnroll.Table(new string[] {
                        "OperatorName",
                        "MerchantName",
                        "MerchantNumber",
                        "TerminalNumber",
                        "EstateName"});
            table16.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate 1"});
            table16.AddRow(new string[] {
                        "Voucher",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate 1"});
            table16.AddRow(new string[] {
                        "PataPawa PostPay",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate 1"});
            table16.AddRow(new string[] {
                        "PataPawa PrePay",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate 1"});
#line 88
 await testRunner.GivenAsync("I have assigned the following  operator to the merchants", ((string)(null)), table16, "Given ");
#line hidden
            global::Reqnroll.Table table17 = new global::Reqnroll.Table(new string[] {
                        "MerchantName",
                        "EstateName"});
            table17.AddRow(new string[] {
                        "Test Merchant 1",
                        "Test Estate 1"});
#line 95
 await testRunner.GivenAsync("I have assigned the following devices to the merchants", ((string)(null)), table17, "Given ");
#line hidden
            global::Reqnroll.Table table18 = new global::Reqnroll.Table(new string[] {
                        "EstateName",
                        "MerchantName",
                        "ContractDescription"});
            table18.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "Safaricom Contract"});
            table18.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "Hospital 1 Contract"});
            table18.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "PataPawa PostPay Contract"});
            table18.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "PataPawa PrePay Contract"});
#line 99
 await testRunner.WhenAsync("I add the following contracts to the following merchants", ((string)(null)), table18, "When ");
#line hidden
            global::Reqnroll.Table table19 = new global::Reqnroll.Table(new string[] {
                        "EmailAddress",
                        "Password",
                        "GivenName",
                        "FamilyName",
                        "EstateName",
                        "MerchantName"});
            table19.AddRow(new string[] {
                        "user1",
                        "123456",
                        "TestMerchant",
                        "User1",
                        "Test Estate 1",
                        "Test Merchant 1"});
#line 106
 await testRunner.GivenAsync("I have created the following security users", ((string)(null)), table19, "Given ");
#line hidden
            global::Reqnroll.Table table20 = new global::Reqnroll.Table(new string[] {
                        "Reference",
                        "Amount",
                        "DateTime",
                        "MerchantName",
                        "EstateName"});
            table20.AddRow(new string[] {
                        "Deposit1",
                        "100.00",
                        "Today",
                        "Test Merchant 1",
                        "Test Estate 1"});
#line 110
 await testRunner.GivenAsync("I make the following manual merchant deposits", ((string)(null)), table20, "Given ");
#line hidden
#line 116
 await testRunner.GivenAsync("I have created a config for my device", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("EndToEnd")]
        [NUnit.Framework.CategoryAttribute("PRTest")]
        public async global::System.Threading.Tasks.Task EndToEnd()
        {
            string[] tagsOfScenario = new string[] {
                    "PRTest"};
            global::System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new global::System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("EndToEnd", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 119
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 4
await this.FeatureBackgroundAsync();
#line hidden
#line 120
 await testRunner.GivenAsync("I am on the Login Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 121
 await testRunner.AndAsync("my device is registered", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 122
 await testRunner.WhenAsync("I enter \'user1\' as the Email Address", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 123
 await testRunner.AndAsync("I enter \'123456\' as the Password", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 124
 await testRunner.AndAsync("I tap on Login", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 125
 await testRunner.ThenAsync("the Merchant Home Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 126
 await testRunner.WhenAsync("I tap on Profile", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 127
 await testRunner.ThenAsync("the My Profile Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 128
 await testRunner.WhenAsync("I tap on the Addresses button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 129
 await testRunner.ThenAsync("the Address List Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table21 = new global::Reqnroll.Table(new string[] {
                            "AddressLine1",
                            "AddressTown"});
                table21.AddRow(new string[] {
                            "test address line 1",
                            "TestTown"});
#line 130
 await testRunner.AndAsync("the Primary Address is displayed", ((string)(null)), table21, "And ");
#line hidden
#line 133
 await testRunner.WhenAsync("I click on the back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 134
 await testRunner.ThenAsync("the My Profile Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 135
 await testRunner.WhenAsync("I tap on the Account Info button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 136
 await testRunner.ThenAsync("the Account Info Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table22 = new global::Reqnroll.Table(new string[] {
                            "Name",
                            "Balance",
                            "AvailableBalance"});
                table22.AddRow(new string[] {
                            "Test Merchant 1",
                            "0",
                            "0"});
#line 137
 await testRunner.AndAsync("the Account Info is displayed", ((string)(null)), table22, "And ");
#line hidden
#line 140
 await testRunner.WhenAsync("I click on the back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 141
 await testRunner.ThenAsync("the My Profile Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 142
 await testRunner.WhenAsync("I click on the back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 143
 await testRunner.ThenAsync("the Merchant Home Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 144
 await testRunner.WhenAsync("I tap on Transactions", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 145
 await testRunner.ThenAsync("the Transaction Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 146
 await testRunner.WhenAsync("I tap on the Mobile Topup button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 147
 await testRunner.ThenAsync("the Transaction Select Mobile Topup Operator Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 148
 await testRunner.WhenAsync("I tap on the \'Safaricom\' button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 149
 await testRunner.ThenAsync("the Select Product Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 150
 await testRunner.WhenAsync("I tap on the \'Custom\' product button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 151
 await testRunner.ThenAsync("the Enter Topup Details Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 152
 await testRunner.WhenAsync("I enter \'07777777775\' as the Customer Mobile Number", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 153
 await testRunner.AndAsync("I enter 10.00 as the Topup Amount", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 154
 await testRunner.AndAsync("I tap on Perform Topup", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 155
 await testRunner.ThenAsync("the Mobile Topup Successful Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 156
 await testRunner.AndAsync("I tap on Complete", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 157
 await testRunner.ThenAsync("the Transaction Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 158
 await testRunner.WhenAsync("I tap on the Voucher button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 159
 await testRunner.ThenAsync("the Transaction Select Voucher Operator Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 160
 await testRunner.WhenAsync("I tap on the \'Hospital 1 Contract\' button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 161
 await testRunner.ThenAsync("the Select Product Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 162
 await testRunner.WhenAsync("I tap on the \'10 KES\' product button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 163
 await testRunner.ThenAsync("the Enter Voucher Issue Details Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 164
 await testRunner.WhenAsync("I enter \'07777777775\' as the Recipient Mobile Number", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 165
 await testRunner.AndAsync("I tap on Issue Voucher", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 166
 await testRunner.ThenAsync("the Voucher Issue Successful Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 167
 await testRunner.AndAsync("I tap on Complete", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 168
 await testRunner.ThenAsync("the Transaction Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 169
 await testRunner.WhenAsync("I tap on the Bill Payment button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 170
 await testRunner.ThenAsync("the Transaction Select Bill Payment Operator Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 171
 await testRunner.WhenAsync("I tap on the \'PataPawa PostPay\' button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 172
 await testRunner.ThenAsync("the Select Product Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 173
 await testRunner.WhenAsync("I tap on the \'Bill Pay (Post)\' product button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 174
 await testRunner.ThenAsync("the Enter Account Details Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 175
 await testRunner.WhenAsync("I enter \'12345678\' as the Account Number", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 176
 await testRunner.AndAsync("I tap on the Get Account Button", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 177
 await testRunner.ThenAsync("the Make Bill Payment page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table23 = new global::Reqnroll.Table(new string[] {
                            "AccountNumber",
                            "AccountHolder",
                            "DueDate",
                            "Balance"});
                table23.AddRow(new string[] {
                            "12345678",
                            "Test Account 1",
                            "Today",
                            "100.00"});
#line 178
 await testRunner.AndAsync("the following Bill Details are displayed", ((string)(null)), table23, "And ");
#line hidden
#line 181
 await testRunner.WhenAsync("I enter \'07777777775\' as the Customer Mobile Number", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 182
 await testRunner.AndAsync("I enter 10.00 as the Payment Amount", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 183
 await testRunner.AndAsync("I tap on the Make Payment Button", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 184
 await testRunner.ThenAsync("the Bill Payment Successful Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 185
 await testRunner.AndAsync("I tap on Complete", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 186
 await testRunner.ThenAsync("the Transaction Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 187
 await testRunner.WhenAsync("I tap on the Bill Payment button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 188
 await testRunner.ThenAsync("the Transaction Select Bill Payment Operator Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 189
 await testRunner.WhenAsync("I tap on the \'PataPawa PrePay\' button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 190
 await testRunner.ThenAsync("the Select Product Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 191
 await testRunner.WhenAsync("I tap on the \'Bill Pay (Pre)\' product button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 192
 await testRunner.ThenAsync("the Enter Meter Details Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 193
 await testRunner.WhenAsync("I enter \'00000001\' as the Meter Number", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 194
 await testRunner.AndAsync("I tap on the Get Meter Button", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 195
 await testRunner.ThenAsync("the Make Bill Payment page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table24 = new global::Reqnroll.Table(new string[] {
                            "MeterNumber"});
                table24.AddRow(new string[] {
                            "00000001"});
#line 196
 await testRunner.AndAsync("the following Meter Details are displayed", ((string)(null)), table24, "And ");
#line hidden
#line 199
 await testRunner.WhenAsync("I enter 10.00 as the Payment Amount", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 200
 await testRunner.AndAsync("I tap on the Make Payment Button", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 201
 await testRunner.ThenAsync("the Bill Payment Successful Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 202
 await testRunner.AndAsync("I tap on Complete", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 203
 await testRunner.ThenAsync("the Transaction Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 204
 await testRunner.WhenAsync("I click on the back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 205
 await testRunner.ThenAsync("the Merchant Home Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 206
 await testRunner.WhenAsync("I click on the back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 207
 await testRunner.ThenAsync("A message is displayed confirming I want to log out", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 208
 await testRunner.WhenAsync("I click yes", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 209
 await testRunner.ThenAsync("the Login Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion
