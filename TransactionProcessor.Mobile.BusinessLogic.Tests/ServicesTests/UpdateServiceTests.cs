using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ServicesTests;

public class UpdateServiceTests
{
    private readonly MockHttpMessageHandler MockHttpMessageHandler;

    private readonly IUpdateService UpdateService;

    public UpdateServiceTests()
    {
        this.MockHttpMessageHandler = new MockHttpMessageHandler();
        this.UpdateService = new UpdateService(_ => "http://localhost", this.MockHttpMessageHandler.ToHttpClient());
    }

    [Fact]
    public async Task UpdateService_CheckForUpdates_ResultSuccess_And_UpdateResponseReturned()
    {
        Logger.Initialise(new NullLogger());

        ApplicationUpdateCheckResponse expectedResponse = new()
        {
            DownloadUri = "https://updates.example.com/transactionmobile.apk",
            LatestVersion = "1.0.1",
            Message = "Install the latest version.",
            UpdateRequired = true
        };

        this.MockHttpMessageHandler.When("http://localhost/api/applicationupdates/check")
            .Respond("application/json", JsonConvert.SerializeObject(expectedResponse));

        Result<ApplicationUpdateCheckResponse> updateResult = await this.UpdateService.CheckForUpdates(TestData.ApplicationVersion,
                                                                                                       "com.transactionprocessor.mobile",
                                                                                                       "Android",
                                                                                                       TestData.DeviceIdentifier,
                                                                                                       CancellationToken.None);

        updateResult.IsSuccess.ShouldBeTrue();
        updateResult.Data.ShouldNotBeNull();
        updateResult.Data.UpdateRequired.ShouldBeTrue();
        updateResult.Data.DownloadUri.ShouldBe(expectedResponse.DownloadUri);
        updateResult.Data.LatestVersion.ShouldBe(expectedResponse.LatestVersion);
    }

    [Fact]
    public async Task UpdateService_CheckForUpdates_FailedHttpCall_ResultFailed()
    {
        Logger.Initialise(new NullLogger());

        this.MockHttpMessageHandler.When("http://localhost/api/applicationupdates/check")
            .Respond(System.Net.HttpStatusCode.BadRequest);

        Result<ApplicationUpdateCheckResponse> updateResult = await this.UpdateService.CheckForUpdates(TestData.ApplicationVersion,
                                                                                                       "com.transactionprocessor.mobile",
                                                                                                       "Android",
                                                                                                       TestData.DeviceIdentifier,
                                                                                                       CancellationToken.None);

        updateResult.IsFailed.ShouldBeTrue();
    }
}
