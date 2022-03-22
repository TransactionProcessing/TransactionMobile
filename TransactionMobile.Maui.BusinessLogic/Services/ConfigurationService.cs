using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using System.Net;
    using ClientProxyBase;
    using Models;
    using Newtonsoft.Json;

    public class ConfigurationService : ClientProxyBase, IConfigurationService
    {
        private readonly Func<String, String> BaseAddressResolver;

        public ConfigurationService(Func<String, String> baseAddressResolver,
                                    HttpClient httpClient) : base(httpClient)
        {
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

        public async Task<Configuration> GetConfiguration(String deviceIdentifier,
                                                          CancellationToken cancellationToken)
        {
            Configuration response = null;
            String requestUri = this.BuildRequestUrl($"/configuration/{deviceIdentifier}");

            try
            {
                Shared.Logger.Logger.LogInformation($"About to request configuration for device identifier {deviceIdentifier}");
                Shared.Logger.Logger.LogDebug($"Configuration Request details: Uri {requestUri}");

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<Configuration>(content);

                Shared.Logger.Logger.LogInformation($"Configuration for device identifier {deviceIdentifier} requested successfully");
                Shared.Logger.Logger.LogDebug($"Configuration Response: [{content}]");
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting configuration for device Id {deviceIdentifier}.", ex);

                Shared.Logger.Logger.LogError(exception);

                throw exception;
            }

            return response;
        }

        public async Task PostDiagnosticLogs(String deviceIdentifier,
                                             List<LogMessage> logMessages,
                                             CancellationToken cancellationToken)
        {
            String requestUri = this.BuildRequestUrl($"/logging/{deviceIdentifier}");

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
