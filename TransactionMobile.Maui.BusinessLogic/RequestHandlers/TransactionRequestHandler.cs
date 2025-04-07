using Newtonsoft.Json;

namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers;

using Database;
using MediatR;
using Models;
using Requests;
using Services;
using Common;
using SimpleResults;
using UIServices;

public class TransactionRequestHandler : IRequestHandler<PerformMobileTopupRequest, Result<PerformMobileTopupResponseModel>>,
                                         IRequestHandler<LogonTransactionRequest, Result<PerformLogonResponseModel>>,
                                         IRequestHandler<PerformVoucherIssueRequest, Result<PerformVoucherIssueResponseModel>>,
                                         IRequestHandler<PerformReconciliationRequest, Result<PerformReconciliationResponseModel>>,
                                         IRequestHandler<PerformBillPaymentGetAccountRequest, Result<PerformBillPaymentGetAccountResponseModel>>,
                                         IRequestHandler<PerformBillPaymentGetMeterRequest, Result<PerformBillPaymentGetMeterResponseModel>>,
                                         IRequestHandler<PerformBillPaymentMakePostPaymentRequest, Result<PerformBillPaymentMakePaymentResponseModel>>,
                                         IRequestHandler<PerformBillPaymentMakePrePaymentRequest, Result<PerformBillPaymentMakePaymentResponseModel>>
{
    #region Fields
    
    private readonly IDatabaseContext DatabaseContext;

    private readonly IApplicationCache ApplicationCache;

    private readonly IApplicationInfoService ApplicationInfoService;

    private readonly IDeviceService DeviceService;

    private readonly Func<Boolean, ITransactionService> TransactionServiceResolver;

    #endregion

    #region Constructors

    public TransactionRequestHandler(Func<Boolean, ITransactionService> transactionServiceResolver,
                                     IDatabaseContext databaseContext,
                                     IApplicationCache applicationCache,
                                     IApplicationInfoService applicationInfoService,
                                     IDeviceService deviceService) {
        this.TransactionServiceResolver = transactionServiceResolver;
        this.DatabaseContext = databaseContext;
        this.ApplicationCache = applicationCache;
        this.ApplicationInfoService = applicationInfoService;
        this.DeviceService = deviceService;
    }

    #endregion

    #region Methods

    public async Task<Result<PerformMobileTopupResponseModel>> Handle(PerformMobileTopupRequest request,
                                                                      CancellationToken cancellationToken){

        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);

        // TODO: Factory
        PerformMobileTopupRequestModel model = new(){
                                                        ContractId = request.ContractId,
                                                        CustomerAccountNumber = request.CustomerAccountNumber,
                                                        CustomerEmailAddress = request.CustomerEmailAddress,
                                                        OperatorId = request.OperatorId,
                                                        ProductId = request.ProductId,
                                                        TopupAmount = request.TopupAmount,
                                                        TransactionDateTime = request.TransactionDateTime,
                                                        TransactionNumber = transaction.transactionNumber.ToString(),
                                                        DeviceIdentifier = this.DeviceService.GetIdentifier(),
                                                        ApplicationVersion = this.ApplicationInfoService.VersionString
                                                    };

        Result<PerformMobileTopupResponseModel> result = await transactionService.PerformMobileTopup(model, cancellationToken);
        
        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        if (result.IsSuccess && result.Data.IsSuccessful == false)
        {
            return Result.Failure("Logon transaction not successful");
        }

        return Result.Success(result.Data);

        
    }

    private async Task<(Int64 transactionNumber, TransactionRecord transactionRecord)> CreateTransactionRecord(TransactionRecord transactionRecord) {
        
        transactionRecord.ApplicationVersion = this.ApplicationInfoService.VersionString;
        transactionRecord.DeviceIdentifier = this.DeviceService.GetIdentifier();
        Int64 transactionNumber = await this.DatabaseContext.CreateTransaction(transactionRecord);

        return (transactionNumber, transactionRecord);
    }

    private async Task UpdateTransactionRecord(TransactionRecord transactionRecord) {
        await this.DatabaseContext.UpdateTransaction(transactionRecord);
    }

    public async Task<Result<PerformLogonResponseModel>> Handle(LogonTransactionRequest request,
                                                                CancellationToken cancellationToken)
    {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);

        // TODO: Factory
        PerformLogonRequestModel model = new PerformLogonRequestModel
        {
            TransactionDateTime = request.TransactionDateTime,
            TransactionNumber = transaction.transactionNumber.ToString(),
            DeviceIdentifier = this.DeviceService.GetIdentifier(),
            ApplicationVersion = this.ApplicationInfoService.VersionString
        };

        Result<PerformLogonResponseModel> result = await transactionService.PerformLogon(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));
        if (result.IsSuccess && result.Data.IsSuccessful == false)
        {
            return  Result.Failure($"Logon transaction not successful {JsonConvert.SerializeObject(result.Data)}");
        }

        return Result.Success(result.Data);
    }

    public async Task<Result<PerformVoucherIssueResponseModel>> Handle(PerformVoucherIssueRequest request,
                                                                       CancellationToken cancellationToken)
    {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);

        PerformVoucherIssueRequestModel model = new PerformVoucherIssueRequestModel
        {
            ContractId = request.ContractId,
            RecipientEmailAddress = request.RecipientEmailAddress,
            RecipientMobileNumber = request.RecipientMobileNumber,
            CustomerEmailAddress = request.CustomerEmailAddress,
            OperatorId = request.OperatorId,
            ProductId = request.ProductId,
            VoucherAmount = request.VoucherAmount,
            TransactionDateTime = request.TransactionDateTime,
            TransactionNumber = transaction.transactionNumber.ToString(),
            DeviceIdentifier = this.DeviceService.GetIdentifier(),
            ApplicationVersion = this.ApplicationInfoService.VersionString
        };

        Result<PerformVoucherIssueResponseModel> result = await transactionService.PerformVoucherIssue(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        if (result.IsSuccess && result.Data.IsSuccessful == false)
        {
            return Result.Failure("Voucher Issue not successful");
        }

        return Result.Success(result.Data);
    }

    public async Task<Result<PerformBillPaymentGetAccountResponseModel>> Handle(PerformBillPaymentGetAccountRequest request,
                                                                                CancellationToken cancellationToken)
    {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);

        PerformBillPaymentGetAccountModel model = new PerformBillPaymentGetAccountModel
        {
            ContractId = request.ContractId,
            OperatorId = request.OperatorId,
            ProductId = request.ProductId,
            TransactionDateTime = request.TransactionDateTime,
            TransactionNumber = transaction.transactionNumber.ToString(),
            CustomerAccountNumber = request.CustomerAccountNumber,
            DeviceIdentifier = this.DeviceService.GetIdentifier(),
            ApplicationVersion = this.ApplicationInfoService.VersionString
        };

        Result<PerformBillPaymentGetAccountResponseModel> result = await transactionService.PerformBillPaymentGetAccount(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        return result;
    }

    public async Task<Result<PerformReconciliationResponseModel>> Handle(PerformReconciliationRequest request,
                                                                         CancellationToken cancellationToken)
    {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        List<TransactionRecord> storedTransactions = await this.DatabaseContext.GetTransactions(useTrainingMode);

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);

        // TODO: convert these to operator totals
        var workingTotals = (from t in storedTransactions
                                                   where t.IsSuccessful && t.TransactionType != 1 // Filter out logons
                                                   group t by new
                                                   {
                                                       t.ContractId,
                                                       t.OperatorIdentifier
                                                   }
                                                   into tempOperatorTotals
                                                   select new 
                                                   {
                                                       ContractId = tempOperatorTotals.Key.ContractId,
                                                       OperatorId = tempOperatorTotals.Key.OperatorIdentifier,
                                                       TransactionValue = tempOperatorTotals.Sum(t => t.Amount),
                                                       TransactionCount = tempOperatorTotals.Count()
                                                   }).ToList();

        List<OperatorTotalModel> operatorTotals = (from t in workingTotals
                                                    where Guid.TryParse(t.OperatorId, out _) == true
                                                   select new OperatorTotalModel
                                                   {
                                                       ContractId = t.ContractId,
                                                       OperatorId = Guid.Parse(t.OperatorId),
                                                       TransactionValue = t.TransactionValue,
                                                       TransactionCount = t.TransactionCount
                                                   }).ToList();

        var grandTotals = new
        {
            TransactionValue = operatorTotals.Sum(t => t.TransactionValue),
            TransactionCount = operatorTotals.Sum(t => t.TransactionCount)
        };

        PerformReconciliationRequestModel model = new PerformReconciliationRequestModel
        {
            ApplicationVersion = request.ApplicationVersion,
            DeviceIdentifier = request.DeviceIdentifier,
            TransactionDateTime = request.TransactionDateTime,
            TransactionValue = grandTotals.TransactionValue,
            TransactionCount = grandTotals.TransactionCount,
            OperatorTotals = operatorTotals
        };

        // Send to the host        
        Result<PerformReconciliationResponseModel> result = await transactionService.PerformReconciliation(model, cancellationToken);

        // Clear store (if successful)
        if (result.IsSuccess && result.Data.IsSuccessful)
        {
            await this.DatabaseContext.ClearStoredTransactions(storedTransactions);
        }

        return result;
    }

    public async Task<Result<PerformBillPaymentMakePaymentResponseModel>> Handle(PerformBillPaymentMakePostPaymentRequest request,
                                                                                 CancellationToken cancellationToken)
    {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);

        PerformBillPaymentMakePaymentModel model = new PerformBillPaymentMakePaymentModel
        {
            ContractId = request.ContractId,
            OperatorId = request.OperatorId,
            ProductId = request.ProductId,
            TransactionDateTime = request.TransactionDateTime,
            TransactionNumber = transaction.transactionNumber.ToString(),
            CustomerAccountNumber = request.CustomerAccountNumber,
            PaymentAmount = request.PaymentAmount,
            CustomerAccountName = request.CustomerAccountName,
            CustomerMobileNumber = request.CustomerMobileNumber,
            DeviceIdentifier = this.DeviceService.GetIdentifier(),
            ApplicationVersion = this.ApplicationInfoService.VersionString,
            PostPayment = true
        };
        
        Result<PerformBillPaymentMakePaymentResponseModel> result = await transactionService.PerformBillPaymentMakePayment(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        if (result.IsSuccess && result.Data.IsSuccessful == false)
        {
            return Result.Failure("Bill Payment not successful");
        }

        return Result.Success(result.Data);

    }

    public async Task<Result<PerformBillPaymentMakePaymentResponseModel>> Handle(PerformBillPaymentMakePrePaymentRequest request,
                                                                                 CancellationToken cancellationToken)
    {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);

        PerformBillPaymentMakePaymentModel model = new PerformBillPaymentMakePaymentModel
                                                   {
                                                       ContractId = request.ContractId,
                                                       OperatorId = request.OperatorId,
                                                       ProductId = request.ProductId,
                                                       TransactionDateTime = request.TransactionDateTime,
                                                       TransactionNumber = transaction.transactionNumber.ToString(),
                                                       CustomerAccountNumber = request.MeterNumber,
                                                       PaymentAmount = request.PaymentAmount,
                                                       CustomerAccountName = request.CustomerAccountName,
                                                       DeviceIdentifier = this.DeviceService.GetIdentifier(),
                                                       ApplicationVersion = this.ApplicationInfoService.VersionString,
                                                       PostPayment = false
                                                   };

        Result<PerformBillPaymentMakePaymentResponseModel> result = await transactionService.PerformBillPaymentMakePayment(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        if (result.IsSuccess && result.Data.IsSuccessful == false)
        {
            return Result.Failure("Bill Payment not successful");
        }

        return Result.Success(result.Data);

    }
    #endregion

    public async Task<Result<PerformBillPaymentGetMeterResponseModel>> Handle(PerformBillPaymentGetMeterRequest request, CancellationToken cancellationToken){
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);

        PerformBillPaymentGetMeterModel model = new PerformBillPaymentGetMeterModel
        {
                                                      ContractId = request.ContractId,
                                                      OperatorId = request.OperatorId,
                                                      ProductId = request.ProductId,
                                                      TransactionDateTime = request.TransactionDateTime,
                                                      TransactionNumber = transaction.transactionNumber.ToString(),
                                                      MeterNumber = request.MeterNumber,
                                                      DeviceIdentifier = this.DeviceService.GetIdentifier(),
                                                      ApplicationVersion = this.ApplicationInfoService.VersionString
                                                  };

        Result<PerformBillPaymentGetMeterResponseModel> result = await transactionService.PerformBillPaymentGetMeter(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        return result;
    }
}