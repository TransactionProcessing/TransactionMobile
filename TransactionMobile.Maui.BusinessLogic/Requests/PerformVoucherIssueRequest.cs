namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using Models;
using RequestHandlers;
using SimpleResults;
using TransactionProcessorACL.DataTransferObjects.Responses;

public class PerformVoucherIssueRequest : IRequest<Result<PerformVoucherIssueResponseModel>>
{
    #region Constructors

    private PerformVoucherIssueRequest(DateTime transactionDateTime,
                                       Guid contractId,
                                       Guid productId,
                                       String operatorIdentifier,
                                       String recipientMobileNumber,
                                       String recipientEmailAddress,
                                       Decimal voucherAmount,
                                       String customerEmailAddress)
    {
        this.TransactionDateTime = transactionDateTime;
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

    public Guid ContractId { get; }

    public String RecipientMobileNumber { get; }

    public String RecipientEmailAddress { get; }

    public String CustomerEmailAddress { get; }

    public String OperatorIdentifier { get; }

    public Guid ProductId { get; }

    public DateTime TransactionDateTime { get; }

    public Decimal VoucherAmount { get; }

    #endregion

    #region Methods

    public static PerformVoucherIssueRequest Create(DateTime transactionDateTime,
                                                    Guid contractId,
                                                    Guid productId,
                                                    String operatorIdentifier,
                                                    String recipientMobileNumber,
                                                    String recipientEmailAddress,
                                                    Decimal voucherAmount,
                                                    String customerEmailAddress)
    {
        return new PerformVoucherIssueRequest(transactionDateTime,
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