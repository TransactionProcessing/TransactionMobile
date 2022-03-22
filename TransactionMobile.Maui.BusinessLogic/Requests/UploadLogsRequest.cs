using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Requests
{

    public class UploadLogsRequest : IRequest<Boolean>
    {
        private UploadLogsRequest(String deviceIdentifier)
        {
            DeviceIdentifier = deviceIdentifier;
        }

        public string DeviceIdentifier { get; }

        public static UploadLogsRequest Create(String deviceIdentifier)
        {
            return new UploadLogsRequest(deviceIdentifier);
        }
    }
}
