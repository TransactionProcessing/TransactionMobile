namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using RequestHandlers;
using SimpleResults;
using TransactionMobile.Maui.BusinessLogic.Models;
using TransactionProcessorACL.DataTransferObjects.Responses;

public class PerformMobileTopupRequest : IRequest<Result<PerformMobileTopupResponseModel>>
{
    #region Constructors

    private PerformMobileTopupRequest(DateTime transactionDateTime,
                                      Guid contractId,
                                      Guid productId,
                                      Guid operatorId,
                                      String customerAccountNumber,
                                      Decimal topupAmount,
                                      String customerEmailAddress)
    {
        this.TransactionDateTime = transactionDateTime;
        this.ContractId = contractId;
        this.ProductId = productId;
        this.OperatorId = operatorId;
        this.CustomerAccountNumber = customerAccountNumber;
        this.TopupAmount = topupAmount;
        this.CustomerEmailAddress = customerEmailAddress;
    }

    #endregion

    #region Properties

    public Guid ContractId { get; }

    public String CustomerAccountNumber { get; }

    public String CustomerEmailAddress { get; }

    public Guid OperatorId { get; }

    public Guid ProductId { get; }

    public Decimal TopupAmount { get; }

    public DateTime TransactionDateTime { get; }

    #endregion

    #region Methods

    public static PerformMobileTopupRequest Create(DateTime transactionDateTime,
                                                   Guid contractId,
                                                   Guid productId,
                                                   Guid operatorId,
                                                   String customerAccountNumber,
                                                   Decimal topupAmount,
                                                   String customerEmailAddress)
    {
        return new PerformMobileTopupRequest(transactionDateTime,
                                             contractId,
                                             productId,
                                             operatorId,
                                             customerAccountNumber,
                                             topupAmount,
                                             customerEmailAddress);
    }

    #endregion
}