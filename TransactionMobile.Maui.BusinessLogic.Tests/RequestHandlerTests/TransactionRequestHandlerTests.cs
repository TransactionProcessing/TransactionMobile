namespace TransactionMobile.Maui.BusinessLogic.Tests.RequestHandlerTests;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Database;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shouldly;
using TransactionProcessorACL.DataTransferObjects.Responses;
using UIServices;
using Xunit;
using static SQLite.SQLite3;

public class TransactionRequestHandlerTests
{
    private Mock<ITransactionService> TransactionService;

    private Mock<IDatabaseContext> DatabaseContext;

    private Mock<IApplicationCache> ApplicationCache;

    private Mock<IApplicationInfoService> ApplicationInfoService;

    private Mock<IDeviceService> DeviceService;

    private TransactionRequestHandler TransactionRequestHandler;

    public TransactionRequestHandlerTests() {
        this.TransactionService = new Mock<ITransactionService>();
        this.DatabaseContext = new Mock<IDatabaseContext>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.ApplicationInfoService = new Mock<IApplicationInfoService>();
        this.DeviceService = new Mock<IDeviceService>();

        this.TransactionRequestHandler = new TransactionRequestHandler(this.TransactionService.Object, 
                                                                          this.DatabaseContext.Object, 
                                                                          this.ApplicationCache.Object,
                                                                          this.ApplicationInfoService.Object,
                                                                          this.DeviceService.Object);

    }

    [Fact]
    public async Task TransactionRequestHandler_LogonTransactionRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformLogon(It.IsAny<PerformLogonRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(
         new SuccessResult<PerformLogonResponseModel>(TestData.PerformLogonResponseModel));
     
        LogonTransactionRequest request = LogonTransactionRequest.Create(TestData.TransactionDateTime);

        Result<PerformLogonResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformMobileTopupRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformMobileTopup(It.IsAny<PerformMobileTopupRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<SaleTransactionResponseMessage>(new SaleTransactionResponseMessage {
            ResponseCode = "0000"
        }));
     
        PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(TestData.TransactionDateTime,
                                                                             TestData.OperatorId1ContractId,
                                                                             TestData.Operator1Product_100KES.ProductId,
                                                                             TestData.OperatorIdentifier1,
                                                                             TestData.CustomerAccountNumber,
                                                                             TestData.Operator1Product_100KES.Value,
                                                                             TestData.CustomerEmailAddress);

        Result<SaleTransactionResponseMessage> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.IsSuccessfulTransaction().ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformVoucherIssueRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformVoucherIssue(It.IsAny<PerformVoucherIssueRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<SaleTransactionResponseMessage>(new SaleTransactionResponseMessage
            {
                ResponseCode = "0000"
            }));

        PerformVoucherIssueRequest request = PerformVoucherIssueRequest.Create(TestData.TransactionDateTime,
                                                                               TestData.OperatorId3ContractId,
                                                                               TestData.Operator3Product_200KES.ProductId,
                                                                               TestData.OperatorIdentifier3,
                                                                               TestData.RecipientMobileNumber,
                                                                               TestData.RecipientEmailAddress,
                                                                               TestData.Operator3Product_200KES.Value,
                                                                               TestData.CustomerEmailAddress);

        Result<SaleTransactionResponseMessage> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.IsSuccessfulTransaction().ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentGetAccountRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentGetAccount(It.IsAny<PerformBillPaymentGetAccountModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SuccessResult<PerformBillPaymentGetAccountResponseModel>(TestData.PerformBillPaymentGetAccountResponseModel));

        PerformBillPaymentGetAccountRequest request = PerformBillPaymentGetAccountRequest.Create(TestData.TransactionDateTime,
                                                                                                 TestData.OperatorId1ContractId,
                                                                                                 TestData.Operator1Product_100KES.ProductId,
                                                                                                 TestData.OperatorIdentifier1,
                                                                                                 TestData.CustomerAccountNumber);

        Result<PerformBillPaymentGetAccountResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
        result.Data.BillDetails.ShouldNotBeNull();
        
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentGetAccountRequest_GetAccountFailed_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentGetAccount(It.IsAny<PerformBillPaymentGetAccountModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SuccessResult<PerformBillPaymentGetAccountResponseModel>(TestData.PerformBillPaymentGetAccountResponseModelFailed));

        PerformBillPaymentGetAccountRequest request = PerformBillPaymentGetAccountRequest.Create(TestData.TransactionDateTime,
                                                                                                 TestData.OperatorId1ContractId,
                                                                                                 TestData.Operator1Product_100KES.ProductId,
                                                                                                 TestData.OperatorIdentifier1,
                                                                                                 TestData.CustomerAccountNumber);

        Result<PerformBillPaymentGetAccountResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeFalse();
        result.Data.BillDetails.ShouldBeNull();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentMakePaymentRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentMakePayment(It.IsAny<PerformBillPaymentMakePaymentModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<SaleTransactionResponseMessage>(new SaleTransactionResponseMessage
        {
            ResponseCode = "0000"
        }));

        PerformBillPaymentMakePaymentRequest request = PerformBillPaymentMakePaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                   TestData.OperatorId1ContractId,
                                                                                                   TestData.Operator1Product_100KES.ProductId,
                                                                                                   TestData.OperatorIdentifier1,
                                                                                                   TestData.CustomerAccountNumber,
                                                                                                   TestData.CustomerAccountName,
                                                                                                   TestData.CustomerMobileNumber,
                                                                                                   TestData.PaymentAmount);

        Result<SaleTransactionResponseMessage> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.IsSuccessfulTransaction().ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentMakePaymentRequest_PaymentFailed_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentMakePayment(It.IsAny<PerformBillPaymentMakePaymentModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<SaleTransactionResponseMessage>(new SaleTransactionResponseMessage
            {
                ResponseCode = "0001"
            }));

        PerformBillPaymentMakePaymentRequest request = PerformBillPaymentMakePaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                   TestData.OperatorId1ContractId,
                                                                                                   TestData.Operator1Product_100KES.ProductId,
                                                                                                   TestData.OperatorIdentifier1,
                                                                                                   TestData.CustomerAccountNumber,
                                                                                                   TestData.CustomerAccountName,
                                                                                                   TestData.CustomerMobileNumber,
                                                                                                   TestData.PaymentAmount);

        Result<SaleTransactionResponseMessage> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.Failure.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformReconciliationRequest_NoTransactions_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformReconciliation(It.IsAny<PerformReconciliationRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<ReconciliationResponseMessage>(new ReconciliationResponseMessage {
            ResponseCode = "0000"
        }));
        this.DatabaseContext.Setup(d => d.GetTransactions()).ReturnsAsync(new List<TransactionRecord>());

        PerformReconciliationRequest request = PerformReconciliationRequest.Create(TestData.TransactionDateTime,
                                                                                   TestData.DeviceIdentifier,
                                                                                   TestData.ApplicationVersion);

        Result<ReconciliationResponseMessage> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.IsSuccessfulReconciliation().ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformReconciliationRequest_TransactionsStored_Handle_IsHandled() {
        this.TransactionService.Setup(t => t.PerformReconciliation(It.IsAny<PerformReconciliationRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<ReconciliationResponseMessage>(new ReconciliationResponseMessage
            {
                ResponseCode = "0000"
            }));

        this.DatabaseContext.Setup(d => d.GetTransactions()).ReturnsAsync(TestData.StoredTransactions);

        PerformReconciliationRequest request = PerformReconciliationRequest.Create(TestData.TransactionDateTime, TestData.DeviceIdentifier, TestData.ApplicationVersion);

        Result<ReconciliationResponseMessage> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.IsSuccessfulReconciliation().ShouldBeTrue();
    }
}
