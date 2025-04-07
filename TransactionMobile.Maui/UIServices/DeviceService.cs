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

#if WINDOWS
        var deviceInformation = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
        string Id = deviceInformation.Id.ToString();
        return Id.Replace("-","");;
#endif

        return this.DeviceIdProvider.GetDeviceId().Replace("-","");
    }

    public String GetModel()
    {
        return DeviceInformationService.Model();
    }

    public String GetPlatform()
    {
        return DeviceInformationService.Platform();
    }

    public Boolean IsIOS() => DeviceInfo.Platform == DevicePlatform.iOS;
    
    public String GetManufacturer(){
        return DeviceInfo.Manufacturer;
    }

    public void SetOrientation(Orientation displayOrientation){
        DisplayOrientation result = displayOrientation switch
        {
            Orientation.Portrait => DisplayOrientation.Portrait,
            Orientation.Landscape => DisplayOrientation.Landscape,
            _ => DisplayOrientation.Unknown
        };
        DeviceOrientationService.SetDeviceOrientation(result);
    }
}