namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers;

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

    #endregion

    #region Constructors

    public TransactionRequestHandler(ITransactionService transactionService)
    {
        this.TransactionService = transactionService;
    }

    #endregion

    #region Methods

    public async Task<Boolean> Handle(PerformMobileTopupRequest request,
                                      CancellationToken cancellationToken)
    {
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

        return result;
    }

    public async Task<PerformLogonResponseModel> Handle(LogonTransactionRequest request,
                                                        CancellationToken cancellationToken)
    {
        // TODO: Factory
        PerformLogonRequestModel model = new PerformLogonRequestModel
                                         {
                                             ApplicationVersion = request.ApplicationVersion,
                                             DeviceIdentifier = request.DeviceIdentifier,
                                             TransactionDateTime = request.TransactionDateTime,
                                             TransactionNumber = request.TransactionNumber
                                         };

        PerformLogonResponseModel result = await this.TransactionService.PerformLogon(model, cancellationToken);

        return result;
    }

    #endregion

    public async Task<Boolean> Handle(PerformVoucherIssueRequest request,
                                      CancellationToken cancellationToken)
    {
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
                                                   TransactionNumber = request.TransactionNumber
                                               };

        Boolean result = await this.TransactionService.PerformVoucherIssue(model, cancellationToken);

        return result;
    }
}