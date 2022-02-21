namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;
using Models;

public class GetContractProductsRequest : IRequest<List<ContractProductModel>>
{
    #region Constructors

    private GetContractProductsRequest(String accessToken,
                                       Guid estateId,
                                       Guid merchantId,
                                       ProductType? productType)
    {
        this.AccessToken = accessToken;
        this.EstateId = estateId;
        this.MerchantId = merchantId;
        this.ProductType = productType;
    }

    #endregion

    #region Properties

    public String AccessToken { get; }

    public Guid EstateId { get; }

    public Guid MerchantId { get; }
    public ProductType? ProductType { get; }
    #endregion

    #region Methods

    public static GetContractProductsRequest Create(String accessToken,
                                                    Guid estateId,
                                                    Guid merchantId,
                                                    ProductType? productType)
    {
        return new GetContractProductsRequest(accessToken, estateId, merchantId,productType);
    }

    #endregion
}