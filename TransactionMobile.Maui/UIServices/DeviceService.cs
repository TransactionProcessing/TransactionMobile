namespace TransactionMobile.Maui.UIServices;

using BusinessLogic.UIServices;
using Platforms.Services;

public class DeviceService : IDeviceService
{
    public String GetModel()
    {
        return DeviceInformationService.Model();
    }

    public String GetPlatform()
    {
        return DeviceInformationService.Platform();
    }

    public String GetIdentifier()
    {
        return DeviceInformationService.Identifier();
    }
}