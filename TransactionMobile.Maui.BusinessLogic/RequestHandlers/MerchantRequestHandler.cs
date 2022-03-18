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

    private readonly IMerchantService MerchantService;

    private readonly IMemoryCacheService MemoryCacheService;

    #endregion

    #region Constructors
    public MerchantRequestHandler(IMerchantService merchantService,IMemoryCacheService memoryCacheService)
    {
        this.MerchantService = merchantService;
        this.MemoryCacheService = memoryCacheService;
    }

    #endregion

    #region Methods

    public async Task<List<ContractProductModel>> Handle(GetContractProductsRequest request,
                                                         CancellationToken cancellationToken)
    {
        this.MemoryCacheService.TryGetValue<List<ContractProductModel>>("ContractProducts", out List<ContractProductModel> products);
        
        if (products == null || products.Any() == false)
        {
            products = await this.MerchantService.GetContractProducts(cancellationToken);

            this.CacheContractData(products);
        }

        if (request.ProductType.HasValue)
        {
            products = products.Where(p => p.ProductType == request.ProductType).ToList();
        }

        return products;
    }

    public async Task<Decimal> Handle(GetMerchantBalanceRequest request,
                                      CancellationToken cancellationToken)
    {
        return await this.MerchantService.GetMerchantBalance(cancellationToken);
    }

    private void CacheContractData(List<ContractProductModel> contractProductModels)
    {
        DateTime expirationTime = DateTime.Now.AddMinutes(60);
        CancellationChangeToken expirationToken = new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMinutes(60)).Token);
        MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                                                    // Pin to cache.
                                                    .SetPriority(CacheItemPriority.NeverRemove)
                                                    // Set the actual expiration time
                                                    .SetAbsoluteExpiration(expirationTime)
                                                    // Force eviction to run
                                                    .AddExpirationToken(expirationToken);

        this.MemoryCacheService.Set("ContractProducts", contractProductModels, cacheEntryOptions);
    }

    #endregion
}