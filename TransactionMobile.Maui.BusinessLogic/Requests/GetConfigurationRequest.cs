namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using Models;
using RequestHandlers;

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