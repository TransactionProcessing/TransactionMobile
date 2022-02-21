using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Tests.RequestTests;

using Requests;
using Shouldly;
using Xunit;

public class RequestTests
{
    [Fact]
    public void GetContractProductsRequest_Create_IsCreated()
    {
        GetContractProductsRequest request = GetContractProductsRequest.Create(TestData.AccessToken,
                                                                               TestData.EstateId,
                                                                               TestData.MerchantId,
                                                                               null);

        request.ShouldNotBeNull();
        request.AccessToken.ShouldBe(TestData.AccessToken);
        request.EstateId.ShouldBe(TestData.EstateId);
        request.MerchantId.ShouldBe(TestData.MerchantId);
    }

    [Fact]
    public void GetMerchantBalanceRequest_Create_IsCreated()
    {
        GetMerchantBalanceRequest request = GetMerchantBalanceRequest.Create(TestData.AccessToken,
                                                                             TestData.EstateId,
                                                                             TestData.MerchantId);

        request.ShouldNotBeNull();
        request.AccessToken.ShouldBe(TestData.AccessToken);
        request.EstateId.ShouldBe(TestData.EstateId);
        request.MerchantId.ShouldBe(TestData.MerchantId);
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
    public void LogonTransactionRequest_Create_IsCreated()
    {
        LogonTransactionRequest request = LogonTransactionRequest.Create(TestData.TransactionDateTime,
                                                                         TestData.TransactionNumber,
                                                                         TestData.DeviceIdentifier,
                                                                         TestData.ApplicationVersion);

        request.ShouldNotBeNull();
        request.TransactionDateTime.ShouldBe(TestData.TransactionDateTime);
        request.TransactionNumber.ShouldBe(TestData.TransactionNumber);
        request.DeviceIdentifier.ShouldBe(TestData.DeviceIdentifier);
        request.ApplicationVersion.ShouldBe(TestData.ApplicationVersion);
    }

    [Fact]
    public void PerformMobileTopupRequest_Create_IsCreated()
    {
        PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(TestData.TransactionDateTime,
                                                                             TestData.TransactionNumber,
                                                                             TestData.DeviceIdentifier,
                                                                             TestData.ApplicationVersion,
                                                                             TestData.OperatorId1ContractId,
                                                                             TestData.Operator1Product_100KES.ProductId,
                                                                             TestData.OperatorIdentifier1,
                                                                             TestData.CustomerAccountNumber,
                                                                             TestData.Operator1Product_100KES.Value,
                                                                             TestData.CustomerEmailAddress);

        request.ShouldNotBeNull();
        request.TransactionDateTime.ShouldBe(TestData.TransactionDateTime);
        request.TransactionNumber.ShouldBe(TestData.TransactionNumber);
        request.DeviceIdentifier.ShouldBe(TestData.DeviceIdentifier);
        request.ApplicationVersion.ShouldBe(TestData.ApplicationVersion);
        request.ContractId.ShouldBe(TestData.OperatorId1ContractId);
        request.ProductId.ShouldBe(TestData.Operator1Product_100KES.ProductId);
        request.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier1);
        request.CustomerAccountNumber.ShouldBe(TestData.CustomerAccountNumber);
        request.TopupAmount.ShouldBe(TestData.Operator1Product_100KES.Value);
        request.CustomerEmailAddress.ShouldBe(TestData.CustomerEmailAddress);
    }

    [Fact]
    public void PerformVoucherIssueRequest_Create_IsCreated()
    {
        PerformVoucherIssueRequest request = PerformVoucherIssueRequest.Create(TestData.TransactionDateTime,
                                                                               TestData.TransactionNumber,
                                                                               TestData.DeviceIdentifier,
                                                                               TestData.ApplicationVersion,
                                                                               TestData.OperatorId3ContractId,
                                                                               TestData.Operator3Product_200KES.ProductId,
                                                                               TestData.OperatorIdentifier3,
                                                                               TestData.RecipientMobileNumber,
                                                                               TestData.RecipientEmailAddress,
                                                                               TestData.Operator3Product_200KES.Value,
                                                                               TestData.CustomerEmailAddress);

        request.ShouldNotBeNull();
        request.TransactionDateTime.ShouldBe(TestData.TransactionDateTime);
        request.TransactionNumber.ShouldBe(TestData.TransactionNumber);
        request.DeviceIdentifier.ShouldBe(TestData.DeviceIdentifier);
        request.ApplicationVersion.ShouldBe(TestData.ApplicationVersion);
        request.ContractId.ShouldBe(TestData.OperatorId3ContractId);
        request.ProductId.ShouldBe(TestData.Operator3Product_200KES.ProductId);
        request.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier3);
        request.RecipientMobileNumber.ShouldBe(TestData.RecipientMobileNumber);
        request.RecipientEmailAddress.ShouldBe(TestData.RecipientEmailAddress);
        request.VoucherAmount.ShouldBe(TestData.Operator3Product_200KES.Value);
        request.CustomerEmailAddress.ShouldBe(TestData.CustomerEmailAddress);
    }
}