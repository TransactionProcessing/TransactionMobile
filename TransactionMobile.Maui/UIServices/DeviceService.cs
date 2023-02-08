namespace TransactionMobile.Maui.UIServices;

using banditoth.MAUI.DeviceId.Interfaces;
using BusinessLogic.UIServices;
using Platforms.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceIdProvider DeviceIdProvider;

    public DeviceService(IDeviceIdProvider deviceIdProvider){
        this.DeviceIdProvider = deviceIdProvider;
    }

    public String GetIdentifier(){
        return this.DeviceIdProvider.GetDeviceId();
    }

    public String GetModel()
    {
        return DeviceInformationService.Model();
    }

    public String GetPlatform()
    {
        return DeviceInformationService.Platform();
    }
    
}