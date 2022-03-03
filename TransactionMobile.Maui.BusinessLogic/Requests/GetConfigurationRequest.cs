namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;
using Models;

public class GetConfigurationRequest : IRequest<Configuration>
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