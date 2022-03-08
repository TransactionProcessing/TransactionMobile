namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;
using Models;

public class RefreshTokenRequest : IRequest<TokenResponseModel>
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