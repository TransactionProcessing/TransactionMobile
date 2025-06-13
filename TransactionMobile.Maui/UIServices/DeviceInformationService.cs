using System;
using Microsoft.Maui.Devices;

namespace TransactionMobile.Maui.Platforms.Services;

public static partial class DeviceInformationService
{
    #region Methods
    public static partial String Model();

    public static partial String Platform();
    
    #endregion
}

public static partial class DeviceOrientationService
{
    #region Methods

    public static partial void SetDeviceOrientation(DisplayOrientation displayOrientation);

    #endregion
}