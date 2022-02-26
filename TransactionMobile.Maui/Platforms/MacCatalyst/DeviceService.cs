using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.Platforms.Services
{
    public partial class DeviceInformationService
    {
        public partial String Model() => DeviceInfo.Model;

        public partial String Platform() => $"{DeviceInfo.Platform} {DeviceInfo.VersionString}";
        
        public partial String DeviceIdentifier()
        {
            return "<Unknown>";
        }
    }
}
