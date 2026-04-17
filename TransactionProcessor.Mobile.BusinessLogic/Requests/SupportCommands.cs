using MediatR;
using SimpleResults;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public record SupportCommands {
    public record UploadLogsCommand(String DeviceIdentifier) : IRequest<Result>;
}