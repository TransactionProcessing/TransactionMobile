using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public record LogonQueries {
    public record GetConfigurationQuery(String DeviceIdentifier) : IRequest<Result<Configuration>>;
}