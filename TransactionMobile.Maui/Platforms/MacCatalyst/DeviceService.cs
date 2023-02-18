using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.Platforms.Services
{
    using BusinessLogic.Common;
    using Newtonsoft.Json;

    public static partial class DeviceInformationService
    {
        public static partial String Model() => DeviceInfo.Model;

        public static partial String Platform() => $"{DeviceInfo.Platform} {DeviceInfo.VersionString}";
    }
}
