using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Tests
{
    using Models;

    public static class TestData
    {
        public static String AccessToken = "Token";
        public static Guid EstateId = Guid.Parse("21D339F4-C97F-4C30-A212-11CA01E2D508");
        public static Guid MerchantId = Guid.Parse("E8B4B839-434A-43A2-B373-D8813F63F615");

        public static String OperatorIdentifier1 = "Safaricom";
        public static String OperatorIdentifier2 = "Safaricom1";
        public static String OperatorName1 = "Safaricom";
        public static String OperatorName2 = "Safaricom1";

        public static Guid OperatorId1 = Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967");
        public static Guid OperatorId2 = Guid.Parse("1CA5F4C7-34EC-425B-BB53-7EEDF48D9968");

        public static Guid OperatorId1ContractId = Guid.Parse("D57DAC9B-4039-4120-B5A8-F7FDF1D3A3C2");
        public static Guid OperatorId2ContractId = Guid.Parse("2471F981-DDA8-4B87-91F4-DD5DCB984FC1");

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

        public static List<ContractProductModel> ContractProductList = new List<ContractProductModel>
                                                                       {
                                                                           Operator1Product_100KES,
                                                                           Operator1Product_200KES,
                                                                           Operator1Product_Custom,
                                                                           Operator2Product_Custom
                                                                       };

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

        public static Decimal MerchantBalance = 199.99m;
    }
}
 