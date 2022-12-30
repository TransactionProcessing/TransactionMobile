using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Models
{
    public class PerformMobileTopupResponseModel
    {
        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public Guid EstateId { get; set; }

        public Guid MerchantId { get; set; }

        public Boolean IsSuccessful => ResponseCode == "0000";
    }

    public class PerformVoucherIssueResponseModel
    {
        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public Guid EstateId { get; set; }

        public Guid MerchantId { get; set; }

        public Boolean IsSuccessful => ResponseCode == "0000";
    }

    public class PerformBillPaymentMakePaymentResponseModel
    {
        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public Guid EstateId { get; set; }

        public Guid MerchantId { get; set; }
        public Boolean IsSuccessful => ResponseCode == "0000";
    }

    public class PerformReconciliationResponseModel
    {
        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public Guid EstateId { get; set; }

        public Guid MerchantId { get; set; }
        public Boolean IsSuccessful => ResponseCode == "0000";
    }
}
