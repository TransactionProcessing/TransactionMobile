using NLog;
using Reqnroll;
using Shared.IntegrationTesting;
using Shared.Logger;

namespace TransactionProcessor.Mobile.UITests.Common;

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
    public async Task StartSystem(){
        if (this.ScenarioContext.ScenarioInfo.Tags.Contains("PRNavTest") ||
            this.ScenarioContext.ScenarioInfo.Tags.Contains("PRHWNavTest") ||
            this.ScenarioContext.ScenarioInfo.Tags.Contains("iOSPRTest"))
        {
            // Initialise a logger
            String scenarioName = this.ScenarioContext.ScenarioInfo.Title.Replace(" ", "");
            NlogLogger logger = new NlogLogger();
            logger.Initialise(LogManager.GetLogger(scenarioName), scenarioName);
            LogManager.AddHiddenAssembly(typeof(NlogLogger).Assembly);
            this.TestingContext.Logger = logger;
        }
        else{
            DockerServices dockerServices = DockerServices.EventStore |
                                            DockerServices.MessagingService | DockerServices.SecurityService |
                                            DockerServices.TestHost | DockerServices.SqlServer | DockerServices.TransactionProcessor |
                                            DockerServices.TransactionProcessorAcl;

            // Initialise a logger
            String scenarioName = this.ScenarioContext.ScenarioInfo.Title.Replace(" ", "");
            NlogLogger logger = new NlogLogger();
            logger.Initialise(LogManager.GetLogger(scenarioName), scenarioName);
            LogManager.AddHiddenAssembly(typeof(NlogLogger).Assembly);

            this.TestingContext.DockerHelper = new DockerHelper();
            this.TestingContext.DockerHelper.Logger = logger;
            this.TestingContext.Logger = logger;
            this.TestingContext.DockerHelper.RequiredDockerServices = dockerServices;
            this.TestingContext.Logger.LogInformation("About to Start Global Setup");

            String? isCi = Environment.GetEnvironmentVariable("IsCI");
            this.TestingContext.Logger.LogInformation($"IsCI [{isCi}]");
            if (String.Compare(isCi, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                // override teh SQL Server image
                this.TestingContext.Logger.LogInformation("Sql Image overridden");
                this.TestingContext.DockerHelper.SetImageDetails(ContainerType.SqlServer, ("mssqlserver:2022-ltsc2022", false));
            }

            await Setup.GlobalSetup(this.TestingContext.DockerHelper);
            
        this.TestingContext.DockerHelper.SqlServerContainer = Setup.DatabaseServerContainer;
        this.TestingContext.DockerHelper.SqlServerNetwork = Setup.DatabaseServerNetwork;
        this.TestingContext.DockerHelper.DockerCredentials = Setup.DockerCredentials;
        this.TestingContext.DockerHelper.SqlCredentials = Setup.SqlCredentials;
        this.TestingContext.DockerHelper.SqlServerContainerName = "sharedsqlserver";

        this.TestingContext.Logger.LogInformation("About to Start Containers for Scenario Run");

        

            await this.TestingContext.DockerHelper.StartContainersForScenarioRun(scenarioName, dockerServices).ConfigureAwait(false);
        this.TestingContext.Logger.LogInformation("Containers for Scenario Run Started");
        }
    }

    [AfterScenario(Order = 0)]
    public async Task StopSystem()
    {
        if (this.ScenarioContext.ScenarioInfo.Tags.Contains("PRNavTest") == false && this.ScenarioContext.ScenarioInfo.Tags.Contains("PRHWNavTest") == false
            && this.ScenarioContext.ScenarioInfo.Tags.Contains("iOSPRTest") == false)
        {
            this.TestingContext.Logger.LogInformation("About to Stop Containers for Scenario Run");
            await this.TestingContext.DockerHelper.StopContainersForScenarioRun(DockerServices.SqlServer).ConfigureAwait(false);
            this.TestingContext.Logger.LogInformation("Containers for Scenario Run Stopped");
        }
    }
}