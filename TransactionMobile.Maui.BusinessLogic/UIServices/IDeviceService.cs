using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.UIServices
{
    using Microsoft.Maui.Devices;

    public interface IDeviceService
    {
        String GetIdentifier();

        String GetModel();

        String GetPlatform();

        String GetManufacturer();

        void SetOrientation(DisplayOrientation displayOrientation);
    }
}
