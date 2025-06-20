using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public class PerformVoucherIssueRequest : IRequest<Result<PerformVoucherIssueResponseModel>>
{
    #region Constructors

    private PerformVoucherIssueRequest(DateTime transactionDateTime,
                                       Guid contractId,
                                       Guid productId,
                                       Guid operatorId,
                                       String recipientMobileNumber,
                                       String recipientEmailAddress,
                                       Decimal voucherAmount,
                                       String customerEmailAddress)
    {
        this.TransactionDateTime = transactionDateTime;
        this.ContractId = contractId;
        this.ProductId = productId;
        this.OperatorId = operatorId;
        this.RecipientEmailAddress = recipientEmailAddress;
        this.RecipientMobileNumber = recipientMobileNumber;
        this.VoucherAmount = voucherAmount;
        this.CustomerEmailAddress = customerEmailAddress;
    }

    #endregion

    #region Properties

    public Guid ContractId { get; }

    public String RecipientMobileNumber { get; }

    public String RecipientEmailAddress { get; }

    public String CustomerEmailAddress { get; }

    public Guid OperatorId { get; }

    public Guid ProductId { get; }

    public DateTime TransactionDateTime { get; }

    public Decimal VoucherAmount { get; }

    #endregion

    #region Methods

    public static PerformVoucherIssueRequest Create(DateTime transactionDateTime,
                                                    Guid contractId,
                                                    Guid productId,
                                                    Guid operatorId,
                                                    String recipientMobileNumber,
                                                    String recipientEmailAddress,
                                                    Decimal voucherAmount,
                                                    String customerEmailAddress)
    {
        return new PerformVoucherIssueRequest(transactionDateTime,
                                              contractId,
                                              productId,
                                              operatorId,
                                              recipientMobileNumber,
                                              recipientEmailAddress,
                                              voucherAmount,
                                              customerEmailAddress);
    }

    #endregion
}