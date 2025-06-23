using MediatR;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests
{
    public class UploadLogsRequest : IRequest<Boolean>
    {
        private UploadLogsRequest(String deviceIdentifier)
        {
            this.DeviceIdentifier = deviceIdentifier;
        }

        public string DeviceIdentifier { get; }

        public static UploadLogsRequest Create(String deviceIdentifier)
        {
            return new UploadLogsRequest(deviceIdentifier);
        }
    }
}
