namespace TransactionMobile.Maui.BusinessLogic.Tests.RequestHandlerTests;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shouldly;
using Xunit;

public class MerchantRequestHandlerTests
{
    #region Fields

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly MerchantRequestHandler MerchantRequestHandler;

    private readonly Mock<IMerchantService> MerchantService;

    #endregion

    #region Constructors

    public MerchantRequestHandlerTests() {
        this.MerchantService = new Mock<IMerchantService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.MerchantRequestHandler = new MerchantRequestHandler(this.MerchantService.Object, this.ApplicationCache.Object);
    }

    #endregion

    #region Methods

    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_IsHandled() {
        this.MerchantService.Setup(m => m.GetContractProducts(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SuccessResult<List<ContractProductModel>>(TestData.ContractProductList));

        GetContractProductsRequest request = GetContractProductsRequest.Create();

        Result<List<ContractProductModel>> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.Count.ShouldBe(TestData.ContractProductList.Count);
    }

    [Fact]
    public async Task MerchantRequestHandler_GetMerchantBalanceRequest_Handle_IsHandled() {
        this.MerchantService.Setup(m => m.GetMerchantBalance(It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<Decimal>(TestData.MerchantBalance));

        GetMerchantBalanceRequest request = GetMerchantBalanceRequest.Create();

        Result<Decimal> result = await this.MerchantRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.ShouldBe(TestData.MerchantBalance);
    }

    #endregion
}