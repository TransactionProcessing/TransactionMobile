using Microsoft.Extensions.Caching.Memory;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.RequestHandlers;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.RequestHandlerTests;

public class MerchantRequestHandlerTests
{
    #region Fields

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly MerchantRequestHandler MerchantRequestHandler;

    private readonly IMerchantServiceImposter MerchantService;

    private Func<Boolean, IMerchantService> MerchantServiceResolver;
    #endregion

    #region Constructors

    public MerchantRequestHandlerTests() {
        this.MerchantService = new IMerchantServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.MerchantService = new IMerchantServiceImposter();
        this.MerchantServiceResolver = (param) => {
                                           return this.MerchantService.Instance();
                                       };
        this.MerchantRequestHandler = new MerchantRequestHandler(this.MerchantServiceResolver, this.ApplicationCache.Instance());
    }

    #endregion

    #region Methods

    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_IsHandled() {
        this.MerchantService.GetContractProducts(Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(TestData.ContractProductList));

        MerchantQueries.GetContractProductsQuery request = new MerchantQueries.GetContractProductsQuery();

        Result<List<ContractProductModel>> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Count.ShouldBe(TestData.ContractProductList.Count);
    }

    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_CacheIsNull_ServiceIsCalled_ProductsAreCached(){
        List<ContractProductModel> products = null;
        this.ApplicationCache.GetContractProducts().Returns(products);

        this.MerchantService.GetContractProducts(Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(TestData.ContractProductList));

        MerchantQueries.GetContractProductsQuery request = new MerchantQueries.GetContractProductsQuery();

        Result<List<ContractProductModel>> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Count.ShouldBe(TestData.ContractProductList.Count);
        this.ApplicationCache.SetContractProducts(Arg<List<ContractProductModel>>.Any(), Arg<MemoryCacheEntryOptions>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_GetContractProductsFailed_IsHandled()
    {
        List<ContractProductModel> products = new List<ContractProductModel>();
        this.ApplicationCache.GetContractProducts().Returns(products);

        this.MerchantService.GetContractProducts(Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("failed"));

        MerchantQueries.GetContractProductsQuery request = new MerchantQueries.GetContractProductsQuery();

        Result<List<ContractProductModel>> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_FilterByType_IsHandled()
    {
        this.MerchantService.GetContractProducts(Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(TestData.ContractProductList));

        MerchantQueries.GetContractProductsQuery request = new MerchantQueries.GetContractProductsQuery(ProductType.Voucher);

        Result<List<ContractProductModel>> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Count.ShouldBe(1);
    }

    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_CacheIsEmpty_ServiceIsCalled_ProductsAreCached()
    {
        List<ContractProductModel> products = new List<ContractProductModel>();
        this.ApplicationCache.GetContractProducts().Returns(products);

        this.MerchantService.GetContractProducts(Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(TestData.ContractProductList));

        MerchantQueries.GetContractProductsQuery request = new MerchantQueries.GetContractProductsQuery();

        Result<List<ContractProductModel>> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Count.ShouldBe(TestData.ContractProductList.Count);
        this.ApplicationCache.SetContractProducts(Arg<List<ContractProductModel>>.Any(), Arg<MemoryCacheEntryOptions>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task MerchantRequestHandler_GetMerchantBalanceRequest_Handle_IsHandled() {
        this.MerchantService.GetMerchantBalance(Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.MerchantBalance));

        MerchantQueries.GetMerchantBalanceQuery request = new MerchantQueries.GetMerchantBalanceQuery();

        Result<Decimal> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldBe(TestData.MerchantBalance);
    }

    [Fact]
    public async Task MerchantRequestHandler_GetMerchantDetailsRequest_Handle_CacheMiss_ServiceIsCalled_DetailsAreCached()
    {
        this.ApplicationCache.GetMerchantDetails().Returns((MerchantDetailsModel)null);
        this.MerchantService.GetMerchantDetails(Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.MerchantDetailsModel));

        MerchantQueries.GetMerchantDetailsQuery request = new MerchantQueries.GetMerchantDetailsQuery();

        Result<MerchantDetailsModel> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        this.MerchantService.GetMerchantDetails(Arg<CancellationToken>.Any()).Called(Count.Once());
        this.ApplicationCache.SetMerchantDetails(Arg<MerchantDetailsModel>.Any(), Arg<MemoryCacheEntryOptions>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task MerchantRequestHandler_GetMerchantDetailsRequest_Handle_CacheHit_ServiceIsNotCalled()
    {
        this.ApplicationCache.GetMerchantDetails().Returns(TestData.MerchantDetailsModel);

        MerchantQueries.GetMerchantDetailsQuery request = new MerchantQueries.GetMerchantDetailsQuery();

        Result<MerchantDetailsModel> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        this.MerchantService.GetMerchantDetails(Arg<CancellationToken>.Any()).Called(Count.Never());
        this.ApplicationCache.SetMerchantDetails(Arg<MerchantDetailsModel>.Any(), Arg<MemoryCacheEntryOptions>.Any()).Called(Count.Never());
    }

    #endregion
}