using MediatR;
using Shared.Results;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.RequestHandlers;

public class MerchantRequestHandler : IRequestHandler<GetContractProductsRequest, Result<List<ContractProductModel>>>,
                                      IRequestHandler<GetMerchantBalanceRequest, Result<Decimal>>,
                                      IRequestHandler<GetMerchantDetailsRequest, Result<MerchantDetailsModel>>,
                                      IRequestHandler<GetProductOperators, Result<List<ContractOperatorModel>>>
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

    public async Task<Result<List<ContractProductModel>>> Handle(GetContractProductsRequest request,
                                                                 CancellationToken cancellationToken) {
        List<ContractProductModel> products = this.ApplicationCache.GetContractProducts();

        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        IMerchantService merchantService = this.MerchantServiceResolver(useTrainingMode);

        if (products == null || products.Any() == false) {
            Result<List<ContractProductModel>> getProductsResult = await merchantService.GetContractProducts(cancellationToken);
            if (getProductsResult.IsSuccess) {
                products = getProductsResult.Data;
            }
            else {
                return getProductsResult;
            }
        }

        if (request.ProductType.HasValue) {
            products = products.Where(p => p.ProductType == request.ProductType).ToList();
        }

        return Result.Success(products);
    }

    public async Task<Result<Decimal>> Handle(GetMerchantBalanceRequest request,
                                              CancellationToken cancellationToken) {

        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        IMerchantService merchantService = this.MerchantServiceResolver(useTrainingMode);

        return await merchantService.GetMerchantBalance(cancellationToken);
    }

    public async Task<Result<MerchantDetailsModel>> Handle(GetMerchantDetailsRequest request,
                                                   CancellationToken cancellationToken) {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        IMerchantService merchantService = this.MerchantServiceResolver(useTrainingMode);

        Result<MerchantDetailsModel> merchantDetails = await merchantService.GetMerchantDetails(cancellationToken);

        return merchantDetails;
    }

    #endregion

    public async Task<Result<List<ContractOperatorModel>>> Handle(GetProductOperators request,
                                                                  CancellationToken cancellationToken) {
        List<ContractProductModel> products = this.ApplicationCache.GetContractProducts();

        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        IMerchantService merchantService = this.MerchantServiceResolver(useTrainingMode);

        if (products == null || products.Any() == false)
        {
            Result<List<ContractProductModel>> getProductsResult = await merchantService.GetContractProducts(cancellationToken);
            if (getProductsResult.IsSuccess)
            {
                products = getProductsResult.Data;

                if (request.ProductType.HasValue)
                {
                    products = products.Where(p => p.ProductType == request.ProductType).ToList();
                }
            }
            
            return ResultHelpers.CreateFailure(getProductsResult);
        }

        List<ContractOperatorModel> operators = products.GroupBy(c => new
        {
            c.OperatorName,
            c.OperatorId,
            c.OperatorIdentfier
        }).Select(g => new ContractOperatorModel
        {
            OperatorId = g.Key.OperatorId,
            OperatorName = g.Key.OperatorName,
            OperatorIdentfier = g.Key.OperatorIdentfier
        }).ToList();

        return Result.Success(operators);

    }
}