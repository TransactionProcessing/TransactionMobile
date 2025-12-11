using banditoth.MAUI.DeviceId.Interfaces;
using Microsoft.Maui.Devices;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using Orientation = TransactionProcessor.Mobile.BusinessLogic.UIServices.Orientation;

namespace TransactionProcessor.Mobile.UIServices;

public class DeviceService : IDeviceService
{
    private readonly IDeviceIdProvider DeviceIdProvider;

    public DeviceService(IDeviceIdProvider deviceIdProvider){
        this.DeviceIdProvider = deviceIdProvider;
    }

    public String GetIdentifier(){

#if WINDOWS
        Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation deviceInformation = new();
        String Id = deviceInformation.Id.ToString();
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