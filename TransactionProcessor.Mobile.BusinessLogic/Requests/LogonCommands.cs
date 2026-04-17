using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public record LogonCommands {
    public record GetTokenCommand(String UserName, String Password) : IRequest<Result<TokenResponseModel>>;

    public record RefreshTokenCommand(String RefreshToken) : IRequest<Result<TokenResponseModel>>;
}