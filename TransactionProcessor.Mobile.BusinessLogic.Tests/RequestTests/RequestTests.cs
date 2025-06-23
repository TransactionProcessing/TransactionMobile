using Shouldly;
using TransactionProcessor.Mobile.BusinessLogic.Requests;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.RequestTests;

public class RequestTests
{
    [Fact]
    public void GetContractProductsRequest_Create_IsCreated()
    {
        GetContractProductsRequest request = GetContractProductsRequest.Create();

        request.ShouldNotBeNull();
        request.ProductType.ShouldBeNull();
    }

    [Theory]
    [InlineData(Models.ProductType.BillPayment)]
    [InlineData(Models.ProductType.MobileWallet)]
    [InlineData(Models.ProductType.MobileTopup)]
    [InlineData(Models.ProductType.Voucher)]
    public void GetContractProductsRequest_Create_WithProductType_IsCreated(Models.ProductType productType)
    {
        GetContractProductsRequest request = GetContractProductsRequest.Create(productType);

        request.ShouldNotBeNull();
        request.ProductType.ShouldBe(productType);
    }

    [Fact]
    public void GetMerchantBalanceRequest_Create_IsCreated()
    {
        GetMerchantBalanceRequest request = GetMerchantBalanceRequest.Create();

        request.ShouldNotBeNull();
    }

    [Fact]
    public void LoginRequest_Create_IsCreated()
    {
        LoginRequest request = LoginRequest.Create(TestData.UserName,
                                                   TestData.Password);

        request.ShouldNotBeNull();
        request.UserName.ShouldBe(TestData.UserName);
        request.Password.ShouldBe(TestData.Password);
    }

    [Fact]
    public void RefreshTokenRequest_Create_IsCreated()
    {
        RefreshTokenRequest request = RefreshTokenRequest.Create(TestData.RefreshToken);

        request.ShouldNotBeNull();
        request.RefreshToken.ShouldBe(TestData.RefreshToken);
    }

    [Fact]
    public void LogonTransactionRequest_Create_IsCreated()
    {
        LogonTransactionRequest request = LogonTransactionRequest.Create(TestData.TransactionDateTime);

        request.ShouldNotBeNull();
        request.TransactionDateTime.ShouldBe(TestData.TransactionDateTime);
    }

    [Fact]
    public void PerformMobileTopupRequest_Create_IsCreated()
    {
        PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(TestData.TransactionDateTime,
                                                                             TestData.OperatorId1ContractId,
                                                                             TestData.Operator1Product_100KES.ProductId,
                                                                             TestData.OperatorId1,
                                                                             TestData.CustomerAccountNumber,
                                                                             TestData.Operator1Product_100KES.Value,
                                                                             TestData.CustomerEmailAddress);

        request.ShouldNotBeNull();
        request.TransactionDateTime.ShouldBe(TestData.TransactionDateTime);
        request.ContractId.ShouldBe(TestData.OperatorId1ContractId);
        request.ProductId.ShouldBe(TestData.Operator1Product_100KES.ProductId);
        request.OperatorId.ShouldBe(TestData.OperatorId1);
        request.CustomerAccountNumber.ShouldBe(TestData.CustomerAccountNumber);
        request.TopupAmount.ShouldBe(TestData.Operator1Product_100KES.Value);
        request.CustomerEmailAddress.ShouldBe(TestData.CustomerEmailAddress);
    }

    [Fact]
    public void PerformVoucherIssueRequest_Create_IsCreated()
    {
        PerformVoucherIssueRequest request = PerformVoucherIssueRequest.Create(TestData.TransactionDateTime,
                                                                               TestData.OperatorId3ContractId,
                                                                               TestData.Operator3Product_200KES.ProductId,
                                                                               TestData.OperatorId3,
                                                                               TestData.RecipientMobileNumber,
                                                                               TestData.RecipientEmailAddress,
                                                                               TestData.Operator3Product_200KES.Value,
                                                                               TestData.CustomerEmailAddress);

        request.ShouldNotBeNull();
        request.TransactionDateTime.ShouldBe(TestData.TransactionDateTime);
        request.ContractId.ShouldBe(TestData.OperatorId3ContractId);
        request.ProductId.ShouldBe(TestData.Operator3Product_200KES.ProductId);
        request.OperatorId.ShouldBe(TestData.OperatorId3);
        request.RecipientMobileNumber.ShouldBe(TestData.RecipientMobileNumber);
        request.RecipientEmailAddress.ShouldBe(TestData.RecipientEmailAddress);
        request.VoucherAmount.ShouldBe(TestData.Operator3Product_200KES.Value);
        request.CustomerEmailAddress.ShouldBe(TestData.CustomerEmailAddress);
    }

    [Fact]
    public void PerformBillPaymentMakePostPaymentRequest_Create_IsCreated() {
        PerformBillPaymentMakePostPaymentRequest request = PerformBillPaymentMakePostPaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                           TestData.OperatorId3ContractId,
                                                                                                           TestData.Operator3Product_200KES.ProductId,
                                                                                                           TestData.OperatorId3,
                                                                                                           TestData.CustomerAccountNumber,
                                                                                                           TestData.CustomerAccountName,
                                                                                                           TestData.CustomerMobileNumber,
                                                                                                           TestData.PaymentAmount);

        request.ShouldNotBeNull();
        request.TransactionDateTime.ShouldBe(TestData.TransactionDateTime);
        request.ContractId.ShouldBe(TestData.OperatorId3ContractId);
        request.ProductId.ShouldBe(TestData.Operator3Product_200KES.ProductId);
        request.OperatorId.ShouldBe(TestData.OperatorId3);
        request.CustomerAccountNumber.ShouldBe(TestData.CustomerAccountNumber);
        request.CustomerAccountName.ShouldBe(TestData.CustomerAccountName);
        request.CustomerMobileNumber.ShouldBe(TestData.CustomerMobileNumber);
        request.PaymentAmount.ShouldBe(TestData.PaymentAmount);
    }

    [Fact]
    public void PerformBillPaymentMakePaymentRequest_Create_IsCreated()
    {
        PerformBillPaymentMakePrePaymentRequest request = PerformBillPaymentMakePrePaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                         TestData.OperatorId3ContractId,
                                                                                                         TestData.Operator3Product_200KES.ProductId,
                                                                                                         TestData.OperatorId3,
                                                                                                         TestData.MeterNumber,
                                                                                                         TestData.CustomerAccountName,
                                                                                                         TestData.PaymentAmount);

        request.ShouldNotBeNull();
        request.TransactionDateTime.ShouldBe(TestData.TransactionDateTime);
        request.ContractId.ShouldBe(TestData.OperatorId3ContractId);
        request.ProductId.ShouldBe(TestData.Operator3Product_200KES.ProductId);
        request.OperatorId.ShouldBe(TestData.OperatorId3);
        request.MeterNumber.ShouldBe(TestData.MeterNumber);
        request.CustomerAccountName.ShouldBe(TestData.CustomerAccountName);
        request.PaymentAmount.ShouldBe(TestData.PaymentAmount);
    }

    [Fact]
    public void PerformBillPaymentGetAccountRequest_Create_IsCreated() {
        PerformBillPaymentGetAccountRequest request = PerformBillPaymentGetAccountRequest.Create(TestData.TransactionDateTime,
                                                                                                 TestData.OperatorId3ContractId,
                                                                                                 TestData.Operator3Product_200KES.ProductId,
                                                                                                 TestData.OperatorId3,
                                                                                                 TestData.CustomerAccountNumber);

        request.ShouldNotBeNull();
        request.TransactionDateTime.ShouldBe(TestData.TransactionDateTime);
        request.ContractId.ShouldBe(TestData.OperatorId3ContractId);
        request.ProductId.ShouldBe(TestData.Operator3Product_200KES.ProductId);
        request.OperatorId.ShouldBe(TestData.OperatorId3);
        request.CustomerAccountNumber.ShouldBe(TestData.CustomerAccountNumber);
    }

    [Fact]
    public void PerformReconciliationRequest_Create_IsCreated()
    {
        PerformReconciliationRequest request = PerformReconciliationRequest.Create(TestData.TransactionDateTime,TestData.DeviceIdentifier,
                                                                                   TestData.ApplicationVersion);

        request.ShouldNotBeNull();
        request.TransactionDateTime.ShouldBe(TestData.TransactionDateTime);
        request.DeviceIdentifier.ShouldBe(TestData.DeviceIdentifier);
        request.ApplicationVersion.ShouldBe(TestData.ApplicationVersion);
    }

    [Fact]
    public void UploadLogsRequest_Create_IsCreated()
    {
        UploadLogsRequest request = UploadLogsRequest.Create(TestData.DeviceIdentifier);

        request.ShouldNotBeNull();
        request.DeviceIdentifier.ShouldBe(TestData.DeviceIdentifier);
    }
    
    [Fact]
    public void PerformBillPaymentGetMeterRequest_Create_IsCreated(){
        PerformBillPaymentGetMeterRequest request = PerformBillPaymentGetMeterRequest.Create(TestData.TransactionDateTime,
                                                                                             TestData.OperatorId3ContractId,
                                                                                             TestData.Operator3Product_200KES.ProductId,
                                                                                             TestData.OperatorId3,
                                                                                             TestData.CustomerAccountNumber);

        request.ShouldNotBeNull();
        request.TransactionDateTime.ShouldBe(TestData.TransactionDateTime);
        request.ContractId.ShouldBe(TestData.OperatorId3ContractId);
        request.ProductId.ShouldBe(TestData.Operator3Product_200KES.ProductId);
        request.OperatorId.ShouldBe(TestData.OperatorId3);
        request.MeterNumber.ShouldBe(TestData.CustomerAccountNumber);

    }
}