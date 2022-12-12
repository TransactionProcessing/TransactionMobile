namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers;

using MediatR;
using Models;
using Requests;
using Services;

public class MerchantRequestHandler : IRequestHandler<GetContractProductsRequest, Result<List<ContractProductModel>>>,
                                      IRequestHandler<GetMerchantBalanceRequest, Result<Decimal>>,
                                      IRequestHandler<GetMerchantDetailsRequest, Result<MerchantDetailsModel>>
{
    #region Fields

    private readonly IApplicationCache ApplicationCache;

    private readonly IMerchantService MerchantService;

    #endregion

    #region Constructors

    public MerchantRequestHandler(IMerchantService merchantService,
                                  IApplicationCache applicationCache) {
        this.MerchantService = merchantService;
        this.ApplicationCache = applicationCache;
    }

    #endregion

    #region Methods

    public async Task<Result<List<ContractProductModel>>> Handle(GetContractProductsRequest request,
                                                                 CancellationToken cancellationToken) {
        List<ContractProductModel> products = this.ApplicationCache.GetContractProducts();
        
        if (products == null || products.Any() == false) {
            Result<List<ContractProductModel>> getProductsResult = await this.MerchantService.GetContractProducts(cancellationToken);
            if (getProductsResult.Success) {
                products = getProductsResult.Data;
            }
            else {
                return getProductsResult;
            }
        }

        if (request.ProductType.HasValue) {
            products = products.Where(p => p.ProductType == request.ProductType).ToList();
        }

        return new SuccessResult<List<ContractProductModel>>(products);
    }

    public async Task<Result<Decimal>> Handle(GetMerchantBalanceRequest request,
                                              CancellationToken cancellationToken) {
        
        return await this.MerchantService.GetMerchantBalance(cancellationToken);
    }

    public async Task<Result<MerchantDetailsModel>> Handle(GetMerchantDetailsRequest request,
                                                   CancellationToken cancellationToken) {
        Result<MerchantDetailsModel> merchantDetails = await this.MerchantService.GetMerchantDetails(cancellationToken);

        return merchantDetails;
    }

    #endregion
}