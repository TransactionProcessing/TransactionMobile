using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests
{
    public class PerformReconciliationRequest : IRequest<Result<PerformReconciliationResponseModel>>
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
