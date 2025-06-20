using System.Net;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Shouldly;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.Services.DataTransferObjects;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ServicesTests
{
    public class ConfigurationServiceTests{
        private MockHttpMessageHandler MockHttpMessageHandler;

        private Func<String, String> BaseAddressResolver;

        private IConfigurationService ConfigurationService;
        public ConfigurationServiceTests(){
            this.MockHttpMessageHandler = new MockHttpMessageHandler();
            this.BaseAddressResolver = (s) => $"http://localhost";
            this.ConfigurationService = new ConfigurationService(this.BaseAddressResolver, this.MockHttpMessageHandler.ToHttpClient());
        }

        [Theory]
        [InlineData(LoggingLevel.Debug, LogLevel.Debug)]
        [InlineData(LoggingLevel.Error, LogLevel.Error)]
        [InlineData(LoggingLevel.Fatal, LogLevel.Fatal)]
        [InlineData(LoggingLevel.Information, LogLevel.Info)]
        [InlineData(LoggingLevel.Trace, LogLevel.Trace)]
        [InlineData(LoggingLevel.Warning, LogLevel.Warn)]
        [InlineData((LoggingLevel)99, LogLevel.Info)]

        public async Task ConfigurationService_GetConfiguration_ResultSuccess_And_ConfigReturned(LoggingLevel configLoggingLevel, LogLevel expectedLogLevel)
        {

            ConfigurationResponse expectedConfiguration = new ConfigurationResponse
                                                          {
                                                              ClientId = "clientId",
                                                              ClientSecret = Guid.NewGuid().ToString(),
                                                              EnableAutoUpdates = false,
                                                              HostAddresses = new List<HostAddress>{
                                                                                                       new HostAddress{
                                                                                                                          ServiceType = ServiceType.TransactionProcessor, 
                                                                                                                          Uri = "http://localhost:5001"
                                                                                                                      },
                                                                                                       new HostAddress{
                                                                                                                          ServiceType = ServiceType.Security,
                                                                                                                          Uri = "http://localhost:5001"
                                                                                                                      },
                                                                                                       new HostAddress{
                                                                                                                          ServiceType = ServiceType.TransactionProcessorAcl,
                                                                                                                          Uri = "http://localhost:5003"
                                                                                                                      }
                                                                                                   },
                                                              DeviceIdentifier = TestData.DeviceIdentifier,
                                                              LogLevel = configLoggingLevel,
                                                              Id = Guid.NewGuid().ToString(),
                                                          };
            Logger.Initialise(new NullLogger());

            this.MockHttpMessageHandler.When($"http://localhost/api/transactionmobileconfiguration/{TestData.DeviceIdentifier}")
                .Respond("application/json", JsonConvert.SerializeObject(expectedConfiguration)); // Respond with JSON

            var configurationResult = await this.ConfigurationService.GetConfiguration(TestData.DeviceIdentifier, CancellationToken.None);
            configurationResult.IsSuccess.ShouldBeTrue();
            configurationResult.Data.ShouldNotBeNull();
            configurationResult.Data.ClientSecret.ShouldBe(expectedConfiguration.ClientSecret);
            configurationResult.Data.ClientId.ShouldBe(expectedConfiguration.ClientId);
            configurationResult.Data.EnableAutoUpdates.ShouldBe(expectedConfiguration.EnableAutoUpdates);
            configurationResult.Data.TransactionProcessorUri.ShouldBe(expectedConfiguration.HostAddresses.Single(s => s.ServiceType == ServiceType.TransactionProcessor).Uri);
            configurationResult.Data.SecurityServiceUri.ShouldBe(expectedConfiguration.HostAddresses.Single(s => s.ServiceType == ServiceType.Security).Uri);
            configurationResult.Data.TransactionProcessorAclUri.ShouldBe(expectedConfiguration.HostAddresses.Single(s => s.ServiceType == ServiceType.TransactionProcessorAcl).Uri);
            configurationResult.Data.LogLevel.ShouldBe(expectedLogLevel);
        }

        [Fact]
        public async Task ConfigurationService_GetConfiguration_FailedHttpCall_ResultFailed()
        {
            Logger.Initialise(new NullLogger());

            this.MockHttpMessageHandler.When($"http://localhost/api/transactionmobileconfiguration/{TestData.DeviceIdentifier}")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest));

            var configurationResult = await this.ConfigurationService.GetConfiguration(TestData.DeviceIdentifier, CancellationToken.None);
            configurationResult.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ConfigurationService_GetConfiguration_FailedHttpCall_NotFound_ResultFailed()
        {
            Logger.Initialise(new NullLogger());

            this.MockHttpMessageHandler.When($"http://localhost/api/transactionmobileconfiguration/{TestData.DeviceIdentifier}")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.NotFound));

            var configurationResult = await this.ConfigurationService.GetConfiguration(TestData.DeviceIdentifier, CancellationToken.None);
            configurationResult.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ConfigurationService_PostDiagnosticLogs_ResultSuccess()
        {
            Logger.Initialise(new NullLogger());

            this.MockHttpMessageHandler.When($"http://localhost/transactionmobilelogging/{TestData.DeviceIdentifier}")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.OK));

            Should.NotThrow(async () => {
                                await this.ConfigurationService.PostDiagnosticLogs(TestData.DeviceIdentifier, null, CancellationToken.None);
                            });
        }

    }
}
