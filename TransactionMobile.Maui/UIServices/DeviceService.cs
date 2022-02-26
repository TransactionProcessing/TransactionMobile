namespace TransactionMobile.Maui.UIServices;

using BusinessLogic.UIServices;
using Platforms.Services;

public class DeviceService : IDeviceService
{
    private readonly DeviceInformationService DeviceInformationService;

    public DeviceService()
    {
        this.DeviceInformationService = new DeviceInformationService();
    }

    public String GetModel()
    {
        return this.DeviceInformationService.Model();
    }

    public String GetPlatform()
    {
        return this.DeviceInformationService.Platform();
    }

    public String GetIdentifier()
    {
        return this.DeviceInformationService.Identifier();
    }
}