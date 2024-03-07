using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using System.Net;
    using ClientProxyBase;
    using Common;
    using Logging;
    using Microsoft.Extensions.Logging;
    using Models;
    using Newtonsoft.Json;
    using RequestHandlers;
    using SimpleResults;
    using TransactionMobile.Maui.BusinessLogic.Services.DataTransferObjects;
    using ViewModels;
    using LogLevel = Models.LogLevel;


    public class ConfigurationService : ClientProxyBase, IConfigurationService
    {
        private readonly Func<String, String> BaseAddressResolver;

        public ConfigurationService(Func<String, String> baseAddressResolver,
                                    HttpClient httpClient) : base(httpClient) {
            this.BaseAddressResolver = baseAddressResolver;
        }


        private String BuildRequestUrl(String route)
        {
            String baseAddress = this.BaseAddressResolver("ConfigServiceUrl");

            String requestUri = $"{baseAddress}{route}";

            return requestUri;
        }

        protected override async Task<String> HandleResponse(HttpResponseMessage responseMessage,
                                                             CancellationToken cancellationToken)
        {
            String content = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                // No error as maybe running under CI (which has no internet)
                return content;
            }

            return await base.HandleResponse(responseMessage, cancellationToken);
        }

        public async Task<Result<Configuration>> GetConfiguration(String deviceIdentifier,
                                                                  CancellationToken cancellationToken)
        {
            Configuration response = null;
            String requestUri = this.BuildRequestUrl($"/api/transactionmobileconfiguration/{deviceIdentifier}");

            try
            {
                Logger.LogInformation($"About to request configuration for device identifier {deviceIdentifier}");
                Logger.LogDebug($"Configuration Request details: Uri {requestUri}");

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);
                Logger.LogDebug($"Configuration Response [{httpResponse.StatusCode}]");
                
                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);
                Logger.LogDebug($"Configuration Response Content [{content}]");

                // call was successful so now deserialise the body to the response object
                ConfigurationResponse apiResponse = JsonConvert.DeserializeObject<ConfigurationResponse>(content);

                Logger.LogDebug($"About to build Configuration");
                response = new Configuration() {
                                                   ClientSecret = apiResponse.ClientSecret,
                                                   ClientId = apiResponse.ClientId,
                                                   EnableAutoUpdates = apiResponse.EnableAutoUpdates,
                                                   EstateManagementUri = apiResponse.HostAddresses.Single(h => h.ServiceType == ServiceType.EstateManagement).Uri,
                                                   SecurityServiceUri = apiResponse.HostAddresses.Single(h => h.ServiceType == ServiceType.Security).Uri,
                                                   TransactionProcessorAclUri =
                                                       apiResponse.HostAddresses.Single(h => h.ServiceType == ServiceType.TransactionProcessorAcl).Uri,
                                               };

                Logger.LogDebug($"About to xlate log level");
                response.LogLevel = apiResponse.LogLevel switch
                {
                    LoggingLevel.Debug => LogLevel.Debug,
                    LoggingLevel.Error => LogLevel.Error,
                    LoggingLevel.Fatal => LogLevel.Fatal,
                    LoggingLevel.Information => LogLevel.Info,
                    LoggingLevel.Trace => LogLevel.Trace,
                    LoggingLevel.Warning => LogLevel.Warn,
                    _ => LogLevel.Info
                };

                Logger.LogDebug($"About to xlate app centre config");
                response.AppCenterConfig = new AppCenterConfiguration() {
                                                                            AndroidKey = apiResponse.ApplicationCentreConfiguration.AndroidKey,
                                                                            MacOSKey = apiResponse.ApplicationCentreConfiguration.MacosKey,
                                                                            WindowsKey = apiResponse.ApplicationCentreConfiguration.WindowsKey,
                                                                            iOSKey = apiResponse.ApplicationCentreConfiguration.IosKey
                                                                        };

                Logger.LogInformation($"Configuration for device identifier {deviceIdentifier} requested successfully");
                Logger.LogDebug($"Configuration Response: [{content}]");

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Logger.LogError($"Error getting configuration for device Id {deviceIdentifier}.",ex);

                return ResultExtensions.FailureExtended($"Error getting configuration data for device Id {deviceIdentifier}", ex);
            }
        }

        public async Task PostDiagnosticLogs(String deviceIdentifier,
                                             List<LogMessage> logMessages,
                                             CancellationToken cancellationToken)
        {
            String requestUri = this.BuildRequestUrl($"/transactionmobilelogging");

            // Create a container
            var container = new
            {
                messages = logMessages
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(container), Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, content, cancellationToken);

            await this.HandleResponse(httpResponse, cancellationToken);
        }
    }
}
