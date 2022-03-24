namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers;

using Database;
using MediatR;
using Models;
using Requests;
using Services;

public class TransactionRequestHandler : IRequestHandler<PerformMobileTopupRequest, Boolean>,
                                         IRequestHandler<LogonTransactionRequest, PerformLogonResponseModel>,
                                         IRequestHandler<PerformVoucherIssueRequest, Boolean>,
                                         IRequestHandler<PerformReconciliationRequest, Boolean>                                         
{
    #region Fields

    private readonly Func<Boolean,ITransactionService> TransactionServiceResolver;
    private readonly IDatabaseContext DatabaseContext;
    private readonly IMemoryCacheService MemoryCacheService;
    #endregion

    #region Constructors

    public TransactionRequestHandler(Func<Boolean, ITransactionService> transactionServiceResolver,
                                     IDatabaseContext databaseContext,
                                     IMemoryCacheService memoryCacheService)
    {
        this.TransactionServiceResolver = transactionServiceResolver;
        this.DatabaseContext = databaseContext;
        this.MemoryCacheService = memoryCacheService;
    }

    #endregion

    #region Methods

    public async Task<Boolean> Handle(PerformMobileTopupRequest request,
                                      CancellationToken cancellationToken)
    {
        this.MemoryCacheService.TryGetValue("UseTrainingMode", out Boolean useTrainingMode);
        (TransactionRecord transactionRecord, Int64 transactionNumber) transaction = await this.CreateTransactionRecord(request, useTrainingMode, cancellationToken);

        // TODO: Factory
        PerformMobileTopupRequestModel model = new PerformMobileTopupRequestModel
                                               {
                                                   ApplicationVersion = request.ApplicationVersion,
                                                   ContractId = request.ContractId,
                                                   CustomerAccountNumber = request.CustomerAccountNumber,
                                                   CustomerEmailAddress = request.CustomerEmailAddress,
                                                   DeviceIdentifier = request.DeviceIdentifier,
                                                   OperatorIdentifier = request.OperatorIdentifier,
                                                   ProductId = request.ProductId,
                                                   TopupAmount = request.TopupAmount,
                                                   TransactionDateTime = request.TransactionDateTime,
                                                   TransactionNumber = request.TransactionNumber
                                               };

        
        var transactionService = this.TransactionServiceResolver(useTrainingMode);
        Boolean result = await transactionService.PerformMobileTopup(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord, result, cancellationToken);

        return result;
    }

    private async Task<(TransactionRecord transactionRecord, Int64 transactionNumber)> CreateTransactionRecord(LogonTransactionRequest request,
        Boolean useTrainingMode,
                                                                                                               CancellationToken cancellationToken)
    {
        TransactionRecord transactionRecord = new TransactionRecord
                                              {
                                                  ApplicationVersion = request.ApplicationVersion,
                                                  DeviceIdentifier = request.DeviceIdentifier,
                                                  TransactionDateTime = request.TransactionDateTime,
                                                  TransactionType = 1,
            IsTrainingMode = useTrainingMode
        };
        Int64 transactionNumber = await this.DatabaseContext.CreateTransaction(transactionRecord);

        return (transactionRecord, transactionNumber);
    }

    private async Task<(TransactionRecord transactionRecord, Int64 transactionNumber)> CreateTransactionRecord(PerformMobileTopupRequest request,
        Boolean useTrainingMode,
                                                                                                               CancellationToken cancellationToken)
    {
        TransactionRecord transactionRecord = new TransactionRecord
                                              {
                                                  ApplicationVersion = request.ApplicationVersion,
                                                  DeviceIdentifier = request.DeviceIdentifier,
                                                  TransactionDateTime = request.TransactionDateTime,
                                                  TransactionType = 2,
                                                  Amount = request.TopupAmount,
                                                  ProductId = request.ProductId,
                                                  ContractId = request.ContractId,
                                                  CustomerAccountNumber = request.CustomerAccountNumber,
                                                  CustomerEmailAddress = request.CustomerEmailAddress,
                                                  OperatorIdentifier = request.OperatorIdentifier,
                                                  IsTrainingMode = useTrainingMode
                                              };
        Int64 transactionNumber = await this.DatabaseContext.CreateTransaction(transactionRecord);

        return (transactionRecord, transactionNumber);
    }

    private async Task<(TransactionRecord transactionRecord, Int64 transactionNumber)> CreateTransactionRecord(PerformVoucherIssueRequest request,
        Boolean useTrainingMode,
                                                                                                               CancellationToken cancellationToken)
    {
        TransactionRecord transactionRecord = new TransactionRecord
                                              {
                                                  ApplicationVersion = request.ApplicationVersion,
                                                  DeviceIdentifier = request.DeviceIdentifier,
                                                  TransactionDateTime = request.TransactionDateTime,
                                                  TransactionType = 2,
                                                  Amount = request.VoucherAmount,
                                                  ProductId = request.ProductId,
                                                  ContractId = request.ContractId,
                                                  RecipientEmailAddress = request.RecipientEmailAddress,
                                                  RecipientMobileNumber = request.RecipientMobileNumber,
                                                  CustomerEmailAddress = request.CustomerEmailAddress,
                                                  OperatorIdentifier = request.OperatorIdentifier,
                                                  IsTrainingMode = useTrainingMode
                                              };
        Int64 transactionNumber = await this.DatabaseContext.CreateTransaction(transactionRecord);

        return (transactionRecord, transactionNumber);
    }

    private async Task UpdateTransactionRecord(TransactionRecord transactionRecord,
                                               PerformLogonResponseModel result,
                                               CancellationToken cancellationToken)
    {
        transactionRecord.IsSuccessful = result.IsSuccessful;
        transactionRecord.ResponseMessage = result.ResponseMessage;
        transactionRecord.EstateId = result.EstateId;
        transactionRecord.MerchantId = result.MerchantId;

        await this.DatabaseContext.UpdateTransaction(transactionRecord);
    }

    private async Task UpdateTransactionRecord(TransactionRecord transactionRecord,
                                               Boolean result,
                                               CancellationToken cancellationToken)
    {
        transactionRecord.IsSuccessful = result;

        await this.DatabaseContext.UpdateTransaction(transactionRecord);
    }

    public async Task<PerformLogonResponseModel> Handle(LogonTransactionRequest request,
                                                        CancellationToken cancellationToken)
    {
        this.MemoryCacheService.TryGetValue("UseTrainingMode", out Boolean useTrainingMode);
        (TransactionRecord transactionRecord, Int64 transactionNumber) transaction = await this.CreateTransactionRecord(request, useTrainingMode, cancellationToken);

        // TODO: Factory
        PerformLogonRequestModel model = new PerformLogonRequestModel
                                         {
                                             ApplicationVersion = request.ApplicationVersion,
                                             DeviceIdentifier = request.DeviceIdentifier,
                                             TransactionDateTime = request.TransactionDateTime,
                                             TransactionNumber = transaction.transactionNumber.ToString()
                                         };
                
        var transactionService = this.TransactionServiceResolver(useTrainingMode);
        PerformLogonResponseModel result = await transactionService.PerformLogon(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord, result, cancellationToken);

        return result;
    }

    #endregion

    public async Task<Boolean> Handle(PerformVoucherIssueRequest request,
                                      CancellationToken cancellationToken)
    {
        this.MemoryCacheService.TryGetValue("UseTrainingMode", out Boolean useTrainingMode);
        (TransactionRecord transactionRecord, Int64 transactionNumber) transaction = await this.CreateTransactionRecord(request, useTrainingMode, cancellationToken);
        // TODO: Factory
        PerformVoucherIssueRequestModel model = new PerformVoucherIssueRequestModel
                                                {
                                                    ApplicationVersion = request.ApplicationVersion,
                                                    ContractId = request.ContractId,
                                                    RecipientEmailAddress = request.RecipientEmailAddress,
                                                    RecipientMobileNumber = request.RecipientMobileNumber,
                                                    CustomerEmailAddress = request.CustomerEmailAddress,
                                                    DeviceIdentifier = request.DeviceIdentifier,
                                                    OperatorIdentifier = request.OperatorIdentifier,
                                                    ProductId = request.ProductId,
                                                    VoucherAmount = request.VoucherAmount,
                                                    TransactionDateTime = request.TransactionDateTime,
                                                    TransactionNumber = transaction.transactionNumber.ToString()
                                                };

        var transactionService = this.TransactionServiceResolver(useTrainingMode);
        Boolean result = await transactionService.PerformVoucherIssue(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord, result, cancellationToken);

        return result;
    }

    public async Task<Boolean> Handle(PerformReconciliationRequest request,
                                      CancellationToken cancellationToken)
    {
        this.MemoryCacheService.TryGetValue("UseTrainingMode", out Boolean useTrainingMode);

        List<TransactionRecord> storedTransactions = await this.DatabaseContext.GetTransactions();

        if (storedTransactions.Any() == false)
        {
            return true;
        }

        // Filter based on training mode
        storedTransactions = storedTransactions.Where(t => t.IsTrainingMode == useTrainingMode).ToList();

        // TODO: convert these to operator totals
        List<OperatorTotalModel> operatorTotals = (from t in storedTransactions
                                                   where t.IsSuccessful = true &&
                                                                          t.TransactionType != 1 // Filter out logons
                                                                          && t.IsTrainingMode == useTrainingMode
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
        var transactionService = this.TransactionServiceResolver(useTrainingMode);
        Boolean result = await transactionService.PerformReconciliation(model, cancellationToken);

        // Clear store (if successful)
        if (result)
        {
            await this.DatabaseContext.ClearStoredTransactions(storedTransactions);
        }

        return result;
    }
}