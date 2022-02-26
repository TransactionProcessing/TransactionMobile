using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Support
{
    using MvvmHelpers;
    using UIServices;

    public class SupportPageViewModel : BaseViewModel
    {
        private readonly IDeviceService DeviceService;

        public SupportPageViewModel(IDeviceService deviceService)
        {
            this.DeviceService = deviceService;
        }
        public string AppVersion => $"Version: {AppInfo.VersionString}{Environment.NewLine}Copyright © 2022 Stuart Ferguson";

        //public string Description => "A playground for experiments with .Net MAUI. All code is available on GitHub and development is documented on my blog 'Sailing the Sharp Sea'.";

        public string Platform
        {
            get
            {
                StringBuilder platform = new();
                platform.Append("Platform: ").AppendLine(this.DeviceService.GetPlatform());
                platform.Append("Manufacturer: ").AppendLine(DeviceInfo.Manufacturer);
                platform.Append("Device: ").AppendLine(this.DeviceService.GetModel());

                return platform.ToString();
            }
        }

    }
}
