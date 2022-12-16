﻿namespace TransactionMobile.Maui.UiTests.Common;

using System;
using System.Threading.Tasks;
using NLog;
using Shared.IntegrationTesting;
using Shared.Logger;
using TechTalk.SpecFlow;

[Binding]
[Scope(Tag = "base")]
public class GenericSteps
{
    private readonly ScenarioContext ScenarioContext;

    private readonly TestingContext TestingContext;

    public GenericSteps(ScenarioContext scenarioContext,
                        TestingContext testingContext)
    {
        this.ScenarioContext = scenarioContext;
        this.TestingContext = testingContext;
    }

    [BeforeScenario(Order = 0)]
    public async Task StartSystem()
    {
        return;
        Setup.GlobalSetup();

        // Initialise a logger
        String scenarioName = this.ScenarioContext.ScenarioInfo.Title.Replace(" ", "");
        NlogLogger logger = new NlogLogger();
        logger.Initialise(LogManager.GetLogger(scenarioName), scenarioName);
        LogManager.AddHiddenAssembly(typeof(NlogLogger).Assembly);

        this.TestingContext.DockerHelper = new DockerHelper();
        this.TestingContext.DockerHelper.Logger = logger;
        this.TestingContext.DockerHelper.SqlServerContainer = Setup.DatabaseServerContainer;
        this.TestingContext.DockerHelper.SqlServerNetwork = Setup.DatabaseServerNetwork;
        this.TestingContext.DockerHelper.DockerCredentials = Setup.DockerCredentials;
        this.TestingContext.DockerHelper.SqlCredentials = Setup.SqlCredentials;
        this.TestingContext.DockerHelper.SqlServerContainerName = "sharedsqlserver";

        this.TestingContext.Logger = logger;
        this.TestingContext.Logger.LogInformation("About to Start Containers for Scenario Run");
        await this.TestingContext.DockerHelper.StartContainersForScenarioRun(scenarioName).ConfigureAwait(false);
        this.TestingContext.Logger.LogInformation("Containers for Scenario Run Started");
    }

    [AfterScenario(Order = 0)]
    public async Task StopSystem()
    {
        return;
        this.TestingContext.Logger.LogInformation("About to Stop Containers for Scenario Run");
        await this.TestingContext.DockerHelper.StopContainersForScenarioRun().ConfigureAwait(false);
        this.TestingContext.Logger.LogInformation("Containers for Scenario Run Stopped");
    }
}