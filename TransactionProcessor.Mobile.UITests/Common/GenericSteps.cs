using NLog;
using Reqnroll;
using Shared.Logger;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Common;

[Binding]
[Scope(Tag = "base")]
public class GenericSteps
{
    private readonly ScenarioContext scenarioContext;
    private readonly TestingContext testingContext;
    private readonly AppiumDriverWrapper appiumDriver;

    public GenericSteps(ScenarioContext scenarioContext,
                        TestingContext testingContext,
                        AppiumDriverWrapper appiumDriver)
    {
        this.scenarioContext = scenarioContext;
        this.testingContext = testingContext;
        this.appiumDriver = appiumDriver;
    }

    [BeforeScenario(Order = 0)]
    public async Task StartSystem()
    {
        string scenarioName = this.scenarioContext.ScenarioInfo.Title.Replace(" ", "");
        NlogLogger logger = new();
        logger.Initialise(LogManager.GetLogger(scenarioName), scenarioName);
        LogManager.AddHiddenAssembly(typeof(NlogLogger).Assembly);

        this.testingContext.Logger = logger;
        this.testingContext.TestHostHelper = new TestHostHelper(this.testingContext);
        this.testingContext.TestHostHelper.Logger = logger;

        try
        {
            this.testingContext.Logger.LogInformation("About to start local test host");
            await Setup.GlobalSetup().ConfigureAwait(false);
            await this.testingContext.TestHostHelper.StartTestHostForScenarioRun(scenarioName).ConfigureAwait(false);
            this.testingContext.Logger.LogInformation("Local test host started");
        }
        catch
        {
            await UiFailureDiagnostics.CaptureAsync(this.testingContext, this.scenarioContext, this.appiumDriver, "BeforeScenario").ConfigureAwait(false);
            throw;
        }
    }

    [AfterStep(Order = 100)]
    public async Task CaptureFailureDiagnosticsAfterStep()
    {
        if (this.scenarioContext.TestError == null)
        {
            return;
        }

        await UiFailureDiagnostics.CaptureAsync(this.testingContext, this.scenarioContext, this.appiumDriver, "AfterStep").ConfigureAwait(false);
    }

    [AfterScenario(Order = 0)]
    public async Task StopSystem()
    {
        this.testingContext.Logger?.LogInformation("About to stop local test host");
        if (this.testingContext.TestHostHelper != null)
        {
            await this.testingContext.TestHostHelper.StopTestHostForScenarioRun().ConfigureAwait(false);
        }

        this.testingContext.Logger?.LogInformation("Local test host stopped");
    }
}

