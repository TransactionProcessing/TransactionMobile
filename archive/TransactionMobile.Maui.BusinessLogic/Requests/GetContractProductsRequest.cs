namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using Models;
using RequestHandlers;
using SimpleResults;

public class GetContractProductsRequest : IRequest<Result<List<ContractProductModel>>>
{
    #region Constructors

    private GetContractProductsRequest(ProductType? productType)
    {
        this.ProductType = productType;
    }

    #endregion

    #region Properties
    public ProductType? ProductType { get; }
    #endregion

    #region Methods

    public static GetContractProductsRequest Create(ProductType? productType =null)
    {
        return new GetContractProductsRequest(productType);
    }

    #endregion
}