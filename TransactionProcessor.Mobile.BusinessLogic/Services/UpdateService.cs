using System.Text;
using Newtonsoft.Json;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Services;

public interface IUpdateService
{
    Task<Result<ApplicationUpdateCheckResponse>> CheckForUpdates(String applicationVersion,
                                                                 String packageName,
                                                                 String platform,
                                                                 String deviceIdentifier,
                                                                 CancellationToken cancellationToken);
}

public class UpdateService : ClientProxyBase.ClientProxyBase, IUpdateService
{
    private readonly Func<String, String> BaseAddressResolver;

    public UpdateService(Func<String, String> baseAddressResolver,
                         HttpClient httpClient) : base(httpClient)
    {
        this.BaseAddressResolver = baseAddressResolver;
    }

    public async Task<Result<ApplicationUpdateCheckResponse>> CheckForUpdates(String applicationVersion,
                                                                              String packageName,
                                                                              String platform,
                                                                              String deviceIdentifier,
                                                                              CancellationToken cancellationToken)
    {
        String requestUri = this.BuildRequestUrl("/api/applicationupdates/check");
        var request = new
        {
            ApplicationVersion = applicationVersion,
            PackageName = packageName,
            Platform = platform,
            DeviceIdentifier = deviceIdentifier
        };

        try
        {
            Logger.LogInformation($"About to check for application updates for device identifier {deviceIdentifier}");
            Logger.LogDebug($"Application update request details: Uri {requestUri}");

            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, content, cancellationToken);
            Logger.LogDebug($"Application update response [{httpResponse.StatusCode}]");

            String responseContent = await this.HandleResponse(httpResponse, cancellationToken);
            Logger.LogDebug($"Application update response content [{responseContent}]");

            ApplicationUpdateCheckResponse? response = JsonConvert.DeserializeObject<ApplicationUpdateCheckResponse>(responseContent);

            if (response == null)
            {
                return Result.Failure("Application update check did not return a valid response.");
            }

            Logger.LogInformation($"Application update check for device identifier {deviceIdentifier} completed successfully");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error checking for application updates for device identifier {deviceIdentifier} {ex.Message}.", ex);
            return ResultExtensions.FailureExtended($"Error checking for application updates for device identifier {deviceIdentifier}", ex);
        }
    }

    private String BuildRequestUrl(String route)
    {
        String baseAddress = this.BaseAddressResolver("ApplicationUpdateServiceUrl");

        return $"{baseAddress.TrimEnd('/')}{route}";
    }
}
