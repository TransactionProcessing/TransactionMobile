using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;

namespace TransactionMobile.Maui.Platforms.Services
{
    public static partial class DeviceInformationService
    {
        public static partial String Model() => new EasClientDeviceInformation().SystemProductName;
        
        public static partial String Platform() => $"UWP {GetVersionString()}";
        
        private static string GetVersionString()
        {
            var version = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;

            if (ulong.TryParse(version, out var v))
            {
                var v1 = (v & 0xFFFF000000000000L) >> 48;
                var v2 = (v & 0x0000FFFF00000000L) >> 32;
                var v3 = (v & 0x00000000FFFF0000L) >> 16;
                var v4 = v & 0x000000000000FFFFL;
                return $"{v1}.{v2}.{v3}.{v4}";
            }

            return version;
        }
        

        public static partial String Identifier()
        {
            var deviceInformation = new EasClientDeviceInformation();
            string Id = deviceInformation.Id.ToString();
        }
    }
}
