using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public record SupportQueries {
    public record ViewLogsQuery() : IRequest<Result<List<LogMessage>>>;
}