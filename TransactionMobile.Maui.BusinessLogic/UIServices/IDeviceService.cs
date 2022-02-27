using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.UIServices
{
    public interface IDeviceService
    {
        String GetModel();

        String GetPlatform();

        String GetIdentifier();
    }
}
