namespace TransactionMobile.Maui.BusinessLogic.Tests.RequestHandlerTests;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Database;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shouldly;
using UIServices;
using Xunit;

public class TransactionRequestHandlerTests
{
    [Fact]
    public async Task TransactionRequestHandler_LogonTransactionRequest_Handle_IsHandled()
    {
        Mock<ITransactionService> transactionService = new Mock<ITransactionService>();
        Func<Boolean, ITransactionService> transactionServiceResolver = new Func<bool, ITransactionService>((param) =>
        {
            return transactionService.Object;
        });
        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
        Mock<IDeviceService> deviceService = new Mock<IDeviceService>();
        transactionService.Setup(t => t.PerformLogon(It.IsAny<PerformLogonRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.PerformLogonResponseModel);
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionServiceResolver, databaseContext.Object, applicationCache.Object,
                                                                          applicationInfoService.Object,
                                                                          deviceService.Object);

        LogonTransactionRequest request = LogonTransactionRequest.Create(TestData.TransactionDateTime);

        PerformLogonResponseModel? response = await handler.Handle(request, CancellationToken.None);

        response.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformMobileTopupRequest_Handle_IsHandled()
    {
        Mock<ITransactionService> transactionService = new Mock<ITransactionService>();
        Func<Boolean, ITransactionService> transactionServiceResolver = new Func<bool, ITransactionService>((param) =>
        {
            return transactionService.Object;
        });
        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        transactionService.Setup(t => t.PerformMobileTopup(It.IsAny<PerformMobileTopupRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
        Mock<IDeviceService> deviceService = new Mock<IDeviceService>();
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionServiceResolver, databaseContext.Object, applicationCache.Object,
                                                                          applicationInfoService.Object,
                                                                          deviceService.Object);

        PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(TestData.TransactionDateTime,
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
        Func<Boolean, ITransactionService> transactionServiceResolver = new Func<bool, ITransactionService>((param) =>
        {
            return transactionService.Object;
        });
        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
        Mock<IDeviceService> deviceService = new Mock<IDeviceService>();

        transactionService.Setup(t => t.PerformVoucherIssue(It.IsAny<PerformVoucherIssueRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionServiceResolver, databaseContext.Object, applicationCache.Object,
                                                                          applicationInfoService.Object,
                                                                          deviceService.Object);

        PerformVoucherIssueRequest request = PerformVoucherIssueRequest.Create(TestData.TransactionDateTime,
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

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentGetAccountRequest_Handle_IsHandled()
    {
        Mock<ITransactionService> transactionService = new Mock<ITransactionService>();
        Func<Boolean, ITransactionService> transactionServiceResolver = new Func<bool, ITransactionService>((param) =>
        {
            return transactionService.Object;
        });
        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
        Mock<IDeviceService> deviceService = new Mock<IDeviceService>();

        transactionService.Setup(t => t.PerformBillPaymentGetAccount(It.IsAny<PerformBillPaymentGetAccountModel>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(TestData.PerformBillPaymentGetAccountResponseModel);
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionServiceResolver, databaseContext.Object, applicationCache.Object,
                                                                          applicationInfoService.Object,
                                                                          deviceService.Object);

        PerformBillPaymentGetAccountRequest request = PerformBillPaymentGetAccountRequest.Create(TestData.TransactionDateTime,
                                                                                                 TestData.OperatorId1ContractId,
                                                                                                 TestData.Operator1Product_100KES.ProductId,
                                                                                                 TestData.OperatorIdentifier1,
                                                                                                 TestData.CustomerAccountNumber);

        PerformBillPaymentGetAccountResponseModel? response = await handler.Handle(request, CancellationToken.None);

        response.ShouldNotBeNull();
        response.BillDetails.ShouldNotBeNull();
        response.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentGetAccountRequest_GetAccountFailed_Handle_IsHandled()
    {
        Mock<ITransactionService> transactionService = new Mock<ITransactionService>();
        Func<Boolean, ITransactionService> transactionServiceResolver = new Func<bool, ITransactionService>((param) =>
        {
            return transactionService.Object;
        });
        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
        Mock<IDeviceService> deviceService = new Mock<IDeviceService>();

        transactionService.Setup(t => t.PerformBillPaymentGetAccount(It.IsAny<PerformBillPaymentGetAccountModel>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(TestData.PerformBillPaymentGetAccountResponseModelFailed);
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionServiceResolver, databaseContext.Object, applicationCache.Object,
                                                                          applicationInfoService.Object,
                                                                          deviceService.Object);

        PerformBillPaymentGetAccountRequest request = PerformBillPaymentGetAccountRequest.Create(TestData.TransactionDateTime,
                                                                                                 TestData.OperatorId1ContractId,
                                                                                                 TestData.Operator1Product_100KES.ProductId,
                                                                                                 TestData.OperatorIdentifier1,
                                                                                                 TestData.CustomerAccountNumber);

        PerformBillPaymentGetAccountResponseModel? response = await handler.Handle(request, CancellationToken.None);

        response.ShouldNotBeNull();
        response.BillDetails.ShouldBeNull();
        response.IsSuccessful.ShouldBeFalse();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentMakePaymentRequest_Handle_IsHandled()
    {
        Mock<ITransactionService> transactionService = new Mock<ITransactionService>();
        Func<Boolean, ITransactionService> transactionServiceResolver = new Func<bool, ITransactionService>((param) =>
        {
            return transactionService.Object;
        });
        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
        Mock<IDeviceService> deviceService = new Mock<IDeviceService>();

        transactionService.Setup(t => t.PerformBillPaymentMakePayment(It.IsAny<PerformBillPaymentMakePaymentModel>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true);
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionServiceResolver, databaseContext.Object, applicationCache.Object,
                                                                          applicationInfoService.Object,
                                                                          deviceService.Object);

        PerformBillPaymentMakePaymentRequest request = PerformBillPaymentMakePaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                   TestData.OperatorId1ContractId,
                                                                                                   TestData.Operator1Product_100KES.ProductId,
                                                                                                   TestData.OperatorIdentifier1,
                                                                                                   TestData.CustomerAccountNumber,
                                                                                                   TestData.CustomerAccountName,
                                                                                                   TestData.CustomerMobileNumber,
                                                                                                   TestData.PaymentAmount);

        Boolean response = await handler.Handle(request, CancellationToken.None);

        response.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentMakePaymentRequest_PaymentFailed_Handle_IsHandled()
    {
        Mock<ITransactionService> transactionService = new Mock<ITransactionService>();
        Func<Boolean, ITransactionService> transactionServiceResolver = new Func<bool, ITransactionService>((param) =>
        {
            return transactionService.Object;
        });
        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
        Mock<IDeviceService> deviceService = new Mock<IDeviceService>();

        transactionService.Setup(t => t.PerformBillPaymentMakePayment(It.IsAny<PerformBillPaymentMakePaymentModel>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(false);
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionServiceResolver, databaseContext.Object, applicationCache.Object,
                                                                          applicationInfoService.Object,
                                                                          deviceService.Object);

        PerformBillPaymentMakePaymentRequest request = PerformBillPaymentMakePaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                   TestData.OperatorId1ContractId,
                                                                                                   TestData.Operator1Product_100KES.ProductId,
                                                                                                   TestData.OperatorIdentifier1,
                                                                                                   TestData.CustomerAccountNumber,
                                                                                                   TestData.CustomerAccountName,
                                                                                                   TestData.CustomerMobileNumber,
                                                                                                   TestData.PaymentAmount);

        Boolean response = await handler.Handle(request, CancellationToken.None);

        response.ShouldBeFalse();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformReconciliationRequest_NoTransactions_Handle_IsHandled()
    {
        Mock<ITransactionService> transactionService = new Mock<ITransactionService>();
        Func<Boolean, ITransactionService> transactionServiceResolver = new Func<bool, ITransactionService>((param) =>
        {
            return transactionService.Object;
        });
        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
        Mock<IDeviceService> deviceService = new Mock<IDeviceService>();
        transactionService.Setup(t => t.PerformReconciliation(It.IsAny<PerformReconciliationRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        databaseContext.Setup(d => d.GetTransactions(It.IsAny<Boolean>())).ReturnsAsync(new List<TransactionRecord>());
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionServiceResolver, databaseContext.Object, applicationCache.Object,
                                                                          applicationInfoService.Object,
                                                                          deviceService.Object);

        PerformReconciliationRequest request = PerformReconciliationRequest.Create(TestData.TransactionDateTime,
                                                                                   TestData.DeviceIdentifier,
                                                                                   TestData.ApplicationVersion);

        Boolean response = await handler.Handle(request, CancellationToken.None);

        response.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformReconciliationRequest_TransactionsStored_Handle_IsHandled()
    {
        Mock<ITransactionService> transactionService = new Mock<ITransactionService>();
        Func<Boolean, ITransactionService> transactionServiceResolver = new Func<bool, ITransactionService>((param) =>
        {
            return transactionService.Object;
        });
        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
        Mock<IDeviceService> deviceService = new Mock<IDeviceService>();
        transactionService.Setup(t => t.PerformReconciliation(It.IsAny<PerformReconciliationRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        databaseContext.Setup(d => d.GetTransactions(It.IsAny<Boolean>())).ReturnsAsync(TestData.StoredTransactions);
        TransactionRequestHandler handler = new TransactionRequestHandler(transactionServiceResolver, databaseContext.Object, applicationCache.Object,
                                                                          applicationInfoService.Object,
                                                                          deviceService.Object);

        PerformReconciliationRequest request = PerformReconciliationRequest.Create(TestData.TransactionDateTime,
                                                                                   TestData.DeviceIdentifier,
                                                                                   TestData.ApplicationVersion);

        Boolean response = await handler.Handle(request, CancellationToken.None);

        response.ShouldBeTrue();
    }
}
