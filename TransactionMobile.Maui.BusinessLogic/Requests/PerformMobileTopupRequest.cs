namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;

public class PerformMobileTopupRequest : IRequest<Boolean>
{
    #region Constructors

    private PerformMobileTopupRequest(DateTime transactionDateTime,
                                      String transactionNumber,
                                      String deviceIdentifier,
                                      String applicationVersion,
                                      Guid contractId,
                                      Guid productId,
                                      String operatorIdentifier,
                                      String customerAccountNumber,
                                      Decimal topupAmount,
                                      String customerEmailAddress)
    {
        this.ApplicationVersion = applicationVersion;
        this.DeviceIdentifier = deviceIdentifier;
        this.TransactionDateTime = transactionDateTime;
        this.TransactionNumber = transactionNumber;
        this.ContractId = contractId;
        this.ProductId = productId;
        this.OperatorIdentifier = operatorIdentifier;
        this.CustomerAccountNumber = customerAccountNumber;
        this.TopupAmount = topupAmount;
        this.CustomerEmailAddress = customerEmailAddress;
    }

    #endregion

    public static PerformMobileTopupRequest Create(DateTime transactionDateTime,
                                                   String transactionNumber,
                                                   String deviceIdentifier,
                                                   String applicationVersion,
                                                   Guid contractId,
                                                   Guid productId,
                                                   String operatorIdentifier,
                                                   String customerAccountNumber,
                                                   Decimal topupAmount,
                                                   String customerEmailAddress)
    {
        return new PerformMobileTopupRequest(transactionDateTime,
                                             transactionNumber,
                                             deviceIdentifier,
                                             applicationVersion,
                                             contractId,
                                             productId,
                                             operatorIdentifier,
                                             customerAccountNumber,
                                             topupAmount,
                                             customerEmailAddress);
    }

    #region Properties

    public String ApplicationVersion { get; }

    public Guid ContractId { get; }

    public String CustomerAccountNumber { get; }

    public String CustomerEmailAddress { get; }

    public String DeviceIdentifier { get; }

    public String OperatorIdentifier { get; }

    public Guid ProductId { get; }

    public Decimal TopupAmount { get; }

    public DateTime TransactionDateTime { get; }

    public String TransactionNumber { get; }

    #endregion
}