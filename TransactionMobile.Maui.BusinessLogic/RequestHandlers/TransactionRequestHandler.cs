namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers;

using Database;
using MediatR;
using Models;
using Requests;
using Services;
using System.Transactions;
using Common;
using Microsoft.Maui.Platform;
using UIServices;
using TransactionProcessorACL.DataTransferObjects.Responses;

public class TransactionRequestHandler : IRequestHandler<PerformMobileTopupRequest, Result<PerformMobileTopupResponseModel>>,
                                         IRequestHandler<LogonTransactionRequest, Result<PerformLogonResponseModel>>,
                                         IRequestHandler<PerformVoucherIssueRequest, Result<PerformVoucherIssueResponseModel>>,
                                         IRequestHandler<PerformReconciliationRequest, Result<PerformReconciliationResponseModel>>,
                                         IRequestHandler<PerformBillPaymentGetAccountRequest, Result<PerformBillPaymentGetAccountResponseModel>>,
                                         IRequestHandler<PerformBillPaymentGetMeterRequest, Result<PerformBillPaymentGetMeterResponseModel>>,
                                         IRequestHandler<PerformBillPaymentMakePaymentRequest, Result<PerformBillPaymentMakePaymentResponseModel>>
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
                                                        OperatorIdentifier = request.OperatorIdentifier,
                                                        ProductId = request.ProductId,
                                                        TopupAmount = request.TopupAmount,
                                                        TransactionDateTime = request.TransactionDateTime,
                                                        TransactionNumber = transaction.transactionNumber.ToString(),
                                                        DeviceIdentifier = this.DeviceService.GetIdentifier(),
                                                        ApplicationVersion = this.ApplicationInfoService.VersionString
                                                    };

        Result<PerformMobileTopupResponseModel> result = await transactionService.PerformMobileTopup(model, cancellationToken);
        
        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        if (result.Success && result.Data.IsSuccessful == false)
        {
            return new ErrorResult<PerformMobileTopupResponseModel>("Logon transaction not successful");
        }

        return new SuccessResult<PerformMobileTopupResponseModel>(result.Data);

        
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
        if (result.Success && result.Data.IsSuccessful == false)
        {
            return new ErrorResult<PerformLogonResponseModel>("Logon transaction not successful");
        }

        return new SuccessResult<PerformLogonResponseModel>(result.Data);
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
            OperatorIdentifier = request.OperatorIdentifier,
            ProductId = request.ProductId,
            VoucherAmount = request.VoucherAmount,
            TransactionDateTime = request.TransactionDateTime,
            TransactionNumber = transaction.transactionNumber.ToString(),
            DeviceIdentifier = this.DeviceService.GetIdentifier(),
            ApplicationVersion = this.ApplicationInfoService.VersionString
        };

        Result<PerformVoucherIssueResponseModel> result = await transactionService.PerformVoucherIssue(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        if (result.Success && result.Data.IsSuccessful == false)
        {
            return new ErrorResult<PerformVoucherIssueResponseModel>("Voucher Issue not successful");
        }

        return new SuccessResult<PerformVoucherIssueResponseModel>(result.Data);
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
            OperatorIdentifier = request.OperatorIdentifier,
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
        List<OperatorTotalModel> operatorTotals = (from t in storedTransactions
                                                   where t.IsSuccessful && t.TransactionType != 1 // Filter out logons
                                                   group t by new
                                                   {
                                                       t.ContractId,
                                                       t.OperatorIdentifier
                                                   }
                                                   into tempOperatorTotals
                                                   select new OperatorTotalModel
                                                   {
                                                       ContractId = tempOperatorTotals.Key.ContractId,
                                                       OperatorIdentifier = tempOperatorTotals.Key.OperatorIdentifier,
                                                       TransactionValue = tempOperatorTotals.Sum(t => t.Amount),
                                                       TransactionCount = tempOperatorTotals.Count()
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
        if (result.Success && result.Data.IsSuccessful)
        {
            await this.DatabaseContext.ClearStoredTransactions(storedTransactions);
        }

        return result;
    }

    public async Task<Result<PerformBillPaymentMakePaymentResponseModel>> Handle(PerformBillPaymentMakePaymentRequest request,
                                                                                 CancellationToken cancellationToken)
    {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);

        PerformBillPaymentMakePaymentModel model = new PerformBillPaymentMakePaymentModel
        {
            ContractId = request.ContractId,
            OperatorIdentifier = request.OperatorIdentifier,
            ProductId = request.ProductId,
            TransactionDateTime = request.TransactionDateTime,
            TransactionNumber = transaction.transactionNumber.ToString(),
            CustomerAccountNumber = request.CustomerAccountNumber,
            PaymentAmount = request.PaymentAmount,
            CustomerAccountName = request.CustomerAccountName,
            CustomerMobileNumber = request.CustomerMobileNumber,
            DeviceIdentifier = this.DeviceService.GetIdentifier(),
            ApplicationVersion = this.ApplicationInfoService.VersionString
        };

        Result<PerformBillPaymentMakePaymentResponseModel> result = await transactionService.PerformBillPaymentMakePayment(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        if (result.Success && result.Data.IsSuccessful == false)
        {
            return new ErrorResult<PerformBillPaymentMakePaymentResponseModel>("Bill Payment not successful");
        }

        return new SuccessResult<PerformBillPaymentMakePaymentResponseModel>(result.Data);

    }
    #endregion

    public async Task<Result<PerformBillPaymentGetMeterResponseModel>> Handle(PerformBillPaymentGetMeterRequest request, CancellationToken cancellationToken){
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        //(Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);

        PerformBillPaymentGetMeterModel model = new PerformBillPaymentGetMeterModel
        {
                                                      ContractId = request.ContractId,
                                                      OperatorIdentifier = request.OperatorIdentifier,
                                                      ProductId = request.ProductId,
                                                      TransactionDateTime = request.TransactionDateTime,
                                                      //TransactionNumber = transaction.transactionNumber.ToString(),
                                                      MeterNumber = request.MeterNumber,
                                                      DeviceIdentifier = this.DeviceService.GetIdentifier(),
                                                      ApplicationVersion = this.ApplicationInfoService.VersionString
                                                  };

        Result<PerformBillPaymentGetMeterResponseModel> result = await transactionService.PerformBillPaymentGetMeter(model, cancellationToken);

        //await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        return result;
    }
}