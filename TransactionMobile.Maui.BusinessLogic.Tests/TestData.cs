using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Tests
{
    using Database;
    using Microsoft.Win32.SafeHandles;
    using Models;
    using ViewModels.Transactions;

    public static class TestData
    {
        public static TokenResponseModel AccessToken =>
            new TokenResponseModel
            {
                ExpiryInMinutes = TestData.TokenExpiryInMinutes,
                AccessToken = TestData.Token,
                RefreshToken = TestData.RefreshToken
            };

        public static Configuration Configuration = new Configuration();
        public static Configuration NullConfiguration = null;

        public static TokenResponseModel NullAccessToken => null;

        public static String Token = "Token";

        public static String RefreshToken = "RefreshToken";
        public static Int32 TokenExpiryInMinutes = 5;
        public static Guid EstateId = Guid.Parse("21D339F4-C97F-4C30-A212-11CA01E2D508");
        public static Guid MerchantId = Guid.Parse("E8B4B839-434A-43A2-B373-D8813F63F615");

        public static String OperatorIdentifier1 = "Safaricom";
        public static String OperatorIdentifier2 = "Safaricom1";
        public static String OperatorIdentifier3 = "Voucher";

        public static String OperatorName1 = "Safaricom";
        public static String OperatorName2 = "Safaricom1";
        public static String OperatorName3 = "Voucher";

        public static Guid OperatorId1 = Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967");
        public static Guid OperatorId2 = Guid.Parse("1CA5F4C7-34EC-425B-BB53-7EEDF48D9968");
        public static Guid OperatorId3 = Guid.Parse("17FA0431-4DCB-43CF-97B7-2CEDC74E4796");

        public static Guid OperatorId1ContractId = Guid.Parse("D57DAC9B-4039-4120-B5A8-F7FDF1D3A3C2");
        public static Guid OperatorId2ContractId = Guid.Parse("2471F981-DDA8-4B87-91F4-DD5DCB984FC1");
        public static Guid OperatorId3ContractId = Guid.Parse("(0BCACE74-CBF1-4F2C-AC1C-733BF1F53133)");

        public static ProductDetails Operator1ProductDetails =>
            new ProductDetails() {
                                     ProductId = Operator1Product_100KES.ProductId,
                                     ContractId = Operator1Product_100KES.ContractId,
                                     OperatorId = Operator1Product_100KES.OperatorId,
                                 };

        public static ContractProductModel Operator1Product_BillPayment_PostPayment = new ContractProductModel
                                                                     {
                                                                         ContractId = OperatorId1ContractId,
                                                                         IsFixedValue = true,
                                                                         OperatorId = OperatorId1,
                                                                         OperatorIdentfier = OperatorIdentifier1,
                                                                         OperatorName = OperatorName1,
                                                                         ProductDisplayText = "Custom",
                                                                         ProductId = Guid.Parse("48D96AE5-A829-498B-8C0D-D7107610EBDC"),
                                                                         ProductType = ProductType.BillPayment,
                                                                         ProductSubType = ProductSubType.BillPaymentPostPay
                                                                     };

        public static ContractProductModel Operator1Product_BillPayment_PrePayment = new ContractProductModel
                                                                                      {
                                                                                          ContractId = OperatorId1ContractId,
                                                                                          IsFixedValue = true,
                                                                                          OperatorId = OperatorId1,
                                                                                          OperatorIdentfier = OperatorIdentifier1,
                                                                                          OperatorName = OperatorName1,
                                                                                          ProductDisplayText = "Custom",
                                                                                          ProductId = Guid.Parse("48D96AE5-A829-498B-8C0D-D7107610EBDC"),
                                                                                          ProductType = ProductType.BillPayment,
                                                                                          ProductSubType = ProductSubType.BillPaymentPrePay
                                                                                      };

        public static ContractProductModel Operator1Product_100KES = new ContractProductModel
                                                                     {
                                                                         ContractId = OperatorId1ContractId,
                                                                         IsFixedValue = true,
                                                                         OperatorId = OperatorId1,
                                                                         OperatorIdentfier = OperatorIdentifier1,
                                                                         OperatorName = OperatorName1,
                                                                         ProductDisplayText = "100 KES",
                                                                         ProductId = Guid.Parse("48D96AE5-A829-498B-8C0D-D7107610EBDC"),
                                                                         ProductType = ProductType.MobileTopup,
                                                                         Value = 100
                                                                     };

        public static ContractProductModel Operator1Product_200KES = new ContractProductModel
                                                                     {
                                                                         ContractId = OperatorId1ContractId,
                                                                         IsFixedValue = true,
                                                                         OperatorId = OperatorId1,
                                                                         OperatorIdentfier = OperatorIdentifier1,
                                                                         OperatorName = OperatorName1,
                                                                         ProductDisplayText = "200 KES",
                                                                         ProductId = Guid.Parse("63821A48-D35E-451D-9E88-C8DCB548E7ED"),
                                                                         ProductType = ProductType.MobileTopup,
                                                                         Value = 200
                                                                     };

        public static ContractProductModel Operator1Product_Custom = new ContractProductModel
                                                                     {
                                                                         ContractId = OperatorId1ContractId,
                                                                         IsFixedValue = false,
                                                                         OperatorId = OperatorId1,
                                                                         OperatorIdentfier = OperatorIdentifier1,
                                                                         OperatorName = OperatorName1,
                                                                         ProductDisplayText = "Custom",
                                                                         ProductId = Guid.Parse("63821A48-D35E-451D-9E88-C8DCB548E7ED"),
                                                                         ProductType = ProductType.MobileTopup
                                                                     };

        public static ContractProductModel Operator2Product_Custom = new ContractProductModel
                                                                     {
                                                                         ContractId = OperatorId2ContractId,
                                                                         IsFixedValue = false,
                                                                         OperatorId = TestData.OperatorId2,
                                                                         OperatorIdentfier = OperatorIdentifier2,
                                                                         OperatorName = OperatorName2,
                                                                         ProductDisplayText = "Custom",
                                                                         ProductId = Guid.Parse("63821A48-D35E-451D-9E88-C8DCB548E7ED"),
                                                                         ProductType = ProductType.MobileTopup
                                                                     };

        public static ContractProductModel Operator3Product_200KES = new ContractProductModel
                                                                     {
                                                                         ContractId = OperatorId3ContractId,
                                                                         IsFixedValue = false,
                                                                         OperatorId = TestData.OperatorId3,
                                                                         OperatorIdentfier = OperatorIdentifier3,
                                                                         OperatorName = OperatorName3,
                                                                         ProductDisplayText = "100 KES",
                                                                         ProductId = Guid.Parse("3270DC5F-1BB3-4ECF-BA06-AFB42B651483"),
                                                                         ProductType = ProductType.Voucher,
                                                                         Value = 200
                                                                     };

        public static List<ContractProductModel> ContractProductList = new List<ContractProductModel>
                                                                       {
                                                                           Operator1Product_100KES,
                                                                           Operator1Product_200KES,
                                                                           Operator1Product_Custom,
                                                                           Operator2Product_Custom,
                                                                           TestData.Operator3Product_200KES
                                                                       };

        public static List<ContractProductModel> ContractProductListEmpty = new List<ContractProductModel>();

        public static ContractOperatorModel ContractOperatorModel = new ContractOperatorModel
                                                                    {
                                                                        OperatorIdentfier = TestData.OperatorIdentifier1,
                                                                        OperatorId = TestData.OperatorId1,
                                                                        OperatorName = TestData.OperatorName1
                                                                    };

        public static String UserName = "testusername";
        public static String Password = "testpassword";

        public static DateTime TransactionDateTime = new DateTime(2022, 2, 14);

        public static String TransactionNumber = "1";

        public static String DeviceIdentifier = "1ABCD11";

        public static String ApplicationVersion = "1.0.0";

        public static String CustomerAccountNumber = "0123456";
        public static String CustomerEmailAddress = "1@2.com";

        public static String RecipientMobileNumber = "0123456";

        public static String RecipientEmailAddress = "1@2.com";

        public static Decimal MerchantBalance = 199.99m;

        public static List<TransactionRecord> StoredTransactions =>
            new List<TransactionRecord>{
                                           new TransactionRecord{
                                                                    Amount = TestData.Operator1Product_100KES.Value,
                                                                    ContractId = TestData.Operator1Product_100KES.ContractId,
                                                                    OperatorIdentifier = TestData.Operator1Product_100KES.OperatorIdentfier,
                                                                    IsSuccessful = true
                                                                },
                                           new TransactionRecord{
                                                                    Amount = TestData.Operator1Product_100KES.Value,
                                                                    ContractId = TestData.Operator1Product_100KES.ContractId,
                                                                    OperatorIdentifier = TestData.Operator1Product_100KES.OperatorIdentfier,
                                                                    IsSuccessful = true
                                                                },
                                           new TransactionRecord{
                                                                    Amount = TestData.Operator1Product_100KES.Value,
                                                                    ContractId = TestData.Operator1Product_100KES.ContractId,
                                                                    OperatorIdentifier = TestData.Operator1Product_100KES.OperatorIdentfier,
                                                                    IsSuccessful = true
                                                                },
                                           new TransactionRecord{
                                                                    Amount = TestData.Operator3Product_200KES.Value,
                                                                    ContractId = TestData.Operator3Product_200KES.ContractId,
                                                                    OperatorIdentifier = TestData.Operator3Product_200KES.OperatorIdentfier,
                                                                    IsSuccessful = true
                                                                }
                                       };

        public static String ResponseCode_Success = "0000";
        public static String ResponseMessage_Success = "SUCCESS";
        public static String ResponseCode_Failed = "1000";
        public static String ResponseMessage_Failed = "Failed";



        public static PerformLogonResponseModel PerformLogonResponseModel =>
            new PerformLogonResponseModel
            {
                EstateId = TestData.EstateId,
                MerchantId = TestData.MerchantId,
                ResponseCode = ResponseCode_Success,
                ResponseMessage = ResponseMessage_Success
            };

        public static PerformLogonResponseModel PerformLogonResponseFailedModel =>
            new PerformLogonResponseModel
            {
                EstateId = TestData.EstateId,
                MerchantId = TestData.MerchantId,
                ResponseCode = ResponseCode_Failed,
                ResponseMessage = ResponseMessage_Failed
            };


        public static PerformBillPaymentGetAccountResponseModel PerformBillPaymentGetAccountResponseModel =>
            new PerformBillPaymentGetAccountResponseModel {
                                                              BillDetails = TestData.BillDetails
                                                          };

        public static PerformBillPaymentGetMeterResponseModel PerformBillPaymentGetMeterResponseModel =>
            new PerformBillPaymentGetMeterResponseModel
            {
                MeterDetails = TestData.MeterDetails
            };

        public static PerformBillPaymentGetAccountResponseModel PerformBillPaymentGetAccountResponseModelFailed =>
            new PerformBillPaymentGetAccountResponseModel
            {
                BillDetails = null
            };

        public static PerformBillPaymentGetMeterResponseModel PerformBillPaymentGetMeterResponseModelFailed =>
            new PerformBillPaymentGetMeterResponseModel
            {
                MeterDetails = null
            };

        public static MerchantDetailsModel MerchantDetailsModel => new MerchantDetailsModel {
                                                                                                MerchantName = TestData.MerchantName,
                                                                                                Contact = new ContactModel {
                                                                                                              Name = TestData.ContactName,
                                                                                                              EmailAddress = TestData.ContactEmailAddress,
                                                                                                              MobileNumber = TestData.ContactMobileNumber
                                                                                                },
                                                                                                Address = new AddressModel {
                                                                                                              AddressLine2 = TestData.AddressLine2,
                                                                                                              Town = TestData.Town,
                                                                                                              AddressLine4 = TestData.AddressLine4,
                                                                                                              PostalCode = TestData.PostalCode,
                                                                                                              Region = TestData.Region,
                                                                                                              AddressLine3 = TestData.AddressLine3,
                                                                                                              AddressLine1 = TestData.AddressLine1
                                                                                                          },
                                                                                                AvailableBalance = TestData.AvailableBalance,
                                                                                                Balance = TestData.Balance,
                                                                                                LastStatementDate = TestData.LastStatementDate,
                                                                                                NextStatementDate = TestData.NextStatementDate,
                                                                                                SettlementSchedule = TestData.SettlementSchedule
                                                                                            };

        public static Decimal Balance = 100.00m;

        public static Decimal AvailableBalance = 99.00m;

        public static String MerchantName = "Test Merchant";

        public static DateTime NextStatementDate = new DateTime(2022, 09, 01);
        public static DateTime LastStatementDate = new DateTime(2022, 08, 01);

        public static String SettlementSchedule = "Monthly";

        // Address
        public static String AddressLine1 = "Address Line 1";
        public static String AddressLine2 = "Address Line 2";
        public static String AddressLine3 = "Address Line 3";
        public static String AddressLine4 = "Address Line 4";
        public static String PostalCode = "TE57 1NG";
        public static String Region = "Region";
        public static String Town = "Town";

        // Contact
        public static String ContactEmailAddress = "testcontact@myemail.com";

        public static String ContactName = "Mr Test Contact";

        public static String ContactMobileNumber = "077777777";

        public static String MeterNumber = "123456";

        public static String CustomerAccountName = "Mr Test Customer";

        public static String AccountBalance = "100 KES";

        public static String AccountDueDate = "20/09/2022";

        public static String CustomerMobileNumber = "077777777";

        public static Decimal PaymentAmount = 50.00m;

        public static Guid TransactionId = Guid.Parse("66F9743A-A682-4A73-9A80-441A3C9BCFDC");

        public static BillDetails BillDetails =>
            new BillDetails {
                                AccountNumber = TestData.CustomerAccountNumber,
                                AccountName = TestData.CustomerAccountName,
                                Balance = TestData.AccountBalance,
                                DueDate = TestData.AccountDueDate
                            };

        public static MeterDetails MeterDetails =>
            new MeterDetails
            {
                MeterNumber = TestData.MeterNumber,
                CustomerName = TestData.CustomerAccountName
            };

        public static Int32 TransactionCount = 1;

        public static Decimal TransactionValue = 100.00m;

    }
}
 