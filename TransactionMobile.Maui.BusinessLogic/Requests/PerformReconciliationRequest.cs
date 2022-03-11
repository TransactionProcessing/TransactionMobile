using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Requests
{
    using MediatR;

    public class PerformReconciliationRequest : IRequest<Boolean>
    {
        public String DeviceIdentifier { get; private set; }
        public DateTime TransactionDateTime { get; private set; }
        public String ApplicationVersion { get; private set; }


        private PerformReconciliationRequest(DateTime transactionDateTime,
                                             String deviceIdentifier,
                                             String applicationVersion)
        {
            this.TransactionDateTime = transactionDateTime;
            this.DeviceIdentifier = deviceIdentifier;
            this.ApplicationVersion = applicationVersion;
        }

        public static PerformReconciliationRequest Create(DateTime transactionDateTime,
                                                          String deviceIdentifier,
                                                          String applicationVersion)
        {
            return new PerformReconciliationRequest(transactionDateTime, deviceIdentifier, applicationVersion);
        }
    }
}
