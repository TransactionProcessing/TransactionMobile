using System.Net;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessorACL.DataTransferObjects.Responses;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ServicesTests
{
    public class TransactionServiceTests{

        private MockHttpMessageHandler MockHttpMessageHandler;

        private Func<String, String> BaseAddressResolver;

        private ITransactionService TransactionService;

        private Mock<IApplicationCache> ApplicationCache;

        public TransactionServiceTests(){
            this.MockHttpMessageHandler = new MockHttpMessageHandler();
            this.BaseAddressResolver = (s) => $"http://localhost";
            this.ApplicationCache = new Mock<IApplicationCache>();
            this.TransactionService = new TransactionService(this.BaseAddressResolver, this.MockHttpMessageHandler.ToHttpClient(), this.ApplicationCache.Object);
            this.ApplicationCache.Setup(s => s.GetAccessToken()).Returns(new TokenResponseModel(){
                                                                                                     AccessToken = "token"
                                                                                                 });
            Logger.Initialise(new Logging.NullLogger());
        }

        [Fact]
        public async Task TransactionService_PerformLogon_LogonPerformed(){

            PerformLogonRequestModel requestModel = new PerformLogonRequestModel{
                                                                                    ApplicationVersion = TestData.ApplicationVersion,
                                                                                    DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                    TransactionDateTime = TestData.TransactionDateTime,
                                                                                    TransactionNumber = TestData.TransactionNumber
                                                                                };

            LogonTransactionResponseMessage expectedResponse = new LogonTransactionResponseMessage{
                                                                                                      ResponseMessage = TestData.ResponseMessage_Success,
                                                                                                      EstateId = TestData.EstateId,
                                                                                                      MerchantId = TestData.MerchantId,
                                                                                                      ResponseCode = TestData.ResponseCode_Success,
                                                                                                      TransactionId = TestData.TransactionId,
                                                                                                      AdditionalResponseMetaData = new Dictionary<String, String>()
                                                                                                  };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond("application/json", JsonConvert.SerializeObject(Result.Success(expectedResponse))); // Respond with JSON

            Result<PerformLogonResponseModel> performLogonResult = await this.TransactionService.PerformLogon(requestModel, CancellationToken.None);

            performLogonResult.IsSuccess.ShouldBeTrue();
            performLogonResult.Data.ShouldNotBeNull();
            performLogonResult.Data.EstateId.ShouldBe(expectedResponse.EstateId);
            performLogonResult.Data.MerchantId.ShouldBe(expectedResponse.MerchantId);
            performLogonResult.Data.ResponseCode.ShouldBe(expectedResponse.ResponseCode);
            performLogonResult.Data.ResponseMessage.ShouldBe(expectedResponse.ResponseMessage);
            performLogonResult.Data.IsSuccessful.ShouldBeTrue();
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.HttpVersionNotSupported)]
        public async Task TransactionService_PerformLogon_LogonFailed(HttpStatusCode statusCode){

            PerformLogonRequestModel requestModel = new PerformLogonRequestModel{
                                                                                    ApplicationVersion = TestData.ApplicationVersion,
                                                                                    DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                    TransactionDateTime = TestData.TransactionDateTime,
                                                                                    TransactionNumber = TestData.TransactionNumber
                                                                                };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond(req => new HttpResponseMessage(statusCode));

            Result<PerformLogonResponseModel> performLogonResult = await this.TransactionService.PerformLogon(requestModel, CancellationToken.None);

            performLogonResult.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task TransactionService_PerformMobileTopup_MobileTopupPerformed(){

            PerformMobileTopupRequestModel requestModel = new PerformMobileTopupRequestModel{
                                                                                                ApplicationVersion = TestData.ApplicationVersion,
                                                                                                DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                TransactionDateTime = TestData.TransactionDateTime,
                                                                                                TransactionNumber = TestData.TransactionNumber,
                                                                                                ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                CustomerAccountNumber = TestData.CustomerAccountNumber,
                                                                                                CustomerEmailAddress = TestData.CustomerEmailAddress,
                                                                                                OperatorId = TestData.OperatorId1,
                                                                                                ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                TopupAmount = TestData.PaymentAmount
                                                                                            };

            SaleTransactionResponseMessage expectedResponse = new SaleTransactionResponseMessage{
                                                                                                    ResponseMessage = TestData.ResponseMessage_Success,
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    MerchantId = TestData.MerchantId,
                                                                                                    ResponseCode = TestData.ResponseCode_Success,
                                                                                                    TransactionId = TestData.TransactionId,
                                                                                                    AdditionalResponseMetaData = new Dictionary<String, String>()
                                                                                                };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond("application/json", JsonConvert.SerializeObject(Result.Success(expectedResponse))); // Respond with JSON

            Result<PerformMobileTopupResponseModel> performMobileTopupResult = await this.TransactionService.PerformMobileTopup(requestModel, CancellationToken.None);

            performMobileTopupResult.IsSuccess.ShouldBeTrue();
            performMobileTopupResult.Data.ShouldNotBeNull();
            performMobileTopupResult.Data.EstateId.ShouldBe(expectedResponse.EstateId);
            performMobileTopupResult.Data.MerchantId.ShouldBe(expectedResponse.MerchantId);
            performMobileTopupResult.Data.ResponseCode.ShouldBe(expectedResponse.ResponseCode);
            performMobileTopupResult.Data.ResponseMessage.ShouldBe(expectedResponse.ResponseMessage);
            performMobileTopupResult.Data.IsSuccessful.ShouldBeTrue();
        }

        [Fact]
        public async Task TransactionService_PerformVoucherIssue_MobileTopupFailed(){

            PerformMobileTopupRequestModel requestModel = new PerformMobileTopupRequestModel{
                                                                                                ApplicationVersion = TestData.ApplicationVersion,
                                                                                                DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                TransactionDateTime = TestData.TransactionDateTime,
                                                                                                TransactionNumber = TestData.TransactionNumber,
                                                                                                ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                CustomerAccountNumber = TestData.CustomerAccountNumber,
                                                                                                CustomerEmailAddress = TestData.CustomerEmailAddress,
                                                                                                OperatorId = TestData.OperatorId1,
                                                                                                ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                TopupAmount = TestData.PaymentAmount
                                                                                            };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest));

            Result<PerformMobileTopupResponseModel> performMobileTopupResult = await this.TransactionService.PerformMobileTopup(requestModel, CancellationToken.None);

            performMobileTopupResult.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task TransactionService_PerformVoucherIssue_VoucherIssuePerformed(){
            PerformVoucherIssueRequestModel requestModel = new PerformVoucherIssueRequestModel{
                                                                                                  ApplicationVersion = TestData.ApplicationVersion,
                                                                                                  DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                  TransactionDateTime = TestData.TransactionDateTime,
                                                                                                  TransactionNumber = TestData.TransactionNumber,
                                                                                                  ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                  CustomerEmailAddress = TestData.CustomerEmailAddress,
                                                                                                  OperatorId = TestData.OperatorId1,
                                                                                                  ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                  RecipientEmailAddress = TestData.RecipientEmailAddress,
                                                                                                  RecipientMobileNumber = TestData.RecipientMobileNumber,
                                                                                                  VoucherAmount = TestData.PaymentAmount
                                                                                              };

            SaleTransactionResponseMessage expectedResponse = new SaleTransactionResponseMessage{
                                                                                                    ResponseMessage = TestData.ResponseMessage_Success,
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    MerchantId = TestData.MerchantId,
                                                                                                    ResponseCode = TestData.ResponseCode_Success,
                                                                                                    TransactionId = TestData.TransactionId,
                                                                                                    AdditionalResponseMetaData = new Dictionary<String, String>()
                                                                                                };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond("application/json", JsonConvert.SerializeObject(Result.Success(expectedResponse))); // Respond with JSON

            Result<PerformVoucherIssueResponseModel> performVoucherIssueResult = await this.TransactionService.PerformVoucherIssue(requestModel, CancellationToken.None);

            performVoucherIssueResult.IsSuccess.ShouldBeTrue();
            performVoucherIssueResult.Data.ShouldNotBeNull();
            performVoucherIssueResult.Data.EstateId.ShouldBe(expectedResponse.EstateId);
            performVoucherIssueResult.Data.MerchantId.ShouldBe(expectedResponse.MerchantId);
            performVoucherIssueResult.Data.ResponseCode.ShouldBe(expectedResponse.ResponseCode);
            performVoucherIssueResult.Data.ResponseMessage.ShouldBe(expectedResponse.ResponseMessage);
            performVoucherIssueResult.Data.IsSuccessful.ShouldBeTrue();
        }

        [Fact]
        public async Task TransactionService_PerformVoucherIssue_VoucherIssueFailed(){

            PerformVoucherIssueRequestModel requestModel = new PerformVoucherIssueRequestModel{
                                                                                                  ApplicationVersion = TestData.ApplicationVersion,
                                                                                                  DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                  TransactionDateTime = TestData.TransactionDateTime,
                                                                                                  TransactionNumber = TestData.TransactionNumber,
                                                                                                  ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                  CustomerEmailAddress = TestData.CustomerEmailAddress,
                                                                                                  OperatorId = TestData.OperatorId1,
                                                                                                  ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                  RecipientEmailAddress = TestData.RecipientEmailAddress,
                                                                                                  RecipientMobileNumber = TestData.RecipientMobileNumber,
                                                                                                  VoucherAmount = TestData.PaymentAmount
                                                                                              };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest));

            Result<PerformVoucherIssueResponseModel> performVoucherIssueResult = await this.TransactionService.PerformVoucherIssue(requestModel, CancellationToken.None);

            performVoucherIssueResult.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task TransactionService_PerformBillPaymentGetAccount_GetAccountPerformed() {
            PerformBillPaymentGetAccountModel requestModel = new PerformBillPaymentGetAccountModel{
                                                                                                      ApplicationVersion = TestData.ApplicationVersion,
                                                                                                      DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                      TransactionDateTime = TestData.TransactionDateTime,
                                                                                                      TransactionNumber = TestData.TransactionNumber,
                                                                                                      ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                      OperatorId = TestData.OperatorId1,
                                                                                                      ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                      CustomerAccountNumber = TestData.CustomerAccountNumber,
                                                                                                  };

            SaleTransactionResponseMessage expectedResponse = new SaleTransactionResponseMessage{
                                                                                                    ResponseMessage = TestData.ResponseMessage_Success,
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    MerchantId = TestData.MerchantId,
                                                                                                    ResponseCode = TestData.ResponseCode_Success,
                                                                                                    TransactionId = TestData.TransactionId,
                                                                                                    AdditionalResponseMetaData = new Dictionary<String, String>(){
                                                                                                                                                                     { "customerAccountName", TestData.BillDetails.AccountName },
                                                                                                                                                                     { "customerAccountNumber", TestData.BillDetails.AccountNumber },
                                                                                                                                                                     { "customerBillBalance", TestData.BillDetails.Balance },
                                                                                                                                                                     { "customerBillDueDate", TestData.BillDetails.DueDate }
                                                                                                                                                                 }
                                                                                                };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond("application/json", JsonConvert.SerializeObject(Result.Success(expectedResponse))); // Respond with JSON

            Result<PerformBillPaymentGetAccountResponseModel> performBillPaymentGetAccountResult = await this.TransactionService.PerformBillPaymentGetAccount(requestModel, CancellationToken.None);
            performBillPaymentGetAccountResult.ShouldNotBeNull();
            performBillPaymentGetAccountResult.IsSuccess.ShouldBeTrue();
            performBillPaymentGetAccountResult.Data.ShouldNotBeNull();
            performBillPaymentGetAccountResult.Data.IsSuccessful.ShouldBeTrue();
            performBillPaymentGetAccountResult.Data.BillDetails.ShouldNotBeNull();
            performBillPaymentGetAccountResult.Data.BillDetails.AccountName.ShouldBe(TestData.BillDetails.AccountName);
            performBillPaymentGetAccountResult.Data.BillDetails.AccountNumber.ShouldBe(TestData.BillDetails.AccountNumber);
            performBillPaymentGetAccountResult.Data.BillDetails.Balance.ShouldBe(TestData.BillDetails.Balance);
            performBillPaymentGetAccountResult.Data.BillDetails.DueDate.ShouldBe(TestData.BillDetails.DueDate);
        }

        [Fact]
        public async Task TransactionService_PerformBillPaymentGetAccount_GetAccountFailed(){

            PerformBillPaymentGetAccountModel requestModel = new PerformBillPaymentGetAccountModel{
                                                                                                      ApplicationVersion = TestData.ApplicationVersion,
                                                                                                      DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                      TransactionDateTime = TestData.TransactionDateTime,
                                                                                                      TransactionNumber = TestData.TransactionNumber,
                                                                                                      ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                      OperatorId = TestData.OperatorId1,
                                                                                                      ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                      CustomerAccountNumber = TestData.CustomerAccountNumber,
                                                                                                  };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest));

            Result<PerformBillPaymentGetAccountResponseModel> performBillPaymentGetAccountResult = await this.TransactionService.PerformBillPaymentGetAccount(requestModel, CancellationToken.None);

            performBillPaymentGetAccountResult.IsFailed.ShouldBeTrue();
        }


        [Fact]
        public async Task TransactionService_PerformBillPaymentGetMeter_GetMeterPerformed(){
            PerformBillPaymentGetMeterModel requestModel = new PerformBillPaymentGetMeterModel{
                                                                                                  ApplicationVersion = TestData.ApplicationVersion,
                                                                                                  DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                  TransactionDateTime = TestData.TransactionDateTime,
                                                                                                  TransactionNumber = TestData.TransactionNumber,
                                                                                                  ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                  OperatorId = TestData.OperatorId1,
                                                                                                  ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                  MeterNumber = TestData.MeterNumber
                                                                                              };

            SaleTransactionResponseMessage expectedResponse = new SaleTransactionResponseMessage{
                                                                                                    ResponseMessage = TestData.ResponseMessage_Success,
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    MerchantId = TestData.MerchantId,
                                                                                                    ResponseCode = TestData.ResponseCode_Success,
                                                                                                    TransactionId = TestData.TransactionId,
                                                                                                    AdditionalResponseMetaData = new Dictionary<String, String>(){
                                                                                                                                                                     { "pataPawaPrePaidCustomerName", TestData.BillDetails.AccountName }
                                                                                                                                                                 }
                                                                                                };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond("application/json", JsonConvert.SerializeObject(Result.Success(expectedResponse))); // Respond with JSON

            Result<PerformBillPaymentGetMeterResponseModel> performBillPaymentGetMeterResult = await this.TransactionService.PerformBillPaymentGetMeter(requestModel, CancellationToken.None);

            performBillPaymentGetMeterResult.IsSuccess.ShouldBeTrue();
            performBillPaymentGetMeterResult.Data.ShouldNotBeNull();
            performBillPaymentGetMeterResult.Data.IsSuccessful.ShouldBeTrue();
            performBillPaymentGetMeterResult.Data.MeterDetails.ShouldNotBeNull();
            performBillPaymentGetMeterResult.Data.MeterDetails.CustomerName.ShouldBe(TestData.MeterDetails.CustomerName);
            performBillPaymentGetMeterResult.Data.MeterDetails.MeterNumber.ShouldBe(TestData.MeterDetails.MeterNumber);
        }

        [Fact]
        public async Task TransactionService_PerformBillPaymentGetMeter_NoAdditionalResponseMetaData_GetMeterPerformed(){
            PerformBillPaymentGetMeterModel requestModel = new PerformBillPaymentGetMeterModel{
                                                                                                  ApplicationVersion = TestData.ApplicationVersion,
                                                                                                  DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                  TransactionDateTime = TestData.TransactionDateTime,
                                                                                                  TransactionNumber = TestData.TransactionNumber,
                                                                                                  ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                  OperatorId = TestData.OperatorId1,
                                                                                                  ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                  MeterNumber = TestData.MeterNumber
                                                                                              };

            SaleTransactionResponseMessage expectedResponse = new SaleTransactionResponseMessage{
                                                                                                    ResponseMessage = TestData.ResponseMessage_Success,
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    MerchantId = TestData.MerchantId,
                                                                                                    ResponseCode = TestData.ResponseCode_Success,
                                                                                                    TransactionId = TestData.TransactionId,
                                                                                                    AdditionalResponseMetaData = null
                                                                                                };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond("application/json", JsonConvert.SerializeObject(Result.Success(expectedResponse))); // Respond with JSON

            Result<PerformBillPaymentGetMeterResponseModel> performBillPaymentGetMeterResult = await this.TransactionService.PerformBillPaymentGetMeter(requestModel, CancellationToken.None);

            performBillPaymentGetMeterResult.IsSuccess.ShouldBeTrue();
            performBillPaymentGetMeterResult.Data.ShouldNotBeNull();
            performBillPaymentGetMeterResult.Data.IsSuccessful.ShouldBeFalse();
            performBillPaymentGetMeterResult.Data.MeterDetails.ShouldBeNull();
        }

        [Fact]
        public async Task TransactionService_PerformBillPaymentGetMeter_GetMeterFailed(){
            PerformBillPaymentGetMeterModel requestModel = new PerformBillPaymentGetMeterModel{
                                                                                                  ApplicationVersion = TestData.ApplicationVersion,
                                                                                                  DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                  TransactionDateTime = TestData.TransactionDateTime,
                                                                                                  TransactionNumber = TestData.TransactionNumber,
                                                                                                  ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                  OperatorId = TestData.OperatorId1,
                                                                                                  ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                  MeterNumber = TestData.MeterNumber
                                                                                              };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest));

            Result<PerformBillPaymentGetMeterResponseModel> performBillPaymentGetMeterResult = await this.TransactionService.PerformBillPaymentGetMeter(requestModel, CancellationToken.None);

            performBillPaymentGetMeterResult.IsFailed.ShouldBeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task TransactionService_PerformBillPaymentMakePayment_MakePaymentPerformed(Boolean isPostPayment){
            PerformBillPaymentMakePaymentModel requestModel = new PerformBillPaymentMakePaymentModel{
                                                                                                        ApplicationVersion = TestData.ApplicationVersion,
                                                                                                        DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                        TransactionDateTime = TestData.TransactionDateTime,
                                                                                                        TransactionNumber = TestData.TransactionNumber,
                                                                                                        ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                        OperatorId = TestData.OperatorId1,
                                                                                                        ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                        CustomerAccountName = TestData.CustomerAccountName,
                                                                                                        PaymentAmount = TestData.PaymentAmount,
                                                                                                        CustomerAccountNumber = TestData.CustomerAccountNumber,
                                                                                                        CustomerMobileNumber = TestData.CustomerMobileNumber,
                                                                                                        PostPayment = isPostPayment
                                                                                                    };

            SaleTransactionResponseMessage expectedResponse = new SaleTransactionResponseMessage{
                                                                                                    ResponseMessage = TestData.ResponseMessage_Success,
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    MerchantId = TestData.MerchantId,
                                                                                                    ResponseCode = TestData.ResponseCode_Success,
                                                                                                    TransactionId = TestData.TransactionId
                                                                                                };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond("application/json", JsonConvert.SerializeObject(Result.Success(expectedResponse))); // Respond with JSON

            Result<PerformBillPaymentMakePaymentResponseModel> performBillPaymentGetMeterResult = await this.TransactionService.PerformBillPaymentMakePayment(requestModel, CancellationToken.None);

            performBillPaymentGetMeterResult.IsSuccess.ShouldBeTrue();
            performBillPaymentGetMeterResult.Data.ShouldNotBeNull();
            performBillPaymentGetMeterResult.Data.IsSuccessful.ShouldBeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task TransactionService_PerformBillPaymentMakePayment_MakePaymentFailed(Boolean isPostPayment){
            PerformBillPaymentMakePaymentModel requestModel = new PerformBillPaymentMakePaymentModel{
                                                                                                        ApplicationVersion = TestData.ApplicationVersion,
                                                                                                        DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                        TransactionDateTime = TestData.TransactionDateTime,
                                                                                                        TransactionNumber = TestData.TransactionNumber,
                                                                                                        ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                        OperatorId = TestData.OperatorId1,
                                                                                                        ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                        CustomerAccountName = TestData.CustomerAccountName,
                                                                                                        PaymentAmount = TestData.PaymentAmount,
                                                                                                        CustomerAccountNumber = TestData.CustomerAccountNumber,
                                                                                                        CustomerMobileNumber = TestData.CustomerMobileNumber,
                                                                                                        PostPayment = isPostPayment
                                                                                                    };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest));

            Result<PerformBillPaymentMakePaymentResponseModel> performBillPaymentGetMeterResult = await this.TransactionService.PerformBillPaymentMakePayment(requestModel, CancellationToken.None);

            performBillPaymentGetMeterResult.IsSuccess.ShouldBeFalse();
        }

        [Fact]
        public async Task TransactionService_PerformReconciliation_ReconciliationPerformed()
        {
            PerformReconciliationRequestModel requestModel = new PerformReconciliationRequestModel
            {
                ApplicationVersion = TestData.ApplicationVersion,
                DeviceIdentifier = TestData.DeviceIdentifier,
                TransactionDateTime = TestData.TransactionDateTime,
                OperatorTotals = new List<OperatorTotalModel>(){
                                                                   new OperatorTotalModel{
                                                                                             TransactionCount = TestData.TransactionCount,
                                                                                             TransactionValue = TestData.TransactionValue,
                                                                                             ContractId = TestData.OperatorId1ContractId,
                                                                                             OperatorId = TestData.OperatorId1
                                                                                         }
                                                               },
                TransactionCount = TestData.TransactionCount,
                TransactionValue = TestData.TransactionValue
            };

            SaleTransactionResponseMessage expectedResponse = new SaleTransactionResponseMessage
            {
                ResponseMessage = TestData.ResponseMessage_Success,
                EstateId = TestData.EstateId,
                MerchantId = TestData.MerchantId,
                ResponseCode = TestData.ResponseCode_Success,
                TransactionId = TestData.TransactionId,
                AdditionalResponseMetaData = null
            };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions")
                .Respond("application/json", JsonConvert.SerializeObject(Result.Success(expectedResponse))); // Respond with JSON

            Result<PerformReconciliationResponseModel> performReconciliationResult = await this.TransactionService.PerformReconciliation(requestModel, CancellationToken.None);

            performReconciliationResult.IsSuccess.ShouldBeTrue();
            performReconciliationResult.Data.ShouldNotBeNull();
            performReconciliationResult.Data.EstateId.ShouldBe(expectedResponse.EstateId);
            performReconciliationResult.Data.MerchantId.ShouldBe(expectedResponse.MerchantId);
            performReconciliationResult.Data.ResponseCode.ShouldBe(expectedResponse.ResponseCode);
            performReconciliationResult.Data.ResponseMessage.ShouldBe(expectedResponse.ResponseMessage);
            performReconciliationResult.Data.IsSuccessful.ShouldBeTrue();
        }

        [Fact]
        public async Task TransactionService_PerformReconciliation_ReconciliationFailed()
        {
            PerformReconciliationRequestModel requestModel = new PerformReconciliationRequestModel
                                                             {
                                                                 ApplicationVersion = TestData.ApplicationVersion,
                                                                 DeviceIdentifier = TestData.DeviceIdentifier,
                                                                 TransactionDateTime = TestData.TransactionDateTime,
                                                                 OperatorTotals = new List<OperatorTotalModel>(){
                                                                                                                    new OperatorTotalModel{
                                                                                                                                              TransactionCount = TestData.TransactionCount,
                                                                                                                                              TransactionValue = TestData.TransactionValue,
                                                                                                                                              ContractId = TestData.OperatorId1ContractId,
                                                                                                                                              OperatorId = TestData.OperatorId1
                                                                                                                                          }
                                                                                                                },
                                                                 TransactionCount = TestData.TransactionCount,
                                                                 TransactionValue = TestData.TransactionValue
                                                             };

            this.MockHttpMessageHandler.When($"http://localhost/api/transactions").Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest));

            Result<PerformReconciliationResponseModel> performReconciliationResult = await this.TransactionService.PerformReconciliation(requestModel, CancellationToken.None);

            performReconciliationResult.IsFailed.ShouldBeTrue();
        }
    }
}
