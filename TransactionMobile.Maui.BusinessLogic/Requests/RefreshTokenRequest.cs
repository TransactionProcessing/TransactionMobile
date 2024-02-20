namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using Models;
using RequestHandlers;

public class RefreshTokenRequest : IRequest<Result<TokenResponseModel>>
{
    #region Constructors

    private RefreshTokenRequest(String refreshToken)
    {
        this.RefreshToken = refreshToken;
    }

    #endregion

    #region Properties
        
    public String RefreshToken { get; }

    #endregion

    #region Methods

    public static RefreshTokenRequest Create(String refreshToken)
    {
        return new RefreshTokenRequest(refreshToken);
    }

    #endregion
}