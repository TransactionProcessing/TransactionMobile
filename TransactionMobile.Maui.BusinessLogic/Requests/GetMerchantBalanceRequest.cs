namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;

public class GetMerchantBalanceRequest : IRequest<Decimal>
{
    #region Constructors

    private GetMerchantBalanceRequest(String accessToken,
                                      Guid estateId,
                                      Guid merchantId)
    {
        this.AccessToken = accessToken;
        this.EstateId = estateId;
        this.MerchantId = merchantId;
    }

    #endregion

    #region Properties

    public String AccessToken { get; }

    public Guid EstateId { get; }

    public Guid MerchantId { get; }

    #endregion

    #region Methods

    public static GetMerchantBalanceRequest Create(String accessToken,
                                                   Guid estateId,
                                                   Guid merchantId)
    {
        return new GetMerchantBalanceRequest(accessToken, estateId, merchantId);
    }

    #endregion
}