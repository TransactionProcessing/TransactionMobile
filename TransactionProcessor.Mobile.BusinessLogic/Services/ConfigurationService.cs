using System.Text;
using Shared.Results;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Serialisation;
using TransactionProcessor.Mobile.BusinessLogic.Services.DataTransferObjects;

namespace TransactionProcessor.Mobile.BusinessLogic.Services;
public interface IConfigurationService
{
    Task<Result<Configuration>> GetConfiguration(String deviceIdentifier,
                                                 CancellationToken cancellationToken);

    Task PostDiagnosticLogs(String deviceIdentifier,
                            List<LogMessage> logMessages,
                            CancellationToken cancellationToken);
}

public class ConfigurationService : ClientProxyBase.ClientProxyBase, IConfigurationService
{
    private readonly Func<String, String> BaseAddressResolver;

    public ConfigurationService(Func<String, String> baseAddressResolver,
                                HttpClient httpClient, Func<Object,String> serialise, Func<String,Type, Object> deserialise) : base(httpClient, serialise, deserialise) {
        this.BaseAddressResolver = baseAddressResolver;
    }


    private String BuildRequestUrl(String route)
    {
        String baseAddress = this.BaseAddressResolver("ConfigServiceUrl");

        String requestUri = $"{baseAddress}{route}";

        return requestUri;
    }

    //protected override async Task<String> HandleResponse(HttpResponseMessage responseMessage,
    //                                                     CancellationToken cancellationToken)
    //{
    //    String content = await responseMessage.Content.ReadAsStringAsync();

    //    if (responseMessage.StatusCode == HttpStatusCode.NotFound)
    //    {
    //        // No error as maybe running under CI (which has no internet)
    //        return content;
    //    }

    //    return await base.HandleResponse(responseMessage, cancellationToken);
    //}

    public async Task<Result<Configuration>> GetConfiguration(String deviceIdentifier,
                                                              CancellationToken cancellationToken)
    {
        Configuration response = null;
        String requestUri = this.BuildRequestUrl($"/api/transactionmobileconfiguration/{deviceIdentifier}");

        try
        {
            Logger.LogInformation($"About to request configuration for device identifier {deviceIdentifier}");
            Logger.LogDebug($"Configuration Request details: Uri {requestUri}");
            
            // call was successful so now deserialise the body to the response object
            Result<ConfigurationResponse>? apiResponse = await this.Get<ConfigurationResponse>(requestUri, cancellationToken);
            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);

            Logger.LogDebug($"About to build Configuration");
            response = new Configuration() {
                ClientSecret = apiResponse.Data.ClientSecret,
                ClientId = apiResponse.Data.ClientId,
                ApplicationUpdateUri = apiResponse.Data.ApplicationUpdateUri,
                EnableAutoUpdates = apiResponse.Data.EnableAutoUpdates,
                SecurityServiceUri = apiResponse.Data.HostAddresses.Single(h => h.ServiceType == ServiceType.Security).Uri,
                TransactionProcessorAclUri =
                    apiResponse.Data.HostAddresses.Single(h => h.ServiceType == ServiceType.TransactionProcessorAcl).Uri,
                LogMessageBatchSize = apiResponse.Data.LogMessageBatchSize.GetValueOrDefault(),
                SentryDsn = apiResponse.Data.SentryDsn ?? String.Empty,
            };

            Logger.LogDebug($"About to xlate log level");
            response.LogLevel = apiResponse.Data.LogLevel switch
            {
                LoggingLevel.Debug => LogLevel.Debug,
                LoggingLevel.Error => LogLevel.Error,
                LoggingLevel.Fatal => LogLevel.Fatal,
                LoggingLevel.Information => LogLevel.Info,
                LoggingLevel.Trace => LogLevel.Trace,
                LoggingLevel.Warning => LogLevel.Warn,
                _ => LogLevel.Info
            };

            Logger.LogInformation($"Configuration for device identifier {deviceIdentifier} requested successfully");
            Logger.LogDebug($"Configuration Response: [{StringSerialiser.Serialise(apiResponse.Data)}]");

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            // An exception has occurred, add some additional information to the message
            Logger.LogError($"Error getting configuration for device Id {deviceIdentifier} {ex.Message}.",ex);

            return ResultExtensions.FailureExtended($"Error getting configuration data for device Id {deviceIdentifier}", ex);
        }
    }

    public async Task PostDiagnosticLogs(String deviceIdentifier,
                                         List<LogMessage> logMessages,
                                         CancellationToken cancellationToken)
    {
        String requestUri = this.BuildRequestUrl($"/transactionmobilelogging/{deviceIdentifier}");

        // Create a container
        var container = new
        {
            messages = logMessages
        };
        StringContent content = new(StringSerialiser.Serialise(container), Encoding.UTF8, "application/json");

        Result? result = await this.Post(requestUri, content, cancellationToken);

        // TODO: return the result to the caller so that we can retry if it fails (and also log any errors that occur here)
    }
}
