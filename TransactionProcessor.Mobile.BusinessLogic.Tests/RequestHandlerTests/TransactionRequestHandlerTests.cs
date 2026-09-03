using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Database;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.RequestHandlers;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Serialisation;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.RequestHandlerTests;

public class TransactionRequestHandlerTests
{
    private ITransactionServiceImposter TransactionService;

    private IDatabaseContextImposter DatabaseContext;

    private IApplicationCacheImposter ApplicationCache;

    private IApplicationInfoServiceImposter ApplicationInfoService;

    private IDeviceServiceImposter DeviceService;

    private TransactionRequestHandler TransactionRequestHandler;

    private Func<Boolean, ITransactionService> TransactionServiceResolver;

    public TransactionRequestHandlerTests() {
        this.TransactionService = new ITransactionServiceImposter();
        this.DatabaseContext = new IDatabaseContextImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.ApplicationInfoService = new IApplicationInfoServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.TransactionServiceResolver = _ =>
                                          {
                                              return this.TransactionService.Instance();
                                          };


        this.TransactionRequestHandler = new TransactionRequestHandler(this.TransactionServiceResolver, 
                                                                       this.DatabaseContext.Instance(),
                                                                       this.ApplicationCache.Instance(),
                                                                       this.ApplicationInfoService.Instance(),
                                                                       this.DeviceService.Instance());
        StringSerialiser.Initialise((IStringSerialiser)new SystemTextJsonSerializer(SystemTextJsonSerializer.GetDefaultJsonSerializerOptions()));

    }

    [Fact]
    public async Task TransactionRequestHandler_LogonTransactionRequest_Handle_IsHandled()
    {
        this.TransactionService.PerformLogon(Arg<PerformLogonRequestModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(
         Result.Success(TestData.PerformLogonResponseModel));
     
        TransactionCommands.PerformLogonCommand request = new(TestData.TransactionDateTime);

        Result<PerformLogonResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_LogonTransactionRequest_Handle_LogonFailed_IsHandled()
    {
        this.TransactionService.PerformLogon(Arg<PerformLogonRequestModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(
                                                                                                                                              Result.Success(TestData.PerformLogonResponseFailedModel));

        TransactionCommands.PerformLogonCommand request = new(TestData.TransactionDateTime);

        Result<PerformLogonResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task TransactionRequestHandler_LogonTransactionRequest_Handle_ServiceFailure_ReturnsFailure_AndDoesNotUpdateTransaction()
    {
        this.DatabaseContext.CreateTransaction(Arg<TransactionRecord>.Any()).ReturnsAsync(1);
        this.TransactionService.PerformLogon(Arg<PerformLogonRequestModel>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("Logon failed"));

        TransactionCommands.PerformLogonCommand request = new(TestData.TransactionDateTime);

        Result<PerformLogonResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        this.DatabaseContext.UpdateTransaction(Arg<TransactionRecord>.Any()).Called(Count.Never());
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformMobileTopupRequest_Handle_IsHandled()
    {
        this.TransactionService.PerformMobileTopup(Arg<PerformMobileTopupRequestModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformMobileTopupResponseModel
                                                                                                                                                                        {
                                                                                                                                                                            ResponseCode = "0000"
                                                                                                                                                                        }));
     
        TransactionCommands.PerformMobileTopupCommand request = new TransactionCommands.PerformMobileTopupCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformMobileTopup(Arg<PerformMobileTopupRequestModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformMobileTopupResponseModel
                                                                                                                                                                        {
                                                                                                                                                                            ResponseCode = "1000"
                                                                                                                                                                        }));

        TransactionCommands.PerformMobileTopupCommand request = new TransactionCommands.PerformMobileTopupCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformVoucherIssue(Arg<PerformVoucherIssueRequestModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformVoucherIssueResponseModel
                                                                                                                                                                          {
                                                                                                                                                                              ResponseCode = "0000"
                                                                                                                                                                          }));
        TransactionCommands.PerformVoucherIssueCommand request = new TransactionCommands.PerformVoucherIssueCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformVoucherIssue(Arg<PerformVoucherIssueRequestModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformVoucherIssueResponseModel
                                                                                                                                                                          {
                                                                                                                                                                              ResponseCode = "1000"
                                                                                                                                                                          }));

        TransactionCommands.PerformVoucherIssueCommand request = new TransactionCommands.PerformVoucherIssueCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformBillPaymentGetAccount(Arg<PerformBillPaymentGetAccountModel>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetAccountResponseModel));

        TransactionCommands.PerformBillPaymentGetAccountCommand request = new TransactionCommands.PerformBillPaymentGetAccountCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformBillPaymentGetAccount(Arg<PerformBillPaymentGetAccountModel>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetAccountResponseModelFailed));

        TransactionCommands.PerformBillPaymentGetAccountCommand request = new TransactionCommands.PerformBillPaymentGetAccountCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformBillPaymentGetMeter(Arg<PerformBillPaymentGetMeterModel>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetMeterResponseModel));

        TransactionCommands.PerformBillPaymentGetMeterCommand request = new TransactionCommands.PerformBillPaymentGetMeterCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformBillPaymentGetMeter(Arg<PerformBillPaymentGetMeterModel>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetMeterResponseModelFailed));

        TransactionCommands.PerformBillPaymentGetMeterCommand request = new TransactionCommands.PerformBillPaymentGetMeterCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformBillPaymentMakePayment(Arg<PerformBillPaymentMakePaymentModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel
                                                                                                                                                                                       {
                                                                                                                                                                                           ResponseCode = "0000"
                                                                                                                                                                                       }));

        TransactionCommands.PerformBillPaymentMakePostPaymentCommand request = new TransactionCommands.PerformBillPaymentMakePostPaymentCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformBillPaymentMakePayment(Arg<PerformBillPaymentMakePaymentModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel
                                                                                                                                                                                       {
                                                                                                                                                                                           ResponseCode = "0000"
                                                                                                                                                                                       }));
        TransactionCommands.PerformBillPaymentMakePrePaymentCommand request = new TransactionCommands.PerformBillPaymentMakePrePaymentCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformBillPaymentMakePayment(Arg<PerformBillPaymentMakePaymentModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel
                                                                                                                                                                                       {
                                                                                                                                                                                           ResponseCode = "0001"
                                                                                                                                                                                       }));

        TransactionCommands.PerformBillPaymentMakePostPaymentCommand request = new TransactionCommands.PerformBillPaymentMakePostPaymentCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformBillPaymentMakePayment(Arg<PerformBillPaymentMakePaymentModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel
                                                                                                                                                                                       {
                                                                                                                                                                                           ResponseCode = "0001"
                                                                                                                                                                                       }));

        TransactionCommands.PerformBillPaymentMakePrePaymentCommand request = new TransactionCommands.PerformBillPaymentMakePrePaymentCommand(TestData.TransactionDateTime,
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
        this.TransactionService.PerformReconciliation(Arg<PerformReconciliationRequestModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformReconciliationResponseModel
                                                                                                                                                                              {
                                                                                                                                                                                  ResponseCode = "0000"
                                                                                                                                                                              }));
        this.DatabaseContext.GetTransactions(Arg<Boolean>.Any()).ReturnsAsync(new List<TransactionRecord>());

        TransactionCommands.PerformReconciliationCommand request = new TransactionCommands.PerformReconciliationCommand(TestData.TransactionDateTime, TestData.DeviceIdentifier, TestData.ApplicationVersion);

        Result<PerformReconciliationResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task TransactionRequestHandler_PerformReconciliationRequest_TransactionsStored_Handle_IsHandled() {
        this.TransactionService.PerformReconciliation(Arg<PerformReconciliationRequestModel>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformReconciliationResponseModel
                                                                                                                                                                              {
                                                                                                                                                                                  ResponseCode = "0000"
                                                                                                                                                                              }));

        this.DatabaseContext.GetTransactions(Arg<Boolean>.Any()).ReturnsAsync(TestData.StoredTransactions);

        TransactionCommands.PerformReconciliationCommand request = new TransactionCommands.PerformReconciliationCommand(TestData.TransactionDateTime, TestData.DeviceIdentifier, TestData.ApplicationVersion);

        Result<PerformReconciliationResponseModel> result = await this.TransactionRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.IsSuccessful.ShouldBeTrue();
    }
}
