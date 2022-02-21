namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers;

using MediatR;
using Models;
using Requests;
using Services;

public class MerchantRequestHandler : IRequestHandler<GetContractProductsRequest, List<ContractProductModel>>, IRequestHandler<GetMerchantBalanceRequest, Decimal>
{
    #region Fields

    private readonly IMerchantService MerchantService;

    #endregion

    #region Constructors
    public MerchantRequestHandler(IMerchantService merchantService)
    {
        this.MerchantService = merchantService;
    }

    #endregion

    #region Methods

    public async Task<List<ContractProductModel>> Handle(GetContractProductsRequest request,
                                                         CancellationToken cancellationToken)
    {
        List<ContractProductModel> products = await this.MerchantService.GetContractProducts(request.AccessToken, request.EstateId, request.MerchantId, cancellationToken);

        if (request.ProductType.HasValue)
        {
            products = products.Where(p => p.ProductType == request.ProductType).ToList();
        }

        return products;
    }

    public async Task<Decimal> Handle(GetMerchantBalanceRequest request,
                                      CancellationToken cancellationToken)
    {
        return await this.MerchantService.GetMerchantBalance(request.AccessToken, request.EstateId, request.MerchantId, cancellationToken);
    }

    #endregion
}