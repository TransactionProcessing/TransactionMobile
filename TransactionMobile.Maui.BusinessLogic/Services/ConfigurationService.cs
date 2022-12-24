using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using System.Net;
    using ClientProxyBase;
    using Logging;
    using Microsoft.Extensions.Logging;
    using Models;
    using Newtonsoft.Json;
    using RequestHandlers;
    using ViewModels;
    using LogLevel = Models.LogLevel;

    public class LoggingService 

    public class ConfigurationService : ClientProxyBase, IConfigurationService
    {
        private readonly ILoggerService Logger;

        private readonly Func<String, String> BaseAddressResolver;

        public ConfigurationService(ILoggerService logger, 
                                    Func<String, String> baseAddressResolver,
                                    HttpClient httpClient) : base(httpClient) {
            this.Logger = logger;
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
                await Logger.LogInformation($"About to request configuration for device identifier {deviceIdentifier}");
                await Logger.LogDebug($"Configuration Request details: Uri {requestUri}");

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                ConfigurationResponse apiResponse = JsonConvert.DeserializeObject<ConfigurationResponse>(content);

                response = new Configuration() {
                                                   ClientSecret = apiResponse.ClientSecret,
                                                   ClientId = apiResponse.ClientId,
                                                   EnableAutoUpdates = apiResponse.EnableAutoUpdates,
                                                   EstateManagementUri = apiResponse.HostAddresses.Single(h => h.ServiceType == ServiceType.EstateManagement).Uri,
                                                   SecurityServiceUri = apiResponse.HostAddresses.Single(h => h.ServiceType == ServiceType.Security).Uri,
                                                   TransactionProcessorAclUri =
                                                       apiResponse.HostAddresses.Single(h => h.ServiceType == ServiceType.TransactionProcessorAcl).Uri,
                                               };

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

                response.AppCenterConfig = new AppCenterConfiguration() {
                                                                            AndroidKey = apiResponse.ApplicationCentreConfiguration.AndroidKey,
                                                                            MacOSKey = apiResponse.ApplicationCentreConfiguration.MacosKey,
                                                                            WindowsKey = apiResponse.ApplicationCentreConfiguration.WindowsKey,
                                                                            iOSKey = apiResponse.ApplicationCentreConfiguration.IosKey
                                                                        };

                await Logger.LogInformation($"Configuration for device identifier {deviceIdentifier} requested successfully");
                await Logger.LogDebug($"Configuration Response: [{content}]");

                return new SuccessResult<Configuration>(response);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                await Logger.LogError($"Error getting configuration for device Id {deviceIdentifier}.",ex);

                return new ErrorResult<Configuration>("Error getting configuration data");
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

    public class ConfigurationResponse
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string DeviceIdentifier { get; set; }

        public bool EnableAutoUpdates { get; set; }

        public List<HostAddress> HostAddresses { get; set; }

        public string Id { get; set; }

        public LoggingLevel LogLevel { get; set; }

        public ApplicationCentreConfiguration ApplicationCentreConfiguration { get; set; }
    }

    public class HostAddress
    {
        public ServiceType ServiceType { get; set; }

        public string Uri { get; set; }
    }

    public enum ServiceType
    {
        EstateManagement = 0,
        Security = 1,
        TransactionProcessorAcl = 2,
        VoucherManagementAcl = 3,
    }

    public enum LoggingLevel
    {
        Fatal = 0,
        Error = 1,
        Warning = 2,
        Information = 3,
        Debug = 4,
        Trace = 5
    }

    public class ApplicationCentreConfiguration
    {
        public String ApplicationId { get; set; }
        public String AndroidKey { get; set; }
        public String IosKey { get; set; }
        public String MacosKey { get; set; }
        public String WindowsKey { get; set; }
    }
}
