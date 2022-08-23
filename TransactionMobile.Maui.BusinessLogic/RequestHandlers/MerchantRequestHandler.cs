namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers;

using MediatR;
using Models;
using Requests;
using Services;

public class MerchantRequestHandler : IRequestHandler<GetContractProductsRequest, List<ContractProductModel>>,
                                      IRequestHandler<GetMerchantBalanceRequest, Decimal>,
                                      IRequestHandler<GetMerchantDetailsRequest, MerchantDetailsModel>
{
    #region Fields

    private readonly IApplicationCache ApplicationCache;

    private readonly Func<Boolean, IMerchantService> MerchantServiceResolver;

    #endregion

    #region Constructors

    public MerchantRequestHandler(Func<Boolean, IMerchantService> merchantServiceResolver,
                                  IApplicationCache applicationCache) {
        this.MerchantServiceResolver = merchantServiceResolver;
        this.ApplicationCache = applicationCache;
    }

    #endregion

    #region Methods

    public async Task<List<ContractProductModel>> Handle(GetContractProductsRequest request,
                                                         CancellationToken cancellationToken) {
        List<ContractProductModel> products = this.ApplicationCache.GetContractProducts();
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        IMerchantService merchantService = this.MerchantServiceResolver(useTrainingMode);

        if (products == null || products.Any() == false) {
            products = await merchantService.GetContractProducts(cancellationToken);
        }

        if (request.ProductType.HasValue) {
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

    public async Task<MerchantDetailsModel> Handle(GetMerchantDetailsRequest request,
                                                   CancellationToken cancellationToken) {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        IMerchantService merchantService = this.MerchantServiceResolver(useTrainingMode);

        MerchantDetailsModel merchantDetails = await merchantService.GetMerchantDetails(cancellationToken);

        return merchantDetails;
    }

    #endregion
}