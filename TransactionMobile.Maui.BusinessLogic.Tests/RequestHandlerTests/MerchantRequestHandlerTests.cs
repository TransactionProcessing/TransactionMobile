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
        Mock<IMemoryCacheService> memoryCacheService = new Mock<IMemoryCacheService>();
        

        MerchantRequestHandler handler = new MerchantRequestHandler(merchantServiceResolver, memoryCacheService.Object);

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
        Mock<IMemoryCacheService> memoryCacheService = new Mock<IMemoryCacheService>();
        MerchantRequestHandler handler = new MerchantRequestHandler(merchantServiceResolver, memoryCacheService.Object);

        GetMerchantBalanceRequest request = GetMerchantBalanceRequest.Create();

        Decimal merchantBalance = await handler.Handle(request, CancellationToken.None);

        merchantBalance.ShouldBe(TestData.MerchantBalance);
    }
}