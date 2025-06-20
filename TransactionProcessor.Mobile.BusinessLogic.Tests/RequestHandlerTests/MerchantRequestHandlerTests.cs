using Moq;
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

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly MerchantRequestHandler MerchantRequestHandler;

    private readonly Mock<IMerchantService> MerchantService;

    private Func<Boolean, IMerchantService> MerchantServiceResolver;
    #endregion

    #region Constructors

    public MerchantRequestHandlerTests() {
        this.MerchantService = new Mock<IMerchantService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.MerchantService = new Mock<IMerchantService>();
        this.MerchantServiceResolver = (param) => {
                                           return this.MerchantService.Object;
                                       };
        this.MerchantRequestHandler = new MerchantRequestHandler(this.MerchantServiceResolver, this.ApplicationCache.Object);
    }

    #endregion

    #region Methods

    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_IsHandled() {
        this.MerchantService.Setup(m => m.GetContractProducts(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.ContractProductList));

        GetContractProductsRequest request = GetContractProductsRequest.Create();

        Result<List<ContractProductModel>> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Count.ShouldBe(TestData.ContractProductList.Count);
    }

    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_CacheIsNull_IsHandled(){
        List<ContractProductModel> products = null;
        this.ApplicationCache.Setup(a => a.GetContractProducts()).Returns(products);

        this.MerchantService.Setup(m => m.GetContractProducts(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.ContractProductList));

        GetContractProductsRequest request = GetContractProductsRequest.Create();

        Result<List<ContractProductModel>> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Count.ShouldBe(TestData.ContractProductList.Count);
    }

    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_GetContractProductsFailed_IsHandled()
    {
        List<ContractProductModel> products = new List<ContractProductModel>();
        this.ApplicationCache.Setup(a => a.GetContractProducts()).Returns(products);

        this.MerchantService.Setup(m => m.GetContractProducts(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("failed"));

        GetContractProductsRequest request = GetContractProductsRequest.Create();

        Result<List<ContractProductModel>> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_FilterByType_IsHandled()
    {
        this.MerchantService.Setup(m => m.GetContractProducts(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.ContractProductList));

        GetContractProductsRequest request = GetContractProductsRequest.Create(ProductType.Voucher);

        Result<List<ContractProductModel>> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Count.ShouldBe(1);
    }

    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_CacheIsEmpty_IsHandled()
    {
        List<ContractProductModel> products = new List<ContractProductModel>();
        this.ApplicationCache.Setup(a => a.GetContractProducts()).Returns(products);

        this.MerchantService.Setup(m => m.GetContractProducts(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.ContractProductList));

        GetContractProductsRequest request = GetContractProductsRequest.Create();

        Result<List<ContractProductModel>> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Count.ShouldBe(TestData.ContractProductList.Count);
    }

    [Fact]
    public async Task MerchantRequestHandler_GetMerchantBalanceRequest_Handle_IsHandled() {
        this.MerchantService.Setup(m => m.GetMerchantBalance(It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.MerchantBalance));

        GetMerchantBalanceRequest request = GetMerchantBalanceRequest.Create();

        Result<Decimal> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldBe(TestData.MerchantBalance);
    }

    [Fact]
    public async Task MerchantRequestHandler_GetMerchantDetailsRequest_Handle_IsHandled()
    {
        this.MerchantService.Setup(m => m.GetMerchantDetails(It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.MerchantDetailsModel));

        GetMerchantDetailsRequest request = GetMerchantDetailsRequest.Create();

        Result<MerchantDetailsModel> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    #endregion
}