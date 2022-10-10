namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers;

using Database;
using MediatR;
using Models;
using Requests;
using Services;
using System.Transactions;
using Common;
using UIServices;

public class TransactionRequestHandler : IRequestHandler<PerformMobileTopupRequest, Boolean>,
                                         IRequestHandler<LogonTransactionRequest, PerformLogonResponseModel>,
                                         IRequestHandler<PerformVoucherIssueRequest, Boolean>,
                                         IRequestHandler<PerformReconciliationRequest, Boolean>,
                                         IRequestHandler<PerformBillPaymentGetAccountRequest, PerformBillPaymentGetAccountResponseModel>,
                                         IRequestHandler<PerformBillPaymentMakePaymentRequest,Boolean>
{
    #region Fields

    private readonly Func<Boolean, ITransactionService> TransactionServiceResolver;

    private readonly IDatabaseContext DatabaseContext;

    private readonly IApplicationCache ApplicationCache;

    private readonly IApplicationInfoService ApplicationInfoService;

    private readonly IDeviceService DeviceService;

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

    public async Task<Boolean> Handle(PerformMobileTopupRequest request,
                                      CancellationToken cancellationToken) {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        // TODO: Factory
        PerformMobileTopupRequestModel model = new PerformMobileTopupRequestModel {
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


        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);
        Boolean result = await transactionService.PerformMobileTopup(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        return result;
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
    
    public async Task<PerformLogonResponseModel> Handle(LogonTransactionRequest request,
                                                        CancellationToken cancellationToken) {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        // TODO: Factory
        PerformLogonRequestModel model = new PerformLogonRequestModel {
                                                                          TransactionDateTime = request.TransactionDateTime,
                                                                          TransactionNumber = transaction.transactionNumber.ToString(),
                                                                          DeviceIdentifier = this.DeviceService.GetIdentifier(),
                                                                          ApplicationVersion = this.ApplicationInfoService.VersionString
                                                                      };

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);
        PerformLogonResponseModel result = await transactionService.PerformLogon(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        return result;
    }

    #endregion

    public async Task<Boolean> Handle(PerformVoucherIssueRequest request,
                                      CancellationToken cancellationToken) {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));
        
        PerformVoucherIssueRequestModel model = new PerformVoucherIssueRequestModel {
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

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);
        Boolean result = await transactionService.PerformVoucherIssue(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));

        return result;
    }

    public async Task<PerformBillPaymentGetAccountResponseModel> Handle(PerformBillPaymentGetAccountRequest request,
                                                                        CancellationToken cancellationToken) {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));
        
        PerformBillPaymentGetAccountModel model = new PerformBillPaymentGetAccountModel {
                                                                                            ContractId = request.ContractId,
                                                                                            OperatorIdentifier = request.OperatorIdentifier,
                                                                                            ProductId = request.ProductId,
                                                                                            TransactionDateTime = request.TransactionDateTime,
                                                                                            TransactionNumber = transaction.transactionNumber.ToString(),
                                                                                            CustomerAccountNumber = request.CustomerAccountNumber,
                                                                                            DeviceIdentifier = this.DeviceService.GetIdentifier(),
                                                                                            ApplicationVersion = this.ApplicationInfoService.VersionString
        };

        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);
        PerformBillPaymentGetAccountResponseModel result = await transactionService.PerformBillPaymentGetAccount(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result.IsSuccessful));

        return result;
    }

    public async Task<Boolean> Handle(PerformReconciliationRequest request,
                                      CancellationToken cancellationToken) {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

        List<TransactionRecord> storedTransactions = await this.DatabaseContext.GetTransactions(useTrainingMode);

        if (storedTransactions.Any() == false) {
            return true;
        }

        // TODO: convert these to operator totals
        List<OperatorTotalModel> operatorTotals = (from t in storedTransactions
                                                   where t.IsSuccessful && t.TransactionType != 1 // Filter out logons
                                                   group t by new {
                                                                      t.ContractId,
                                                                      t.OperatorIdentifier
                                                                  }
                                                   into tempOperatorTotals
                                                   select new OperatorTotalModel {
                                                                                     ContractId = tempOperatorTotals.Key.ContractId,
                                                                                     OperatorIdentifier = tempOperatorTotals.Key.OperatorIdentifier,
                                                                                     TransactionValue = tempOperatorTotals.Sum(t => t.Amount),
                                                                                     TransactionCount = tempOperatorTotals.Count()
                                                                                 }).ToList();

        var grandTotals = new {
                                  TransactionValue = operatorTotals.Sum(t => t.TransactionValue),
                                  TransactionCount = operatorTotals.Sum(t => t.TransactionCount)
                              };

        PerformReconciliationRequestModel model = new PerformReconciliationRequestModel {
                                                                                            ApplicationVersion = request.ApplicationVersion,
                                                                                            DeviceIdentifier = request.DeviceIdentifier,
                                                                                            TransactionDateTime = request.TransactionDateTime,
                                                                                            TransactionValue = grandTotals.TransactionValue,
                                                                                            TransactionCount = grandTotals.TransactionCount,
                                                                                            OperatorTotals = operatorTotals
                                                                                        };
        // Send to the host        
        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);
        Boolean result = await transactionService.PerformReconciliation(model, cancellationToken);

        // Clear store (if successful)
        if (result) {
            await this.DatabaseContext.ClearStoredTransactions(storedTransactions);
        }

        return result;
    }

    public async Task<Boolean> Handle(PerformBillPaymentMakePaymentRequest request,
                                CancellationToken cancellationToken) {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        (Int64 transactionNumber, TransactionRecord transactionRecord) transaction = await this.CreateTransactionRecord(request.ToTransactionRecord(useTrainingMode));

        PerformBillPaymentMakePaymentModel model = new PerformBillPaymentMakePaymentModel {
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
        
        ITransactionService transactionService = this.TransactionServiceResolver(useTrainingMode);
        Boolean result = await transactionService.PerformBillPaymentMakePayment(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord.UpdateFrom(result));
        return result;
    }
}