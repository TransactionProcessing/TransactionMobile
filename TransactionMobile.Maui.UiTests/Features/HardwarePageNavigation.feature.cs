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
namespace TransactionMobile.Maui.UiTests.Features
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("HardwarePageNavigation")]
    [NUnit.Framework.FixtureLifeCycleAttribute(NUnit.Framework.LifeCycle.InstancePerTestCase)]
    [NUnit.Framework.CategoryAttribute("background")]
    [NUnit.Framework.CategoryAttribute("login")]
    [NUnit.Framework.CategoryAttribute("toolbar")]
    [NUnit.Framework.CategoryAttribute("profile")]
    [NUnit.Framework.CategoryAttribute("shared")]
    [NUnit.Framework.CategoryAttribute("transactions")]
    [NUnit.Framework.CategoryAttribute("sharedapp")]
    [NUnit.Framework.CategoryAttribute("base")]
    [NUnit.Framework.CategoryAttribute("reports")]
    public partial class HardwarePageNavigationFeature
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "background",
                "login",
                "toolbar",
                "profile",
                "shared",
                "transactions",
                "sharedapp",
                "base",
                "reports"};
        
        private static global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "HardwarePageNavigation", null, global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
        
#line 1 "HardwarePageNavigation.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public static async System.Threading.Tasks.Task FeatureSetupAsync()
        {
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public static async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(featureHint: featureInfo);
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Equals(featureInfo) == false)))
            {
                await testRunner.OnFeatureEndAsync();
            }
            if ((testRunner.FeatureContext == null))
            {
                await testRunner.OnFeatureStartAsync(featureInfo);
            }
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
            global::Reqnroll.TestRunnerManager.ReleaseTestRunner(testRunner);
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Back Button from Login Screen")]
        [NUnit.Framework.CategoryAttribute("PRNavTest")]
        public async System.Threading.Tasks.Task BackButtonFromLoginScreen()
        {
            string[] tagsOfScenario = new string[] {
                    "PRNavTest"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Back Button from Login Screen", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 5
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 6
 await testRunner.GivenAsync("I am on the Login Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 7
 await testRunner.WhenAsync("I click on the device back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 8
 await testRunner.ThenAsync("The application closes", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Device Back Button from Home Page Screen")]
        [NUnit.Framework.CategoryAttribute("PRHWNavTest")]
        public async System.Threading.Tasks.Task DeviceBackButtonFromHomePageScreen()
        {
            string[] tagsOfScenario = new string[] {
                    "PRHWNavTest"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Device Back Button from Home Page Screen", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 12
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 13
 await testRunner.GivenAsync("I am on the Login Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 14
 await testRunner.AndAsync("the application is in training mode", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 15
 await testRunner.WhenAsync("I enter \'user1\' as the Email Address", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 16
 await testRunner.AndAsync("I enter \'123456\' as the Password", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 17
 await testRunner.AndAsync("I tap on Login", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 18
 await testRunner.ThenAsync("the Merchant Home Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 19
 await testRunner.WhenAsync("I click on the device back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 20
 await testRunner.ThenAsync("A message is displayed confirming I want to log out", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 21
 await testRunner.WhenAsync("I click no", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 22
 await testRunner.ThenAsync("the Merchant Home Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 23
 await testRunner.WhenAsync("I click on the device back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 24
 await testRunner.ThenAsync("A message is displayed confirming I want to log out", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 25
 await testRunner.WhenAsync("I click yes", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 26
 await testRunner.ThenAsync("the Login Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Back Button from Transaction Page Screen")]
        [NUnit.Framework.CategoryAttribute("PRHWNavTest")]
        public async System.Threading.Tasks.Task BackButtonFromTransactionPageScreen()
        {
            string[] tagsOfScenario = new string[] {
                    "PRHWNavTest"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Back Button from Transaction Page Screen", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 30
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 31
 await testRunner.GivenAsync("I am on the Login Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 32
 await testRunner.AndAsync("the application is in training mode", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 33
 await testRunner.WhenAsync("I enter \'user1\' as the Email Address", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 34
 await testRunner.AndAsync("I enter \'123456\' as the Password", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 35
 await testRunner.AndAsync("I tap on Login", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 36
 await testRunner.ThenAsync("the Merchant Home Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 37
 await testRunner.WhenAsync("I tap on Transactions", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 38
 await testRunner.ThenAsync("the Transaction Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 39
 await testRunner.WhenAsync("I tap on the Mobile Topup button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 40
 await testRunner.ThenAsync("the Transaction Select Mobile Topup Operator Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 41
 await testRunner.WhenAsync("I click on the device back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 42
 await testRunner.ThenAsync("the Transaction Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 43
 await testRunner.WhenAsync("I click on the device back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 44
 await testRunner.ThenAsync("the Merchant Home Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Back Button from Reports Page Screen")]
        [NUnit.Framework.CategoryAttribute("PRHWNavTest")]
        public async System.Threading.Tasks.Task BackButtonFromReportsPageScreen()
        {
            string[] tagsOfScenario = new string[] {
                    "PRHWNavTest"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Back Button from Reports Page Screen", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 48
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 49
 await testRunner.GivenAsync("I am on the Login Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 50
 await testRunner.AndAsync("the application is in training mode", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 51
 await testRunner.WhenAsync("I enter \'user1\' as the Email Address", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 52
 await testRunner.AndAsync("I enter \'123456\' as the Password", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 53
 await testRunner.AndAsync("I tap on Login", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 54
 await testRunner.ThenAsync("the Merchant Home Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 55
 await testRunner.WhenAsync("I tap on Reports", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 56
 await testRunner.ThenAsync("the Reports Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 57
 await testRunner.WhenAsync("I tap on the Sales Analysis Button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 58
 await testRunner.ThenAsync("the Sales Analysis Report is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 59
 await testRunner.WhenAsync("I click on the device back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 60
 await testRunner.ThenAsync("the Transaction Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 61
 await testRunner.WhenAsync("I click on the device back button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 62
 await testRunner.ThenAsync("the Merchant Home Page is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion
