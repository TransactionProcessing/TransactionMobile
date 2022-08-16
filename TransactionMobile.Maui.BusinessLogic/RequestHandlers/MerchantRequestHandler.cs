namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers;

using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Models;
using Requests;
using Services;

public class MerchantRequestHandler : IRequestHandler<GetContractProductsRequest, List<ContractProductModel>>, IRequestHandler<GetMerchantBalanceRequest, Decimal>
{
    #region Fields

    private readonly Func<Boolean,IMerchantService> MerchantServiceResolver;

    private readonly IApplicationCache ApplicationCache;
    
    #endregion

    #region Constructors
    public MerchantRequestHandler(Func<Boolean, IMerchantService> merchantServiceResolver,IApplicationCache applicationCache)
    {
        this.MerchantServiceResolver = merchantServiceResolver;
        this.ApplicationCache = applicationCache;
    }

    #endregion

    #region Methods

    public async Task<List<ContractProductModel>> Handle(GetContractProductsRequest request,
                                                         CancellationToken cancellationToken)
    {
        List<ContractProductModel> products = this.ApplicationCache.GetContractProducts();
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        IMerchantService merchantService = this.MerchantServiceResolver(useTrainingMode);

        if (products == null || products.Any() == false)
        {
            products = await merchantService.GetContractProducts(cancellationToken);
        }

        if (request.ProductType.HasValue)
        {
            products = products.Where(p => p.ProductType == request.ProductType).ToList();
        }

        return products;
    }

    public async Task<Decimal> Handle(GetMerchantBalanceRequest request,
                                      CancellationToken cancellationToken) {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        IMerchantService merchantService = this.MerchantServiceResolver(useTrainingMode);
        return await merchantService.GetMerchantBalance(cancellationToken);
    }


    #endregion
}