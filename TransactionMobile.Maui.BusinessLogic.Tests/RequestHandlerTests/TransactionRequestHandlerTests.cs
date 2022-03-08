﻿namespace TransactionMobile.Maui.BusinessLogic.Tests.RequestHandlerTests;

using System;
using System.Threading;
using System.Threading.Tasks;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shouldly;
using Xunit;

public class TransactionRequestHandlerTests
{
    [Fact]
    public async Task TransactionRequestHandler_LogonTransactionRequest_Handle_IsHandled()
    {
        Mock<ITransactionService> transactionService = new Mock<ITransactionService>();
        transactionService.Setup(t => t.PerformLogon(It.IsAny<PerformLogonRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.PerformLogonResponseModel);
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionService.Object);

        LogonTransactionRequest request = LogonTransactionRequest.Create(TestData.TransactionDateTime,
                                                                         TestData.TransactionNumber,
                                                                         TestData.DeviceIdentifier,
                                                                         TestData.ApplicationVersion);

        PerformLogonResponseModel? response = await handler.Handle(request, CancellationToken.None);

        response.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformMobileTopupRequest_Handle_IsHandled()
    {
        Mock<ITransactionService> transactionService = new Mock<ITransactionService>();
        transactionService.Setup(t => t.PerformMobileTopup(It.IsAny<PerformMobileTopupRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionService.Object);

        PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(TestData.TransactionDateTime,
                                                                             TestData.TransactionNumber,
                                                                             TestData.DeviceIdentifier,
                                                                             TestData.ApplicationVersion,
                                                                             TestData.OperatorId1ContractId,
                                                                             TestData.Operator1Product_100KES.ProductId,
                                                                             TestData.OperatorIdentifier1,
                                                                             TestData.CustomerAccountNumber,
                                                                             TestData.Operator1Product_100KES.Value,
                                                                             TestData.CustomerEmailAddress);

        Boolean response = await handler.Handle(request, CancellationToken.None);

        response.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformVoucherIssueRequest_Handle_IsHandled()
    {
        Mock<ITransactionService> transactionService = new Mock<ITransactionService>();
        transactionService.Setup(t => t.PerformVoucherIssue(It.IsAny<PerformVoucherIssueRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionService.Object);

        PerformVoucherIssueRequest request = PerformVoucherIssueRequest.Create(TestData.TransactionDateTime,
                                                                               TestData.TransactionNumber,
                                                                               TestData.DeviceIdentifier,
                                                                               TestData.ApplicationVersion,
                                                                               TestData.OperatorId3ContractId,
                                                                               TestData.Operator3Product_200KES.ProductId,
                                                                               TestData.OperatorIdentifier3,
                                                                               TestData.RecipientMobileNumber,
                                                                               TestData.RecipientEmailAddress,
                                                                               TestData.Operator3Product_200KES.Value,
                                                                               TestData.CustomerEmailAddress);

        Boolean response = await handler.Handle(request, CancellationToken.None);

        response.ShouldBeTrue();
    }
}