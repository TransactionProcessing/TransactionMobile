namespace TransactionMobile.Maui.BusinessLogic.Tests.RequestHandlerTests;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
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
        Func<Boolean, IMerchantService> merchantServiceResolver = new Func<bool, IMerchantService>((param) =>
        {
            return merchantService.Object;
        });
        
        merchantService.Setup(m => m.GetContractProducts(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(TestData.ContractProductList);
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        
        MerchantRequestHandler handler = new MerchantRequestHandler(merchantServiceResolver, applicationCache.Object);

        GetContractProductsRequest request = GetContractProductsRequest.Create();

        List<ContractProductModel> contractProductModels = await handler.Handle(request, CancellationToken.None);

        contractProductModels.Count.ShouldBe(TestData.ContractProductList.Count);
    }

    [Fact]
    public async Task MerchantRequestHandler_GetMerchantBalanceRequest_Handle_IsHandled()
    {
        Mock<IMerchantService> merchantService = new Mock<IMerchantService>();
        Func<Boolean, IMerchantService> merchantServiceResolver = new Func<bool, IMerchantService>((param) =>
        {
            return merchantService.Object;
        });
        merchantService.Setup(m => m.GetMerchantBalance(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(TestData.MerchantBalance);
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        MerchantRequestHandler handler = new MerchantRequestHandler(merchantServiceResolver, applicationCache.Object);

        GetMerchantBalanceRequest request = GetMerchantBalanceRequest.Create();

        Decimal merchantBalance = await handler.Handle(request, CancellationToken.None);

        merchantBalance.ShouldBe(TestData.MerchantBalance);
    }
}