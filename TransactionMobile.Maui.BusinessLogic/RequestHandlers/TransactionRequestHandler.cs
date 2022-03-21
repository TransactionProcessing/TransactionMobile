namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers;

using Database;
using MediatR;
using Models;
using Requests;
using Services;

public class TransactionRequestHandler : IRequestHandler<PerformMobileTopupRequest, Boolean>,
                                         IRequestHandler<LogonTransactionRequest, PerformLogonResponseModel>,
                                         IRequestHandler<PerformVoucherIssueRequest, Boolean>,
                                         IRequestHandler<PerformReconciliationRequest, Boolean>,
                                         IRequestHandler<UploadLogsRequest,Boolean>
{
    #region Fields

    private readonly ITransactionService TransactionService;
    private readonly IConfigurationService ConfigurationService;
    private readonly IDatabaseContext DatabaseContext;

    #endregion

    #region Constructors

    public TransactionRequestHandler(ITransactionService transactionService,
                                     IConfigurationService configurationService,
                                     IDatabaseContext databaseContext)
    {
        this.TransactionService = transactionService;
        this.ConfigurationService = configurationService;
        this.DatabaseContext = databaseContext;
    }

    #endregion

    #region Methods

    public async Task<Boolean> Handle(PerformMobileTopupRequest request,
                                      CancellationToken cancellationToken)
    {
        (TransactionRecord transactionRecord, Int64 transactionNumber) transaction = await this.CreateTransactionRecord(request, cancellationToken);

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

        Boolean result = await this.TransactionService.PerformMobileTopup(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord, result, cancellationToken);

        return result;
    }

    private async Task<(TransactionRecord transactionRecord, Int64 transactionNumber)> CreateTransactionRecord(LogonTransactionRequest request,
                                                                                                               CancellationToken cancellationToken)
    {
        TransactionRecord transactionRecord = new TransactionRecord
                                              {
                                                  ApplicationVersion = request.ApplicationVersion,
                                                  DeviceIdentifier = request.DeviceIdentifier,
                                                  TransactionDateTime = request.TransactionDateTime,
                                                  TransactionType = 1
                                              };
        Int64 transactionNumber = await this.DatabaseContext.CreateTransaction(transactionRecord);

        return (transactionRecord, transactionNumber);
    }

    private async Task<(TransactionRecord transactionRecord, Int64 transactionNumber)> CreateTransactionRecord(PerformMobileTopupRequest request,
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
                                                  OperatorIdentifier = request.OperatorIdentifier
                                              };
        Int64 transactionNumber = await this.DatabaseContext.CreateTransaction(transactionRecord);

        return (transactionRecord, transactionNumber);
    }

    private async Task<(TransactionRecord transactionRecord, Int64 transactionNumber)> CreateTransactionRecord(PerformVoucherIssueRequest request,
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
                                                  OperatorIdentifier = request.OperatorIdentifier
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
        (TransactionRecord transactionRecord, Int64 transactionNumber) transaction = await this.CreateTransactionRecord(request, cancellationToken);

        // TODO: Factory
        PerformLogonRequestModel model = new PerformLogonRequestModel
                                         {
                                             ApplicationVersion = request.ApplicationVersion,
                                             DeviceIdentifier = request.DeviceIdentifier,
                                             TransactionDateTime = request.TransactionDateTime,
                                             TransactionNumber = transaction.transactionNumber.ToString()
                                         };

        PerformLogonResponseModel result = await this.TransactionService.PerformLogon(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord, result, cancellationToken);

        return result;
    }

    #endregion

    public async Task<Boolean> Handle(PerformVoucherIssueRequest request,
                                      CancellationToken cancellationToken)
    {
        (TransactionRecord transactionRecord, Int64 transactionNumber) transaction = await this.CreateTransactionRecord(request, cancellationToken);
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

        Boolean result = await this.TransactionService.PerformVoucherIssue(model, cancellationToken);

        await this.UpdateTransactionRecord(transaction.transactionRecord, result, cancellationToken);

        return result;
    }

    public async Task<Boolean> Handle(PerformReconciliationRequest request,
                                      CancellationToken cancellationToken)
    {
        List<TransactionRecord> storedTransactions = await this.DatabaseContext.GetTransactions();

        if (storedTransactions.Any() == false)
        {
            return true;
        }

        // TODO: convert these to operator totals
        List<OperatorTotalModel> operatorTotals = (from t in storedTransactions
                                                   where t.IsSuccessful = true &&
                                                                          t.TransactionType != 1 // Filter out logons
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
        Boolean result = await this.TransactionService.PerformReconciliation(model, cancellationToken);

        // Clear store (if successful)
        if (result)
        {
            await this.DatabaseContext.ClearStoredTransactions();
        }

        return result;
    }

    public async Task<Boolean> Handle(UploadLogsRequest request, CancellationToken cancellationToken)
    {
        while (true)
        {
            var logEntries = await this.DatabaseContext.GetLogMessages(10); // TODO: Configurable batch size

            if (logEntries.Any() == false)
            {
                break;
            }

            // TODO: Translate log messages
            List<Models.LogMessage> logMessageModels = new List<Models.LogMessage>();

            logEntries.ForEach(l => logMessageModels.Add(new Models.LogMessage
            {
                LogLevel = l.LogLevel,
                Message = l.Message,
                EntryDateTime = l.EntryDateTime,
                Id = l.Id
            }));

            await this.ConfigurationService.PostDiagnosticLogs(request.DeviceIdentifier, logMessageModels, CancellationToken.None);

            // Clear the logs that have been uploaded
            await this.DatabaseContext.RemoveUploadedMessages(logEntries);            
        }

        return true;
    }
}