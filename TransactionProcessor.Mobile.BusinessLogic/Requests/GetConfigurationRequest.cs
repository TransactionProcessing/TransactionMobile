using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public class GetConfigurationRequest : IRequest<Result<Configuration>>
{
    public String DeviceIdentifier { get; private set; }

    private GetConfigurationRequest(String deviceIdentifier)
    {
        this.DeviceIdentifier = deviceIdentifier;
    }

    public static GetConfigurationRequest Create(String deviceIdentifier)
    {
        return new GetConfigurationRequest(deviceIdentifier);
    }
}