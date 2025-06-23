using Moq;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Database;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.RequestHandlers;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.RequestHandlerTests;

public class TransactionRequestHandlerTests
{
    private Mock<ITransactionService> TransactionService;

    private Mock<IDatabaseContext> DatabaseContext;

    private Mock<IApplicationCache> ApplicationCache;

    private Mock<IApplicationInfoService> ApplicationInfoService;

    private Mock<IDeviceService> DeviceService;

    private TransactionRequestHandler TransactionRequestHandler;

    private Func<Boolean, ITransactionService> TransactionServiceResolver;

    public TransactionRequestHandlerTests() {
        this.TransactionService = new Mock<ITransactionService>();
        this.DatabaseContext = new Mock<IDatabaseContext>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.ApplicationInfoService = new Mock<IApplicationInfoService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.TransactionServiceResolver = _ =>
                                          {
                                              return this.TransactionService.Object;
                                          };


        this.TransactionRequestHandler = new TransactionRequestHandler(this.TransactionServiceResolver, 
                                                                       this.DatabaseContext.Object, 
                                                                       this.ApplicationCache.Object,
                                                                       this.ApplicationInfoService.Object,
                                                                       this.DeviceService.Object);

    }

    [Fact]
    public async Task TransactionRequestHandler_LogonTransactionRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformLogon(It.IsAny<PerformLogonRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(
         Result.Success(TestData.PerformLogonResponseModel));
     
        LogonTransactionRequest request = LogonTransactionRequest.Create(TestData.TransactionDateTime);

        Result<PerformLogonResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_LogonTransactionRequest_Handle_LogonFailed_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformLogon(It.IsAny<PerformLogonRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                                                                                                                                             Result.Success(TestData.PerformLogonResponseFailedModel));

        LogonTransactionRequest request = LogonTransactionRequest.Create(TestData.TransactionDateTime);

        Result<PerformLogonResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformMobileTopupRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformMobileTopup(It.IsAny<PerformMobileTopupRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformMobileTopupResponseModel
                                                                                                                                                                        {
                                                                                                                                                                            ResponseCode = "0000"
                                                                                                                                                                        }));
     
        PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(TestData.TransactionDateTime,
                                                                             TestData.OperatorId1ContractId,
                                                                             TestData.Operator1Product_100KES.ProductId,
                                                                             TestData.OperatorId1,
                                                                             TestData.CustomerAccountNumber,
                                                                             TestData.Operator1Product_100KES.Value,
                                                                             TestData.CustomerEmailAddress);

        Result<PerformMobileTopupResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformMobileTopupRequest_Handle_TopupFailed_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformMobileTopup(It.IsAny<PerformMobileTopupRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformMobileTopupResponseModel
                                                                                                                                                                        {
                                                                                                                                                                            ResponseCode = "1000"
                                                                                                                                                                        }));

        PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(TestData.TransactionDateTime,
                                                                             TestData.OperatorId1ContractId,
                                                                             TestData.Operator1Product_100KES.ProductId,
                                                                             TestData.OperatorId1,
                                                                             TestData.CustomerAccountNumber,
                                                                             TestData.Operator1Product_100KES.Value,
                                                                             TestData.CustomerEmailAddress);

        Result<PerformMobileTopupResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformVoucherIssueRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformVoucherIssue(It.IsAny<PerformVoucherIssueRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformVoucherIssueResponseModel
                                                                                                                                                                          {
                                                                                                                                                                              ResponseCode = "0000"
                                                                                                                                                                          }));

        PerformVoucherIssueRequest request = PerformVoucherIssueRequest.Create(TestData.TransactionDateTime,
                                                                               TestData.OperatorId3ContractId,
                                                                               TestData.Operator3Product_200KES.ProductId,
                                                                               TestData.OperatorId3,
                                                                               TestData.RecipientMobileNumber,
                                                                               TestData.RecipientEmailAddress,
                                                                               TestData.Operator3Product_200KES.Value,
                                                                               TestData.CustomerEmailAddress);

        Result<PerformVoucherIssueResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformVoucherIssueRequest_Handle_VoucherIssueFailed_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformVoucherIssue(It.IsAny<PerformVoucherIssueRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformVoucherIssueResponseModel
                                                                                                                                                                          {
                                                                                                                                                                              ResponseCode = "1000"
                                                                                                                                                                          }));

        PerformVoucherIssueRequest request = PerformVoucherIssueRequest.Create(TestData.TransactionDateTime,
                                                                               TestData.OperatorId3ContractId,
                                                                               TestData.Operator3Product_200KES.ProductId,
                                                                               TestData.OperatorId3,
                                                                               TestData.RecipientMobileNumber,
                                                                               TestData.RecipientEmailAddress,
                                                                               TestData.Operator3Product_200KES.Value,
                                                                               TestData.CustomerEmailAddress);

        Result<PerformVoucherIssueResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentGetAccountRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentGetAccount(It.IsAny<PerformBillPaymentGetAccountModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetAccountResponseModel));

        PerformBillPaymentGetAccountRequest request = PerformBillPaymentGetAccountRequest.Create(TestData.TransactionDateTime,
                                                                                                 TestData.OperatorId1ContractId,
                                                                                                 TestData.Operator1Product_100KES.ProductId,
                                                                                                 TestData.OperatorId1,
                                                                                                 TestData.CustomerAccountNumber);

        Result<PerformBillPaymentGetAccountResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
        result.Data.BillDetails.ShouldNotBeNull();
        
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentGetAccountRequest_GetAccountFailed_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentGetAccount(It.IsAny<PerformBillPaymentGetAccountModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetAccountResponseModelFailed));

        PerformBillPaymentGetAccountRequest request = PerformBillPaymentGetAccountRequest.Create(TestData.TransactionDateTime,
                                                                                                 TestData.OperatorId1ContractId,
                                                                                                 TestData.Operator1Product_100KES.ProductId,
                                                                                                 TestData.OperatorId1,
                                                                                                 TestData.CustomerAccountNumber);

        Result<PerformBillPaymentGetAccountResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeFalse();
        result.Data.BillDetails.ShouldBeNull();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentGetMeterRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentGetMeter(It.IsAny<PerformBillPaymentGetMeterModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetMeterResponseModel));

        PerformBillPaymentGetMeterRequest request = PerformBillPaymentGetMeterRequest.Create(TestData.TransactionDateTime,
                                                                                             TestData.OperatorId1ContractId,
                                                                                             TestData.Operator1Product_100KES.ProductId,
                                                                                             TestData.OperatorId1,
                                                                                             TestData.CustomerAccountNumber);

        Result<PerformBillPaymentGetMeterResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
        result.Data.MeterDetails.ShouldNotBeNull();

    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentGetMeterRequest_GetMeterFailed_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentGetMeter(It.IsAny<PerformBillPaymentGetMeterModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetMeterResponseModelFailed));

        PerformBillPaymentGetMeterRequest request = PerformBillPaymentGetMeterRequest.Create(TestData.TransactionDateTime,
                                                                                             TestData.OperatorId1ContractId,
                                                                                             TestData.Operator1Product_100KES.ProductId,
                                                                                             TestData.OperatorId1,
                                                                                             TestData.CustomerAccountNumber);

        Result<PerformBillPaymentGetMeterResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeFalse();
        result.Data.MeterDetails.ShouldBeNull();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentMakePostPaymentRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentMakePayment(It.IsAny<PerformBillPaymentMakePaymentModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel
                                                                                                                                                                                       {
                                                                                                                                                                                           ResponseCode = "0000"
                                                                                                                                                                                       }));

        PerformBillPaymentMakePostPaymentRequest request = PerformBillPaymentMakePostPaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                           TestData.OperatorId1ContractId,
                                                                                                           TestData.Operator1Product_100KES.ProductId,
                                                                                                           TestData.OperatorId1,
                                                                                                           TestData.CustomerAccountNumber,
                                                                                                           TestData.CustomerAccountName,
                                                                                                           TestData.CustomerMobileNumber,
                                                                                                           TestData.PaymentAmount);

        Result<PerformBillPaymentMakePaymentResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentMakePrePaymentRequest_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentMakePayment(It.IsAny<PerformBillPaymentMakePaymentModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel
                                                                                                                                                                                       {
                                                                                                                                                                                           ResponseCode = "0000"
                                                                                                                                                                                       }));

        PerformBillPaymentMakePrePaymentRequest request = PerformBillPaymentMakePrePaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                         TestData.OperatorId1ContractId,
                                                                                                         TestData.Operator1Product_100KES.ProductId,
                                                                                                         TestData.OperatorId1,
                                                                                                         TestData.MeterNumber,
                                                                                                         TestData.CustomerAccountName,
                                                                                                         TestData.PaymentAmount);

        Result<PerformBillPaymentMakePaymentResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentMakePostPaymentRequest_PaymentFailed_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentMakePayment(It.IsAny<PerformBillPaymentMakePaymentModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel
                                                                                                                                                                                       {
                                                                                                                                                                                           ResponseCode = "0001"
                                                                                                                                                                                       }));

        PerformBillPaymentMakePostPaymentRequest request = PerformBillPaymentMakePostPaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                           TestData.OperatorId1ContractId,
                                                                                                           TestData.Operator1Product_100KES.ProductId,
                                                                                                           TestData.OperatorId1,
                                                                                                           TestData.CustomerAccountNumber,
                                                                                                           TestData.CustomerAccountName,
                                                                                                           TestData.CustomerMobileNumber,
                                                                                                           TestData.PaymentAmount);

        Result<PerformBillPaymentMakePaymentResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformBillPaymentMakePrePaymentRequest_PaymentFailed_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformBillPaymentMakePayment(It.IsAny<PerformBillPaymentMakePaymentModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel
                                                                                                                                                                                       {
                                                                                                                                                                                           ResponseCode = "0001"
                                                                                                                                                                                       }));

        PerformBillPaymentMakePrePaymentRequest request = PerformBillPaymentMakePrePaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                         TestData.OperatorId1ContractId,
                                                                                                         TestData.Operator1Product_100KES.ProductId,
                                                                                                         TestData.OperatorId1,
                                                                                                         TestData.MeterNumber,
                                                                                                         TestData.CustomerAccountName,
                                                                                                         TestData.PaymentAmount);

        Result<PerformBillPaymentMakePaymentResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformReconciliationRequest_NoTransactions_Handle_IsHandled()
    {
        this.TransactionService.Setup(t => t.PerformReconciliation(It.IsAny<PerformReconciliationRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformReconciliationResponseModel
                                                                                                                                                                              {
                                                                                                                                                                                  ResponseCode = "0000"
                                                                                                                                                                              }));
        this.DatabaseContext.Setup(d => d.GetTransactions(It.IsAny<Boolean>())).ReturnsAsync(new List<TransactionRecord>());

        PerformReconciliationRequest request = PerformReconciliationRequest.Create(TestData.TransactionDateTime,
                                                                                   TestData.DeviceIdentifier,
                                                                                   TestData.ApplicationVersion);

        Result<PerformReconciliationResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformReconciliationRequest_TransactionsStored_Handle_IsHandled() {
        this.TransactionService.Setup(t => t.PerformReconciliation(It.IsAny<PerformReconciliationRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformReconciliationResponseModel
                                                                                                                                                                              {
                                                                                                                                                                                  ResponseCode = "0000"
                                                                                                                                                                              }));

        this.DatabaseContext.Setup(d => d.GetTransactions(It.IsAny<Boolean>())).ReturnsAsync(TestData.StoredTransactions);

        PerformReconciliationRequest request = PerformReconciliationRequest.Create(TestData.TransactionDateTime, TestData.DeviceIdentifier, TestData.ApplicationVersion);

        Result<PerformReconciliationResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
    }
}
