namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;
using Models;

public class GetContractProductsRequest : IRequest<List<ContractProductModel>>
{
    #region Constructors

    private GetContractProductsRequest(String accessToken,
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

    public static GetContractProductsRequest Create(String accessToken,
                                                    Guid estateId,
                                                    Guid merchantId)
    {
        return new GetContractProductsRequest(accessToken, estateId, merchantId);
    }

    #endregion
}