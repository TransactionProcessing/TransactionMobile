namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;

public class PerformMobileTopupRequest : IRequest<Boolean>
{
    #region Constructors

    private PerformMobileTopupRequest(DateTime transactionDateTime,
                                      Guid contractId,
                                      Guid productId,
                                      String operatorIdentifier,
                                      String customerAccountNumber,
                                      Decimal topupAmount,
                                      String customerEmailAddress)
    {
        this.TransactionDateTime = transactionDateTime;
        this.ContractId = contractId;
        this.ProductId = productId;
        this.OperatorIdentifier = operatorIdentifier;
        this.CustomerAccountNumber = customerAccountNumber;
        this.TopupAmount = topupAmount;
        this.CustomerEmailAddress = customerEmailAddress;
    }

    #endregion

    #region Properties

    public Guid ContractId { get; }

    public String CustomerAccountNumber { get; }

    public String CustomerEmailAddress { get; }

    public String OperatorIdentifier { get; }

    public Guid ProductId { get; }

    public Decimal TopupAmount { get; }

    public DateTime TransactionDateTime { get; }

    #endregion

    #region Methods

    public static PerformMobileTopupRequest Create(DateTime transactionDateTime,
                                                   Guid contractId,
                                                   Guid productId,
                                                   String operatorIdentifier,
                                                   String customerAccountNumber,
                                                   Decimal topupAmount,
                                                   String customerEmailAddress)
    {
        return new PerformMobileTopupRequest(transactionDateTime,
                                             contractId,
                                             productId,
                                             operatorIdentifier,
                                             customerAccountNumber,
                                             topupAmount,
                                             customerEmailAddress);
    }

    #endregion
}