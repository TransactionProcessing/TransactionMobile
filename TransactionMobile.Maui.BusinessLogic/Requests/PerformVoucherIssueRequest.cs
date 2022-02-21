namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;

public class PerformVoucherIssueRequest : IRequest<Boolean>
{
    #region Constructors

    private PerformVoucherIssueRequest(DateTime transactionDateTime,
                                       String transactionNumber,
                                       String deviceIdentifier,
                                       String applicationVersion,
                                       Guid contractId,
                                       Guid productId,
                                       String operatorIdentifier,
                                       String recipientMobileNumber,
                                       String recipientEmailAddress,
                                       Decimal voucherAmount,
                                       String customerEmailAddress)
    {
        this.ApplicationVersion = applicationVersion;
        this.DeviceIdentifier = deviceIdentifier;
        this.TransactionDateTime = transactionDateTime;
        this.TransactionNumber = transactionNumber;
        this.ContractId = contractId;
        this.ProductId = productId;
        this.OperatorIdentifier = operatorIdentifier;
        this.RecipientEmailAddress = recipientEmailAddress;
        this.RecipientMobileNumber = recipientMobileNumber;
        this.VoucherAmount = voucherAmount;
        this.CustomerEmailAddress = customerEmailAddress;
    }

    #endregion

    #region Properties

    public String ApplicationVersion { get; }

    public Guid ContractId { get; }

    public String RecipientMobileNumber { get; }

    public String RecipientEmailAddress { get; }

    public String CustomerEmailAddress { get; }

    public String DeviceIdentifier { get; }

    public String OperatorIdentifier { get; }

    public Guid ProductId { get; }

    public DateTime TransactionDateTime { get; }

    public String TransactionNumber { get; }

    public Decimal VoucherAmount { get; }

    #endregion

    #region Methods

    public static PerformVoucherIssueRequest Create(DateTime transactionDateTime,
                                                    String transactionNumber,
                                                    String deviceIdentifier,
                                                    String applicationVersion,
                                                    Guid contractId,
                                                    Guid productId,
                                                    String operatorIdentifier,
                                                    String recipientMobileNumber,
                                                    String recipientEmailAddress,
                                                    Decimal voucherAmount,
                                                    String customerEmailAddress)
    {
        return new PerformVoucherIssueRequest(transactionDateTime,
                                              transactionNumber,
                                              deviceIdentifier,
                                              applicationVersion,
                                              contractId,
                                              productId,
                                              operatorIdentifier,
                                              recipientMobileNumber,
                                              recipientEmailAddress,
                                              voucherAmount,
                                              customerEmailAddress);
    }

    #endregion
}