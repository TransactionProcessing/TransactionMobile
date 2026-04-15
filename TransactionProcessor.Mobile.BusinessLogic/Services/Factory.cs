using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessorACL.DataTransferObjects.Responses;

namespace TransactionProcessor.Mobile.BusinessLogic.Services;

public static class Factory {
    public static PerformBillPaymentGetAccountResponseModel ToPerformBillPaymentGetAccountResponseModel(SaleTransactionResponseMessage responseMessage) {
        return new PerformBillPaymentGetAccountResponseModel() {
            BillDetails = responseMessage.AdditionalResponseMetadata.ToBillDetails()
        };
    }

    public static PerformBillPaymentMakePaymentResponseModel ToPerformBillPaymentMakePaymentResponseModel(SaleTransactionResponseMessage responseMessage) {
        return new()
        {
            EstateId = responseMessage.EstateId,
            MerchantId = responseMessage.MerchantId,
            ResponseCode = responseMessage.ResponseCode,
            ResponseMessage = responseMessage.ResponseMessage
        };
    }

    public static PerformBillPaymentGetMeterResponseModel ToPerformBillPaymentGetMeterResponseModel(SaleTransactionResponseMessage responseMessage, String meterNumber) {
        MeterDetails meterDetails = responseMessage.AdditionalResponseMetadata switch{
            null => null,
            _ => responseMessage.AdditionalResponseMetadata.ToMeterDetails(meterNumber)
        };
        return new PerformBillPaymentGetMeterResponseModel() {
            MeterDetails = meterDetails
        };
    }

    public static PerformLogonResponseModel ToPerformLogonResponseModel(LogonTransactionResponseMessage responseMessage) {
        return new()
        {
            EstateId = responseMessage.EstateId,
            MerchantId = responseMessage.MerchantId,
            ResponseCode = responseMessage.ResponseCode,
            ResponseMessage = responseMessage.ResponseMessage,
        };
    }

    public static PerformMobileTopupResponseModel ToPerformMobileTopupResponseModel(SaleTransactionResponseMessage responseMessage)
    {
        return new()
        {
            EstateId = responseMessage.EstateId,
            MerchantId = responseMessage.MerchantId,
            ResponseCode = responseMessage.ResponseCode,
            ResponseMessage = responseMessage.ResponseMessage,
        };
    }

    public static PerformReconciliationResponseModel ToPerformReconciliationResponseModel(ReconciliationResponseMessage responseMessage) {
        return new()
        {
            EstateId = responseMessage.EstateId,
            MerchantId = responseMessage.MerchantId,
            ResponseCode = responseMessage.ResponseCode,
            ResponseMessage = responseMessage.ResponseMessage
        };
    }

    public static PerformVoucherIssueResponseModel ToPerformVoucherIssueResponseModel(SaleTransactionResponseMessage responseMessage) {
        return new()
        {
            EstateId = responseMessage.EstateId,
            MerchantId = responseMessage.MerchantId,
            ResponseCode = responseMessage.ResponseCode,
            ResponseMessage = responseMessage.ResponseMessage
        };
    }
}