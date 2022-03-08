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
    [Fact]
    public async Task MerchantRequestHandler_GetContractProductsRequest_Handle_IsHandled()
    {
        Mock<IMerchantService> merchantService = new Mock<IMerchantService>();
        merchantService.Setup(m => m.GetContractProducts(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(TestData.ContractProductList);
        MerchantRequestHandler handler = new MerchantRequestHandler(merchantService.Object);

        GetContractProductsRequest request = GetContractProductsRequest.Create(TestData.Token,
                                                                               TestData.EstateId,
                                                                               TestData.MerchantId,
                                                                               null);

        List<ContractProductModel> contractProductModels = await handler.Handle(request, CancellationToken.None);

        contractProductModels.Count.ShouldBe(TestData.ContractProductList.Count);
    }

    [Fact]
    public async Task MerchantRequestHandler_GetMerchantBalanceRequest_Handle_IsHandled()
    {
        Mock<IMerchantService> merchantService = new Mock<IMerchantService>();
        merchantService.Setup(m => m.GetMerchantBalance(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(TestData.MerchantBalance);
        MerchantRequestHandler handler = new MerchantRequestHandler(merchantService.Object);

        GetMerchantBalanceRequest request = GetMerchantBalanceRequest.Create(TestData.Token,
                                                                             TestData.EstateId,
                                                                             TestData.MerchantId);

        Decimal merchantBalance = await handler.Handle(request, CancellationToken.None);

        merchantBalance.ShouldBe(TestData.MerchantBalance);
    }
}