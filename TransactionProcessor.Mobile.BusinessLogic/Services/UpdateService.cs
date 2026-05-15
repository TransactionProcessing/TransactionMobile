using Shared.Results;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Serialisation;

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
    private sealed record ApplicationUpdateCheckRequest(String ApplicationVersion,
                                                        String PackageName,
                                                        String Platform,
                                                        String DeviceIdentifier);

    private readonly Func<String, String> BaseAddressResolver;

    public UpdateService(Func<String, String> baseAddressResolver,
                         HttpClient httpClient, Func<Object, String> serialise,
                         Func<String, Type, Object> deserialise) : base(httpClient, serialise, deserialise)
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
        ApplicationUpdateCheckRequest request = new(applicationVersion, packageName, platform, deviceIdentifier);

        try
        {
            Logger.LogInformation($"About to check for application updates for device identifier {deviceIdentifier}");
            Logger.LogDebug($"Application update request details: Uri {requestUri}");

            Result<ApplicationUpdateCheckResponse>? result = await this.Post<ApplicationUpdateCheckRequest, ApplicationUpdateCheckResponse>(requestUri, request, cancellationToken);

            if (result.IsFailed) {
                return ResultHelpers.CreateFailure(result);
            }

            Logger.LogDebug($"Application update response content [{StringSerialiser.Serialise(result.Data)}]");
            
            Logger.LogInformation($"Application update check for device identifier {deviceIdentifier} completed successfully");
            return Result.Success(result.Data);
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
