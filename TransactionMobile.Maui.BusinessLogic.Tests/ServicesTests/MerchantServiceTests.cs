namespace TransactionMobile.Maui.BusinessLogic.Tests.ServicesTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EstateManagement.Client;
using EstateManagement.DataTransferObjects;
using EstateManagement.DataTransferObjects.Responses;
using Logging;
using Models;
using Moq;
using Services;
using Shouldly;
using SimpleResults;
using Xunit;
using ProductType = EstateManagement.DataTransferObjects.ProductType;

public class MerchantServiceTests{

    private readonly Mock<IEstateClient> EstateClient;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly IMerchantService MerchantService;

    public MerchantServiceTests(){
        Logger.Initialise(new NullLogger());
        this.EstateClient = new Mock<IEstateClient>();
        this.ApplicationCache = new Mock<IApplicationCache>();

        this.MerchantService = new MerchantService(this.EstateClient.Object, this.ApplicationCache.Object);

        // Standard cache mocking here
        this.ApplicationCache.Setup(a => a.GetAccessToken()).Returns(TestData.AccessToken);
    }

    [Fact]
    public async Task MerchantService_GetContractProducts_ContractProductsAreReturned(){

        List<ContractResponse> contracts = new List<ContractResponse>();
        contracts.Add(new ContractResponse{
                                              ContractId = TestData.OperatorId1ContractId,
                                              EstateId = TestData.EstateId,
                                              Products = new List<ContractProduct>{
                                                                                      new ContractProduct{
                                                                                                             ProductId = TestData.Operator1Product_100KES.ProductId,
                                                                                                             DisplayText = TestData.Operator1Product_100KES.ProductDisplayText,
                                                                                                             Name = TestData.Operator1Product_100KES.ProductDisplayText,
                                                                                                             ProductType = ProductType.MobileTopup,
                                                                                                             Value = TestData.Operator1Product_100KES.Value,
                                                                                                         },
                                                                                      new ContractProduct{
                                                                                                             ProductId = TestData.Operator1Product_200KES.ProductId,
                                                                                                             DisplayText = TestData.Operator1Product_200KES.ProductDisplayText,
                                                                                                             Name = TestData.Operator1Product_200KES.ProductDisplayText,
                                                                                                             ProductType = ProductType.MobileTopup,
                                                                                                             Value = TestData.Operator1Product_200KES.Value,
                                                                                                         }

                                                                                  }
                                          });


        this.EstateClient.Setup(e => e.GetMerchantContracts(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(contracts);

        var result = await this.MerchantService.GetContractProducts(CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task MerchantService_GetContractProducts_EstateClientThrowsException_FailureReturned(){
        this.EstateClient.Setup(e => e.GetMerchantContracts(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        var result = await this.MerchantService.GetContractProducts(CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task MerchantService_GetMerchantDetails_MerchantDetailsReturned(){
        MerchantResponse merchantResponse = new MerchantResponse{
                                                                    MerchantName = TestData.MerchantName,
                                                                    NextStatementDate = TestData.NextStatementDate,
                                                                    SettlementSchedule = SettlementSchedule.Immediate,
                                                                    Contacts = new List<ContactResponse>{
                                                                                                            new ContactResponse{
                                                                                                                                   ContactEmailAddress = TestData.ContactEmailAddress,
                                                                                                                                   ContactName = TestData.ContactName,
                                                                                                                                   ContactPhoneNumber = TestData.ContactMobileNumber
                                                                                                                               }
                                                                                                        },
                                                                    Addresses = new List<AddressResponse>{
                                                                                                             new AddressResponse{
                                                                                                                                    AddressLine1 = TestData.AddressLine1,
                                                                                                                                    AddressLine2 = TestData.AddressLine2,
                                                                                                                                    AddressLine3 = TestData.AddressLine3,
                                                                                                                                    AddressLine4 = TestData.AddressLine4,
                                                                                                                                    PostalCode = TestData.PostalCode,
                                                                                                                                    Region = TestData.Region,
                                                                                                                                    Town = TestData.Town,

                                                                                                                                }
                                                                                                         }
                                                                };

        this.EstateClient.Setup(e => e.GetMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchantResponse);

        Result<MerchantDetailsModel> merchantDetails = await this.MerchantService.GetMerchantDetails(CancellationToken.None);
        merchantDetails.IsSuccess.ShouldBeTrue();
        merchantDetails.Data.Address.AddressLine1.ShouldBe(merchantResponse.Addresses.First().AddressLine1);
        merchantDetails.Data.Address.AddressLine2.ShouldBe(merchantResponse.Addresses.First().AddressLine2);
        merchantDetails.Data.Address.AddressLine3.ShouldBe(merchantResponse.Addresses.First().AddressLine3);
        merchantDetails.Data.Address.AddressLine4.ShouldBe(merchantResponse.Addresses.First().AddressLine4);
        merchantDetails.Data.Address.PostalCode.ShouldBe(merchantResponse.Addresses.First().PostalCode);
        merchantDetails.Data.Address.Region.ShouldBe(merchantResponse.Addresses.First().Region);
        merchantDetails.Data.Address.Town.ShouldBe(merchantResponse.Addresses.First().Town);
        merchantDetails.Data.Contact.Name.ShouldBe(merchantResponse.Contacts.First().ContactName);
        merchantDetails.Data.Contact.EmailAddress.ShouldBe(merchantResponse.Contacts.First().ContactEmailAddress);
        merchantDetails.Data.Contact.MobileNumber.ShouldBe(merchantResponse.Contacts.First().ContactPhoneNumber);
        merchantDetails.Data.MerchantName.ShouldBe(merchantResponse.MerchantName);
        merchantDetails.Data.NextStatementDate.ShouldBe(merchantResponse.NextStatementDate);
        merchantDetails.Data.SettlementSchedule.ShouldBe(merchantResponse.SettlementSchedule.ToString());
    }

    [Fact]
    public async Task MerchantService_GetMerchantDetails_ExceptionThrown_FailedResultReturned(){
        this.EstateClient.Setup(e => e.GetMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        Result<MerchantDetailsModel> merchantDetails = await this.MerchantService.GetMerchantDetails(CancellationToken.None);
        merchantDetails.IsSuccess.ShouldBeFalse();
    }

    [Theory]
    [InlineData("Description", "OperatorName", Models.ProductType.BillPayment, "OperatorName")]
    [InlineData("Description", "OperatorName", Models.ProductType.MobileTopup, "OperatorName")]
    [InlineData("Description", "OperatorName", Models.ProductType.Voucher, "Description")]
    public void GetOperatorName_CorrectNameReturned(String contractDescription, String operatorName, Models.ProductType productType, String expectedName){
        ContractResponse contractResponse = new ContractResponse{
                                                                    Description = contractDescription,
                                                                    OperatorName = operatorName
                                                                };
        String result = Services.MerchantService.GetOperatorName(contractResponse, productType);
        result.ShouldBe(expectedName);
    }

    [Theory]
    [InlineData("Safaricom", Models.ProductType.MobileTopup)]
    [InlineData("Voucher", Models.ProductType.Voucher)]
    [InlineData("PataPawa PostPay", Models.ProductType.BillPayment)]
    [InlineData("PataPawa PrePay", Models.ProductType.BillPayment)]
    public void GetProductType_CorrectProductTypeReturned(String operatorName, Models.ProductType expectedProductType){
        Models.ProductType result = Services.MerchantService.GetProductType(operatorName);
        result.ShouldBe(expectedProductType);
    }

    [Theory]
    [InlineData("Safaricom", Models.ProductSubType.MobileTopup)]
    [InlineData("Voucher", Models.ProductSubType.Voucher)]
    [InlineData("PataPawa PostPay", Models.ProductSubType.BillPaymentPostPay)]
    [InlineData("PataPawa PrePay", Models.ProductSubType.BillPaymentPrePay)]
    public void GetProductSubType_CorrectProductSubTypeReturned(String operatorName, Models.ProductSubType expectedProductSubType)
    {
        Models.ProductSubType result = Services.MerchantService.GetProductSubType(operatorName);
        result.ShouldBe(expectedProductSubType);
    }
}