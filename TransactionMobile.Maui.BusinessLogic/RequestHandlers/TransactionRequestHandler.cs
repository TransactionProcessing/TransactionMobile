namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers;

using Database;
using MediatR;
using Models;
using Requests;
using Services;

public class TransactionRequestHandler : IRequestHandler<PerformMobileTopupRequest, Boolean>, 
                                         IRequestHandler<LogonTransactionRequest, PerformLogonResponseModel>,
                                         IRequestHandler<PerformVoucherIssueRequest, Boolean>
{
    #region Fields

    private readonly ITransactionService TransactionService;

    private readonly IDatabaseContext DatabaseContext;

    #endregion

    #region Constructors

    public TransactionRequestHandler(ITransactionService transactionService, IDatabaseContext databaseContext)
    {
        this.TransactionService = transactionService;
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
}