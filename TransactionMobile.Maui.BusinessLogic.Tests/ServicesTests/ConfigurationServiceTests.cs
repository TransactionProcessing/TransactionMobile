using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Tests.ServicesTests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using Logging;
    using Models;
    using Newtonsoft.Json;
    using RichardSzalay.MockHttp;
    using Services;
    using Services.DataTransferObjects;
    using Shouldly;
    using Xunit;

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
                                                                                                                          ServiceType = ServiceType.EstateManagement, 
                                                                                                                          Uri = "http://localhost:5000"
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
                                                              ApplicationCentreConfiguration = new ApplicationCentreConfiguration{
                                                                                                                                     AndroidKey = "Android",
                                                                                                                                     MacosKey = "MacOS",
                                                                                                                                     WindowsKey = "Windows",
                                                                                                                                     IosKey = "iOS",
                                                                                                                                     ApplicationId = "ApplicationId"
                                                              },
                                                              DeviceIdentifier = TestData.DeviceIdentifier,
                                                              LogLevel = configLoggingLevel,
                                                              Id = Guid.NewGuid().ToString(),
                                                          };
            Logger.Initialise(new NullLogger());

            this.MockHttpMessageHandler.When($"http://localhost/api/transactionmobileconfiguration/{TestData.DeviceIdentifier}")
                .Respond("application/json", JsonConvert.SerializeObject(expectedConfiguration)); // Respond with JSON

            var configurationResult = await ConfigurationService.GetConfiguration(TestData.DeviceIdentifier, CancellationToken.None);
            configurationResult.IsSuccess.ShouldBeTrue();
            configurationResult.Data.ShouldNotBeNull();
            configurationResult.Data.ClientSecret.ShouldBe(expectedConfiguration.ClientSecret);
            configurationResult.Data.ClientId.ShouldBe(expectedConfiguration.ClientId);
            configurationResult.Data.EnableAutoUpdates.ShouldBe(expectedConfiguration.EnableAutoUpdates);
            configurationResult.Data.EstateManagementUri.ShouldBe(expectedConfiguration.HostAddresses.Single(s => s.ServiceType == ServiceType.EstateManagement).Uri);
            configurationResult.Data.SecurityServiceUri.ShouldBe(expectedConfiguration.HostAddresses.Single(s => s.ServiceType == ServiceType.Security).Uri);
            configurationResult.Data.TransactionProcessorAclUri.ShouldBe(expectedConfiguration.HostAddresses.Single(s => s.ServiceType == ServiceType.TransactionProcessorAcl).Uri);
            configurationResult.Data.LogLevel.ShouldBe(expectedLogLevel);
            configurationResult.Data.AppCenterConfig.AndroidKey.ShouldBe(expectedConfiguration.ApplicationCentreConfiguration.AndroidKey);
            configurationResult.Data.AppCenterConfig.MacOSKey.ShouldBe(expectedConfiguration.ApplicationCentreConfiguration.MacosKey);
            configurationResult.Data.AppCenterConfig.WindowsKey.ShouldBe(expectedConfiguration.ApplicationCentreConfiguration.WindowsKey);
            configurationResult.Data.AppCenterConfig.iOSKey.ShouldBe(expectedConfiguration.ApplicationCentreConfiguration.IosKey);
        }

        [Fact]
        public async Task ConfigurationService_GetConfiguration_FailedHttpCall_ResultFailed()
        {
            Logger.Initialise(new NullLogger());

            this.MockHttpMessageHandler.When($"http://localhost/api/transactionmobileconfiguration/{TestData.DeviceIdentifier}")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest));

            var configurationResult = await ConfigurationService.GetConfiguration(TestData.DeviceIdentifier, CancellationToken.None);
            configurationResult.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ConfigurationService_GetConfiguration_FailedHttpCall_NotFound_ResultFailed()
        {
            Logger.Initialise(new NullLogger());

            this.MockHttpMessageHandler.When($"http://localhost/api/transactionmobileconfiguration/{TestData.DeviceIdentifier}")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.NotFound));

            var configurationResult = await ConfigurationService.GetConfiguration(TestData.DeviceIdentifier, CancellationToken.None);
            configurationResult.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ConfigurationService_PostDiagnosticLogs_ResultSuccess()
        {
            Logger.Initialise(new NullLogger());

            this.MockHttpMessageHandler.When($"http://localhost/transactionmobilelogging/{TestData.DeviceIdentifier}")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.OK));

            Should.NotThrow(async () => {
                                await ConfigurationService.PostDiagnosticLogs(TestData.DeviceIdentifier, null, CancellationToken.None);
                            });
        }

    }
}
