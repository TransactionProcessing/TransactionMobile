namespace TransactionMobile.Maui.BusinessLogic.Common;

using Database;
using Models;
using Requests;
using TransactionProcessorACL.DataTransferObjects;
using ViewModels.Transactions;

public static class Extensions
{
    public static TransactionRecord ToTransactionRecord(this LogonTransactionRequest request,
                                                        Boolean useTrainingMode) {
        TransactionRecord transactionRecord = new TransactionRecord {
                                                                        TransactionDateTime = request.TransactionDateTime,
                                                                        TransactionType = 1,
                                                                        IsTrainingMode = useTrainingMode
                                                                    };

        return transactionRecord;
    }

    public static TransactionRecord ToTransactionRecord(this PerformMobileTopupRequest request,
                                                        Boolean useTrainingMode) {
        TransactionRecord transactionRecord = new TransactionRecord {
                                                                        TransactionDateTime = request.TransactionDateTime,
                                                                        TransactionType = 2,
                                                                        Amount = request.TopupAmount,
                                                                        ProductId = request.ProductId,
                                                                        ContractId = request.ContractId,
                                                                        CustomerAccountNumber = request.CustomerAccountNumber,
                                                                        CustomerEmailAddress = request.CustomerEmailAddress,
                                                                        OperatorIdentifier = request.OperatorIdentifier,
                                                                        IsTrainingMode = useTrainingMode,
                                                                    };

        return transactionRecord;
    }

    public static TransactionRecord ToTransactionRecord(this PerformVoucherIssueRequest request,
                                                        Boolean useTrainingMode) {
        TransactionRecord transactionRecord = new TransactionRecord {
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

        return transactionRecord;
    }

    public static TransactionRecord ToTransactionRecord(this PerformBillPaymentGetAccountRequest request,
                                                        Boolean useTrainingMode) {
        TransactionRecord transactionRecord = new TransactionRecord
                                              {
                                                  TransactionDateTime = request.TransactionDateTime,
                                                  TransactionType = 2,
                                                  Amount = 0,
                                                  ProductId = request.ProductId,
                                                  ContractId = request.ContractId,
                                                  OperatorIdentifier = request.OperatorIdentifier,
                                                  IsTrainingMode = useTrainingMode,
                                                  CustomerAccountNumber = request.CustomerAccountNumber,
                                              };
        return transactionRecord;
    }

    public static TransactionRecord ToTransactionRecord(this PerformBillPaymentMakePaymentRequest request,
                                                        Boolean useTrainingMode) {
        TransactionRecord transactionRecord = new TransactionRecord
                                              {
                                                  TransactionDateTime = request.TransactionDateTime,
                                                  TransactionType = 2,
                                                  Amount = 0,
                                                  ProductId = request.ProductId,
                                                  ContractId = request.ContractId,
                                                  OperatorIdentifier = request.OperatorIdentifier,
                                                  IsTrainingMode = useTrainingMode,
                                                  CustomerAccountNumber = request.CustomerAccountNumber
                                              };
        return transactionRecord;
    }


    public static TransactionRecord UpdateFrom(this TransactionRecord transactionRecord,
                                               PerformLogonResponseModel result)
    {
        transactionRecord.IsSuccessful = result.IsSuccessful;
        transactionRecord.ResponseMessage = result.ResponseMessage;
        transactionRecord.EstateId = result.EstateId;
        transactionRecord.MerchantId = result.MerchantId;

        return transactionRecord;
    }

    public static TransactionRecord UpdateFrom(this TransactionRecord transactionRecord,
                                               Boolean result)
    {
        transactionRecord.IsSuccessful = result;

        return transactionRecord;
    }

    public static LogonTransactionRequestMessage ToLogonTransactionRequest(this PerformLogonRequestModel model)
    {
        LogonTransactionRequestMessage logonTransactionRequest = new LogonTransactionRequestMessage
        {
            ApplicationVersion = model.ApplicationVersion,
            DeviceIdentifier = model.DeviceIdentifier,
            TransactionDateTime = model.TransactionDateTime,
            TransactionNumber = model.TransactionNumber
        };

        return logonTransactionRequest;
    }

    public static ReconciliationRequestMessage ToReconciliationRequest(this PerformReconciliationRequestModel model)
    {
        ReconciliationRequestMessage reconciliationRequest = new ReconciliationRequestMessage
        {
            ApplicationVersion = model.ApplicationVersion,
            TransactionDateTime = model.TransactionDateTime,
            DeviceIdentifier = model.DeviceIdentifier,
            TransactionCount = model.TransactionCount,
            TransactionValue = model.TransactionValue,
            OperatorTotals = new List<OperatorTotalRequest>()
        };
        foreach (OperatorTotalModel modelOperatorTotal in model.OperatorTotals)
        {
            reconciliationRequest.OperatorTotals.Add(new OperatorTotalRequest
            {
                OperatorIdentifier = modelOperatorTotal.OperatorIdentifier,
                TransactionValue = modelOperatorTotal.TransactionValue,
                ContractId = modelOperatorTotal.ContractId,
                TransactionCount = modelOperatorTotal.TransactionCount
            });
        }

        return reconciliationRequest;
    }

    public static SaleTransactionRequestMessage ToSaleTransactionRequest(this PerformVoucherIssueRequestModel model)
    {
        SaleTransactionRequestMessage saleTransactionRequest = new SaleTransactionRequestMessage
        {
            ProductId = model.ProductId,
            OperatorIdentifier = model.OperatorIdentifier,
            ApplicationVersion = model.ApplicationVersion,
            DeviceIdentifier = model.DeviceIdentifier,
            ContractId = model.ContractId,
            TransactionDateTime = model.TransactionDateTime,
            CustomerEmailAddress = model.CustomerEmailAddress,
            TransactionNumber = model.TransactionNumber
        };

        // Add the additional request data
        saleTransactionRequest.AdditionalRequestMetaData = new Dictionary<String, String> {
                                                                                                  {"Amount", model.VoucherAmount.ToString()},
                                                                                                  {"RecipientEmail", model.RecipientEmailAddress},
                                                                                                  {"RecipientMobile", model.RecipientMobileNumber}
                                                                                              };

        return saleTransactionRequest;
    }

    public static SaleTransactionRequestMessage ToSaleTransactionRequest(this PerformMobileTopupRequestModel model)
    {
        SaleTransactionRequestMessage saleTransactionRequest = new SaleTransactionRequestMessage
        {
            ProductId = model.ProductId,
            OperatorIdentifier = model.OperatorIdentifier,
            ApplicationVersion = model.ApplicationVersion,
            DeviceIdentifier = model.DeviceIdentifier,
            ContractId = model.ContractId,
            TransactionDateTime = model.TransactionDateTime,
            CustomerEmailAddress = model.CustomerEmailAddress,
            TransactionNumber = model.TransactionNumber
        };

        // Add the additional request data
        saleTransactionRequest.AdditionalRequestMetaData = new Dictionary<String, String> {
                                                                                                  {"Amount", model.TopupAmount.ToString()},
                                                                                                  {"CustomerAccountNumber", model.CustomerAccountNumber}
                                                                                              };

        return saleTransactionRequest;
    }

    public static SaleTransactionRequestMessage ToSaleTransactionRequest(this PerformBillPaymentGetAccountModel model) {
        SaleTransactionRequestMessage saleTransactionRequest = new SaleTransactionRequestMessage
                                                               {
                                                                   ProductId = model.ProductId,
                                                                   OperatorIdentifier = model.OperatorIdentifier,
                                                                   ApplicationVersion = model.ApplicationVersion,
                                                                   DeviceIdentifier = model.DeviceIdentifier,
                                                                   ContractId = model.ContractId,
                                                                   TransactionDateTime = model.TransactionDateTime,
                                                                   TransactionNumber = model.TransactionNumber
                                                               };
        // Add the additional request data
        saleTransactionRequest.AdditionalRequestMetaData = new Dictionary<String, String> {
                                                                                              {"CustomerAccountNumber", model.CustomerAccountNumber},
                                                                                              {"PataPawaPostPaidMessageType", "VerifyAccount"}
                                                                                          };

        return saleTransactionRequest;
    }

    public static SaleTransactionRequestMessage ToSaleTransactionRequest(this PerformBillPaymentMakePaymentModel model)
    {
        SaleTransactionRequestMessage saleTransactionRequest = new SaleTransactionRequestMessage
                                                               {
                                                                   ProductId = model.ProductId,
                                                                   OperatorIdentifier = model.OperatorIdentifier,
                                                                   ApplicationVersion = model.ApplicationVersion,
                                                                   DeviceIdentifier = model.DeviceIdentifier,
                                                                   ContractId = model.ContractId,
                                                                   TransactionDateTime = model.TransactionDateTime,
                                                                   TransactionNumber = model.TransactionNumber
                                                               };
        // Add the additional request data
        saleTransactionRequest.AdditionalRequestMetaData = new Dictionary<String, String> {
                                                                                              {"CustomerAccountNumber", model.CustomerAccountNumber},
                                                                                              {"CustomerName", model.CustomerAccountName},
                                                                                              {"MobileNumber", model.CustomerMobileNumber},
                                                                                              {"Amount", model.PaymentAmount.ToString()},
                                                                                              {"PataPawaPostPaidMessageType", "ProcessBill"}
                                                                                          };

        return saleTransactionRequest;
    }

    public static T ExtractFieldFromMetadata<T>(this Dictionary<String, String> additionalTransactionMetadata,
                                                String fieldName)
    {
        // Create a case insensitive version of the dictionary
        Dictionary<String, String> caseInsensitiveDictionary = new Dictionary<String, String>(StringComparer.InvariantCultureIgnoreCase);
        foreach (KeyValuePair<String, String> keyValuePair in additionalTransactionMetadata)
        {
            caseInsensitiveDictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }

        if (caseInsensitiveDictionary.ContainsKey(fieldName))
        {
            String fieldData = caseInsensitiveDictionary[fieldName];
            Type t = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            Object safeValue = (fieldData == null) ? null : Convert.ChangeType(fieldData, t);
            return (T)safeValue;
        }

        return default(T);
    }

    public static BillDetails ToBillDetails(this Dictionary<String, String> additionalTransactionMetadata)
    {
        BillDetails billDetails = new BillDetails();

        billDetails.AccountName = additionalTransactionMetadata.ExtractFieldFromMetadata<String>("customerAccountName");
        billDetails.AccountNumber = additionalTransactionMetadata.ExtractFieldFromMetadata<String>("customerAccountNumber");
        billDetails.Balance = additionalTransactionMetadata.ExtractFieldFromMetadata<String>("customerBillBalance");
        billDetails.DueDate = additionalTransactionMetadata.ExtractFieldFromMetadata<String>("customerBillDueDate");

        return billDetails;
    }

}