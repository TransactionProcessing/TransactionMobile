using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Tests.Common
{
    using BusinessLogic.Common;
    using Database;
    using Models;
    using Requests;
    using Shouldly;
    using SimpleResults;
    using TransactionProcessorACL.DataTransferObjects;
    using TransactionProcessorACL.DataTransferObjects.Responses;
    using ViewModels.Transactions;
    using Xunit;

    public class ExtensionsTests{
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LogonTransactionRequest_ToTransactionRecord_IsTranslated(Boolean inTrainingMode){
            LogonTransactionRequest request = LogonTransactionRequest.Create(TestData.TransactionDateTime);
            TransactionRecord transactionRecord = request.ToTransactionRecord(inTrainingMode);
            transactionRecord.TransactionDateTime.ShouldBe(request.TransactionDateTime);
            transactionRecord.TransactionType.ShouldBe(1);
            transactionRecord.IsTrainingMode.ShouldBe(inTrainingMode);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PerformMobileTopupRequest_ToTransactionRecord_IsTranslated(Boolean inTrainingMode){
            PerformMobileTopupRequest request = PerformMobileTopupRequest.Create(TestData.TransactionDateTime,
                                                                                 TestData.OperatorId1ContractId,
                                                                                 TestData.Operator1Product_100KES.ProductId,
                                                                                 TestData.OperatorIdentifier1,
                                                                                 TestData.CustomerAccountNumber,
                                                                                 TestData.Operator1Product_100KES.Value,
                                                                                 TestData.CustomerEmailAddress);

            TransactionRecord transactionRecord = request.ToTransactionRecord(inTrainingMode);
            transactionRecord.TransactionDateTime.ShouldBe(request.TransactionDateTime);
            transactionRecord.TransactionType.ShouldBe(2);
            transactionRecord.IsTrainingMode.ShouldBe(inTrainingMode);
            transactionRecord.Amount.ShouldBe(request.TopupAmount);
            transactionRecord.ProductId.ShouldBe(request.ProductId);
            transactionRecord.ContractId.ShouldBe(request.ContractId);
            transactionRecord.CustomerAccountNumber.ShouldBe(request.CustomerAccountNumber);
            transactionRecord.CustomerEmailAddress.ShouldBe(request.CustomerEmailAddress);
            transactionRecord.OperatorIdentifier.ShouldBe(request.OperatorIdentifier);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PerformVoucherIssueRequest_ToTransactionRecord_IsTranslated(Boolean inTrainingMode){
            PerformVoucherIssueRequest request = PerformVoucherIssueRequest.Create(TestData.TransactionDateTime,
                                                                                   TestData.OperatorId3ContractId,
                                                                                   TestData.Operator3Product_200KES.ProductId,
                                                                                   TestData.OperatorIdentifier3,
                                                                                   TestData.RecipientMobileNumber,
                                                                                   TestData.RecipientEmailAddress,
                                                                                   TestData.Operator3Product_200KES.Value,
                                                                                   TestData.CustomerEmailAddress);

            TransactionRecord transactionRecord = request.ToTransactionRecord(inTrainingMode);
            transactionRecord.TransactionDateTime.ShouldBe(request.TransactionDateTime);
            transactionRecord.TransactionType.ShouldBe(2);
            transactionRecord.IsTrainingMode.ShouldBe(inTrainingMode);
            transactionRecord.Amount.ShouldBe(request.VoucherAmount);
            transactionRecord.ProductId.ShouldBe(request.ProductId);
            transactionRecord.ContractId.ShouldBe(request.ContractId);
            transactionRecord.RecipientEmailAddress.ShouldBe(request.RecipientEmailAddress);
            transactionRecord.RecipientMobileNumber.ShouldBe(request.RecipientMobileNumber);
            transactionRecord.CustomerEmailAddress.ShouldBe(request.CustomerEmailAddress);
            transactionRecord.OperatorIdentifier.ShouldBe(request.OperatorIdentifier);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PerformBillPaymentGetAccountRequest_ToTransactionRecord_IsTranslated(Boolean inTrainingMode){
            PerformBillPaymentGetAccountRequest request = PerformBillPaymentGetAccountRequest.Create(TestData.TransactionDateTime,
                                                                                                     TestData.OperatorId3ContractId,
                                                                                                     TestData.Operator3Product_200KES.ProductId,
                                                                                                     TestData.OperatorIdentifier3,
                                                                                                     TestData.CustomerAccountNumber);

            TransactionRecord transactionRecord = request.ToTransactionRecord(inTrainingMode);
            transactionRecord.TransactionDateTime.ShouldBe(request.TransactionDateTime);
            transactionRecord.TransactionType.ShouldBe(2);
            transactionRecord.IsTrainingMode.ShouldBe(inTrainingMode);
            transactionRecord.ProductId.ShouldBe(request.ProductId);
            transactionRecord.ContractId.ShouldBe(request.ContractId);
            transactionRecord.CustomerAccountNumber.ShouldBe(request.CustomerAccountNumber);
            transactionRecord.OperatorIdentifier.ShouldBe(request.OperatorIdentifier);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PerformBillPaymentMakePostPaymentRequest_ToTransactionRecord_IsTranslated(Boolean inTrainingMode){
            PerformBillPaymentMakePostPaymentRequest request = PerformBillPaymentMakePostPaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                               TestData.OperatorId3ContractId,
                                                                                                               TestData.Operator3Product_200KES.ProductId,
                                                                                                               TestData.OperatorIdentifier3,
                                                                                                               TestData.CustomerAccountNumber,
                                                                                                               TestData.CustomerAccountName,
                                                                                                               TestData.CustomerMobileNumber,
                                                                                                               TestData.PaymentAmount);

            TransactionRecord transactionRecord = request.ToTransactionRecord(inTrainingMode);
            transactionRecord.TransactionDateTime.ShouldBe(request.TransactionDateTime);
            transactionRecord.TransactionType.ShouldBe(2);
            transactionRecord.IsTrainingMode.ShouldBe(inTrainingMode);
            transactionRecord.ProductId.ShouldBe(request.ProductId);
            transactionRecord.ContractId.ShouldBe(request.ContractId);
            transactionRecord.CustomerAccountNumber.ShouldBe(request.CustomerAccountNumber);
            transactionRecord.OperatorIdentifier.ShouldBe(request.OperatorIdentifier);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PerformBillPaymentMakePrePaymentRequest_ToTransactionRecord_IsTranslated(Boolean inTrainingMode){
            PerformBillPaymentMakePostPaymentRequest request = PerformBillPaymentMakePostPaymentRequest.Create(TestData.TransactionDateTime,
                                                                                                               TestData.OperatorId3ContractId,
                                                                                                               TestData.Operator3Product_200KES.ProductId,
                                                                                                               TestData.OperatorIdentifier3,
                                                                                                               TestData.CustomerAccountNumber,
                                                                                                               TestData.CustomerAccountName,
                                                                                                               TestData.CustomerMobileNumber,
                                                                                                               TestData.PaymentAmount);

            TransactionRecord transactionRecord = request.ToTransactionRecord(inTrainingMode);
            transactionRecord.TransactionDateTime.ShouldBe(request.TransactionDateTime);
            transactionRecord.TransactionType.ShouldBe(2);
            transactionRecord.IsTrainingMode.ShouldBe(inTrainingMode);
            transactionRecord.ProductId.ShouldBe(request.ProductId);
            transactionRecord.ContractId.ShouldBe(request.ContractId);
            transactionRecord.CustomerAccountNumber.ShouldBe(request.CustomerAccountNumber);
            transactionRecord.OperatorIdentifier.ShouldBe(request.OperatorIdentifier);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PerformBillPaymentGetMeterRequest_ToTransactionRecord_IsTranslated(Boolean inTrainingMode){
            PerformBillPaymentGetMeterRequest request = PerformBillPaymentGetMeterRequest.Create(TestData.TransactionDateTime,
                                                                                                 TestData.OperatorId3ContractId,
                                                                                                 TestData.Operator3Product_200KES.ProductId,
                                                                                                 TestData.OperatorIdentifier3,
                                                                                                 TestData.CustomerAccountNumber);

            TransactionRecord transactionRecord = request.ToTransactionRecord(inTrainingMode);
            transactionRecord.TransactionDateTime.ShouldBe(request.TransactionDateTime);
            transactionRecord.TransactionType.ShouldBe(2);
            transactionRecord.IsTrainingMode.ShouldBe(inTrainingMode);
            transactionRecord.ProductId.ShouldBe(request.ProductId);
            transactionRecord.ContractId.ShouldBe(request.ContractId);
            transactionRecord.CustomerAccountNumber.ShouldBe(request.MeterNumber);
            transactionRecord.OperatorIdentifier.ShouldBe(request.OperatorIdentifier);
        }

        [Fact]
        public void TransactionRecord_UpdateFrom_PerformLogonResponseModel_IsUpdated(){
            TransactionRecord transactionRecord = new TransactionRecord();

            transactionRecord.IsSuccessful.ShouldBeFalse();

            Result<PerformLogonResponseModel> result = Result.Success(TestData.PerformLogonResponseModel);

            TransactionRecord updatedRecord = transactionRecord.UpdateFrom(result);
            updatedRecord.IsSuccessful.ShouldBe(result.Data.IsSuccessful);
            updatedRecord.ResponseMessage.ShouldBe(result.Data.ResponseMessage);
            updatedRecord.EstateId.ShouldBe(result.Data.EstateId);
            updatedRecord.MerchantId.ShouldBe(result.Data.MerchantId);
        }

        [Fact]
        public void TransactionRecord_UpdateFrom_PerformMobileTopupResponseModel_IsUpdated(){
            TransactionRecord transactionRecord = new TransactionRecord();

            transactionRecord.IsSuccessful.ShouldBeFalse();

            Result<PerformMobileTopupResponseModel> result = Result.Success(new PerformMobileTopupResponseModel{
                                                                                                                   ResponseCode = "0000"
                                                                                                               });

            TransactionRecord updatedRecord = transactionRecord.UpdateFrom(result);
            updatedRecord.IsSuccessful.ShouldBe(result.Data.IsSuccessful);
        }

        [Fact]
        public void TransactionRecord_UpdateFrom_PerformVoucherIssueResponseModel_IsUpdated(){
            TransactionRecord transactionRecord = new TransactionRecord();

            transactionRecord.IsSuccessful.ShouldBeFalse();

            Result<PerformVoucherIssueResponseModel> result = Result.Success(new PerformVoucherIssueResponseModel{
                                                                                                                     ResponseCode = "0000"
                                                                                                                 });

            TransactionRecord updatedRecord = transactionRecord.UpdateFrom(result);
            updatedRecord.IsSuccessful.ShouldBe(result.Data.IsSuccessful);
        }

        [Fact]
        public void TransactionRecord_UpdateFrom_PerformBillPaymentMakePaymentResponseModel_IsUpdated(){
            TransactionRecord transactionRecord = new TransactionRecord();

            transactionRecord.IsSuccessful.ShouldBeFalse();

            Result<PerformBillPaymentMakePaymentResponseModel> result = Result.Success(new PerformBillPaymentMakePaymentResponseModel{
                                                                                                                                         ResponseCode = "0000"
                                                                                                                                     });

            TransactionRecord updatedRecord = transactionRecord.UpdateFrom(result);
            updatedRecord.IsSuccessful.ShouldBe(result.Data.IsSuccessful);
        }

        [Fact]
        public void TransactionRecord_UpdateFrom_PerformBillPaymentGetAccountResponseModel_IsUpdated(){
            TransactionRecord transactionRecord = new TransactionRecord();

            transactionRecord.IsSuccessful.ShouldBeFalse();

            Result<PerformBillPaymentGetAccountResponseModel> result = Result.Success(new PerformBillPaymentGetAccountResponseModel{
                                                                                                                                       BillDetails = new BillDetails()
                                                                                                                                   });

            TransactionRecord updatedRecord = transactionRecord.UpdateFrom(result);
            updatedRecord.IsSuccessful.ShouldBe(result.Data.IsSuccessful);
        }

        [Fact]
        public void TransactionRecord_UpdateFrom_PerformBillPaymentGetMeterResponseModel_IsUpdated(){
            TransactionRecord transactionRecord = new TransactionRecord();

            transactionRecord.IsSuccessful.ShouldBeFalse();

            Result<PerformBillPaymentGetMeterResponseModel> result = Result.Success(new PerformBillPaymentGetMeterResponseModel{
                                                                                                                                   MeterDetails = new MeterDetails()
                                                                                                                               });

            TransactionRecord updatedRecord = transactionRecord.UpdateFrom(result);
            updatedRecord.IsSuccessful.ShouldBe(result.Data.IsSuccessful);
        }

        [Fact]
        public void PerformLogonRequestModel_ToLogonTransactionRequest_LogonTransactionRequestMessageIsCreated(){
            PerformLogonRequestModel model = new PerformLogonRequestModel{
                                                                             TransactionDateTime = TestData.TransactionDateTime,
                                                                             DeviceIdentifier = TestData.DeviceIdentifier,
                                                                             ApplicationVersion = TestData.ApplicationVersion,
                                                                             TransactionNumber = TestData.TransactionNumber
                                                                         };
            LogonTransactionRequestMessage request = model.ToLogonTransactionRequest();

            request.ShouldNotBeNull();
            request.TransactionDateTime.ShouldBe(model.TransactionDateTime);
            request.DeviceIdentifier.ShouldBe(model.DeviceIdentifier);
            request.ApplicationVersion.ShouldBe(model.ApplicationVersion);
            request.TransactionNumber.ShouldBe(model.TransactionNumber);
        }

        [Fact]
        public void PerformReconciliationRequestModel_ToReconciliationRequest_ReconciliationRequestMessageIsCreated(){
            PerformReconciliationRequestModel model = new PerformReconciliationRequestModel{
                                                                                               TransactionDateTime = TestData.TransactionDateTime,
                                                                                               DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                               ApplicationVersion = TestData.ApplicationVersion,
                                                                                               OperatorTotals = new List<OperatorTotalModel>{
                                                                                                                                                new OperatorTotalModel{
                                                                                                                                                                          ContractId = TestData.OperatorId1ContractId,
                                                                                                                                                                          OperatorIdentifier = TestData.OperatorIdentifier1,
                                                                                                                                                                          TransactionCount = 10,
                                                                                                                                                                          TransactionValue = 100.00m
                                                                                                                                                                      }
                                                                                                                                            },
                                                                                               TransactionCount = 10,
                                                                                               TransactionValue = 100.00m
                                                                                           };
            ReconciliationRequestMessage request = model.ToReconciliationRequest();

            request.ShouldNotBeNull();
            request.TransactionDateTime.ShouldBe(model.TransactionDateTime);
            request.DeviceIdentifier.ShouldBe(model.DeviceIdentifier);
            request.ApplicationVersion.ShouldBe(model.ApplicationVersion);
            request.TransactionCount.ShouldBe(model.TransactionCount);
            request.TransactionValue.ShouldBe(model.TransactionValue);
            request.OperatorTotals.Single().TransactionValue.ShouldBe(model.OperatorTotals.Single().TransactionValue);
            request.OperatorTotals.Single().TransactionCount.ShouldBe(model.OperatorTotals.Single().TransactionCount);
            request.OperatorTotals.Single().ContractId.ShouldBe(model.OperatorTotals.Single().ContractId);
            // TODO: Fix this 
            //request.OperatorTotals.Single().OperatorIdentifier.ShouldBe(model.OperatorTotals.Single().OperatorIdentifier);
        }

        [Fact]
        public void PerformVoucherIssueRequestModel_ToSaleTransactionRequest_SaleTransactionRequestMessageIsCreated(){
            PerformVoucherIssueRequestModel model = new PerformVoucherIssueRequestModel{
                                                                                           TransactionDateTime = TestData.TransactionDateTime,
                                                                                           DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                           ApplicationVersion = TestData.ApplicationVersion,
                                                                                           TransactionNumber = TestData.TransactionNumber,
                                                                                           OperatorIdentifier = TestData.OperatorIdentifier1,
                                                                                           CustomerEmailAddress = TestData.CustomerEmailAddress,
                                                                                           RecipientEmailAddress = TestData.RecipientEmailAddress,
                                                                                           VoucherAmount = TestData.PaymentAmount,
                                                                                           ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                           ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                           RecipientMobileNumber = TestData.RecipientMobileNumber
                                                                                       };
            var request = model.ToSaleTransactionRequest();

            request.ShouldNotBeNull();
            request.TransactionDateTime.ShouldBe(model.TransactionDateTime);
            request.DeviceIdentifier.ShouldBe(model.DeviceIdentifier);
            request.ApplicationVersion.ShouldBe(model.ApplicationVersion);
            request.TransactionNumber.ShouldBe(model.TransactionNumber);
            // TODO: Fix this 
            //request.OperatorIdentifier.ShouldBe(model.OperatorIdentifier);
            request.CustomerEmailAddress.ShouldBe(model.CustomerEmailAddress);
            request.ContractId.ShouldBe(model.ContractId);
            request.ProductId.ShouldBe(model.ProductId);
            request.AdditionalRequestMetaData.ContainsKey("Amount").ShouldBeTrue();
            request.AdditionalRequestMetaData["Amount"].ShouldBe(model.VoucherAmount.ToString());
            request.AdditionalRequestMetaData.ContainsKey("RecipientEmail").ShouldBeTrue();
            request.AdditionalRequestMetaData["RecipientEmail"].ShouldBe(model.RecipientEmailAddress);
            request.AdditionalRequestMetaData.ContainsKey("RecipientMobile").ShouldBeTrue();
            request.AdditionalRequestMetaData["RecipientMobile"].ShouldBe(model.RecipientMobileNumber);
        }

        [Fact]
        public void PerformMobileTopupRequestModel_ToSaleTransactionRequest_SaleTransactionRequestMessageIsCreated(){
            PerformMobileTopupRequestModel model = new PerformMobileTopupRequestModel{
                                                                                         TransactionDateTime = TestData.TransactionDateTime,
                                                                                         DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                         ApplicationVersion = TestData.ApplicationVersion,
                                                                                         TransactionNumber = TestData.TransactionNumber,
                                                                                         OperatorIdentifier = TestData.OperatorIdentifier1,
                                                                                         CustomerEmailAddress = TestData.CustomerEmailAddress,
                                                                                         TopupAmount = TestData.PaymentAmount,
                                                                                         ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                         ProductId = TestData.Operator1ProductDetails.ProductId
                                                                                     };
            var request = model.ToSaleTransactionRequest();

            request.ShouldNotBeNull();
            request.TransactionDateTime.ShouldBe(model.TransactionDateTime);
            request.DeviceIdentifier.ShouldBe(model.DeviceIdentifier);
            request.ApplicationVersion.ShouldBe(model.ApplicationVersion);
            request.TransactionNumber.ShouldBe(model.TransactionNumber);
            // TODO: Fix this 
            //request.OperatorIdentifier.ShouldBe(model.OperatorIdentifier);
            request.CustomerEmailAddress.ShouldBe(model.CustomerEmailAddress);
            request.ContractId.ShouldBe(model.ContractId);
            request.ProductId.ShouldBe(model.ProductId);
            request.AdditionalRequestMetaData.ContainsKey("Amount").ShouldBeTrue();
            request.AdditionalRequestMetaData["Amount"].ShouldBe(model.TopupAmount.ToString());
        }

        [Fact]
        public void PerformBillPaymentGetAccountModel_ToSaleTransactionRequest_SaleTransactionRequestMessageIsCreated(){
            PerformBillPaymentGetAccountModel model = new PerformBillPaymentGetAccountModel{
                                                                                               TransactionDateTime = TestData.TransactionDateTime,
                                                                                               DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                               ApplicationVersion = TestData.ApplicationVersion,
                                                                                               TransactionNumber = TestData.TransactionNumber,
                                                                                               OperatorIdentifier = TestData.OperatorIdentifier1,
                                                                                               ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                               ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                               CustomerAccountNumber = TestData.CustomerAccountNumber,
                                                                                           };
            var request = model.ToSaleTransactionRequest();

            request.ShouldNotBeNull();
            request.TransactionDateTime.ShouldBe(model.TransactionDateTime);
            request.DeviceIdentifier.ShouldBe(model.DeviceIdentifier);
            request.ApplicationVersion.ShouldBe(model.ApplicationVersion);
            request.TransactionNumber.ShouldBe(model.TransactionNumber);
            // TODO: Fix this 
            //request.OperatorIdentifier.ShouldBe(model.OperatorIdentifier);
            request.ContractId.ShouldBe(model.ContractId);
            request.ProductId.ShouldBe(model.ProductId);
            request.AdditionalRequestMetaData.ContainsKey("CustomerAccountNumber").ShouldBeTrue();
            request.AdditionalRequestMetaData["CustomerAccountNumber"].ShouldBe(model.CustomerAccountNumber);
            request.AdditionalRequestMetaData.ContainsKey("PataPawaPostPaidMessageType").ShouldBeTrue();
            request.AdditionalRequestMetaData["PataPawaPostPaidMessageType"].ShouldBe("VerifyAccount");
        }

        [Fact]
        public void PerformBillPaymentGetMeterModel_ToSaleTransactionRequest_SaleTransactionRequestMessageIsCreated(){
            PerformBillPaymentGetMeterModel model = new PerformBillPaymentGetMeterModel{
                                                                                           TransactionDateTime = TestData.TransactionDateTime,
                                                                                           DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                           ApplicationVersion = TestData.ApplicationVersion,
                                                                                           TransactionNumber = TestData.TransactionNumber,
                                                                                           OperatorIdentifier = TestData.OperatorIdentifier1,
                                                                                           ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                           ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                           MeterNumber = TestData.MeterNumber,
                                                                                       };
            var request = model.ToSaleTransactionRequest();

            request.ShouldNotBeNull();
            request.TransactionDateTime.ShouldBe(model.TransactionDateTime);
            request.DeviceIdentifier.ShouldBe(model.DeviceIdentifier);
            request.ApplicationVersion.ShouldBe(model.ApplicationVersion);
            request.TransactionNumber.ShouldBe(model.TransactionNumber);
            // TODO: Fix this 
            //request.OperatorIdentifier.ShouldBe(model.OperatorIdentifier);
            request.ContractId.ShouldBe(model.ContractId);
            request.ProductId.ShouldBe(model.ProductId);
            request.AdditionalRequestMetaData.ContainsKey("MeterNumber").ShouldBeTrue();
            request.AdditionalRequestMetaData["MeterNumber"].ShouldBe(model.MeterNumber);
            request.AdditionalRequestMetaData.ContainsKey("PataPawaPrePayMessageType").ShouldBeTrue();
            request.AdditionalRequestMetaData["PataPawaPrePayMessageType"].ShouldBe("meter");
        }

        [Fact]
        public void PerformBillPaymentMakePaymentModel_PrePayment_ToSaleTransactionRequest_SaleTransactionRequestMessageIsCreated(){
            PerformBillPaymentMakePaymentModel model = new PerformBillPaymentMakePaymentModel{
                                                                                                 TransactionDateTime = TestData.TransactionDateTime,
                                                                                                 DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                 ApplicationVersion = TestData.ApplicationVersion,
                                                                                                 TransactionNumber = TestData.TransactionNumber,
                                                                                                 OperatorIdentifier = TestData.OperatorIdentifier1,
                                                                                                 ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                 ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                 PaymentAmount = TestData.PaymentAmount,
                                                                                                 CustomerAccountNumber = TestData.CustomerAccountNumber,
                                                                                                 CustomerAccountName = TestData.CustomerAccountName,
                                                                                                 CustomerMobileNumber = TestData.CustomerMobileNumber,
                                                                                             };
            var request = model.ToSaleTransactionRequest();

            request.ShouldNotBeNull();
            request.TransactionDateTime.ShouldBe(model.TransactionDateTime);
            request.DeviceIdentifier.ShouldBe(model.DeviceIdentifier);
            request.ApplicationVersion.ShouldBe(model.ApplicationVersion);
            request.TransactionNumber.ShouldBe(model.TransactionNumber);
            // TODO: Fix this 
            //request.OperatorIdentifier.ShouldBe(model.OperatorIdentifier);
            request.ContractId.ShouldBe(model.ContractId);
            request.ProductId.ShouldBe(model.ProductId);

            request.AdditionalRequestMetaData.ContainsKey("MeterNumber").ShouldBeTrue();
            request.AdditionalRequestMetaData["MeterNumber"].ShouldBe(model.CustomerAccountNumber);
            request.AdditionalRequestMetaData.ContainsKey("CustomerName").ShouldBeTrue();
            request.AdditionalRequestMetaData["CustomerName"].ShouldBe(model.CustomerAccountName);
            request.AdditionalRequestMetaData.ContainsKey("MeterNumber").ShouldBeTrue();
            request.AdditionalRequestMetaData["MeterNumber"].ShouldBe(model.CustomerAccountNumber);
            request.AdditionalRequestMetaData.ContainsKey("Amount").ShouldBeTrue();
            request.AdditionalRequestMetaData["Amount"].ShouldBe(model.PaymentAmount.ToString());
            request.AdditionalRequestMetaData.ContainsKey("PataPawaPrePayMessageType").ShouldBeTrue();
            request.AdditionalRequestMetaData["PataPawaPrePayMessageType"].ShouldBe("vend");
        }

        [Fact]
        public void PerformBillPaymentMakePaymentModel_PostPayment_ToSaleTransactionRequest_SaleTransactionRequestMessageIsCreated(){
            PerformBillPaymentMakePaymentModel model = new PerformBillPaymentMakePaymentModel{
                                                                                                 TransactionDateTime = TestData.TransactionDateTime,
                                                                                                 DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                 ApplicationVersion = TestData.ApplicationVersion,
                                                                                                 TransactionNumber = TestData.TransactionNumber,
                                                                                                 OperatorIdentifier = TestData.OperatorIdentifier1,
                                                                                                 ContractId = TestData.Operator1ProductDetails.ContractId,
                                                                                                 ProductId = TestData.Operator1ProductDetails.ProductId,
                                                                                                 PaymentAmount = TestData.PaymentAmount,
                                                                                                 CustomerAccountNumber = TestData.CustomerAccountNumber,
                                                                                                 CustomerAccountName = TestData.CustomerAccountName,
                                                                                                 CustomerMobileNumber = TestData.CustomerMobileNumber,
                                                                                                 PostPayment = true
                                                                                             };
            var request = model.ToSaleTransactionRequest();

            request.ShouldNotBeNull();
            request.TransactionDateTime.ShouldBe(model.TransactionDateTime);
            request.DeviceIdentifier.ShouldBe(model.DeviceIdentifier);
            request.ApplicationVersion.ShouldBe(model.ApplicationVersion);
            request.TransactionNumber.ShouldBe(model.TransactionNumber);
            // TODO: Fix this 
            //request.OperatorIdentifier.ShouldBe(model.OperatorIdentifier);
            request.ContractId.ShouldBe(model.ContractId);
            request.ProductId.ShouldBe(model.ProductId);

            request.AdditionalRequestMetaData.ContainsKey("CustomerAccountNumber").ShouldBeTrue();
            request.AdditionalRequestMetaData["CustomerAccountNumber"].ShouldBe(model.CustomerAccountNumber);
            request.AdditionalRequestMetaData.ContainsKey("CustomerName").ShouldBeTrue();
            request.AdditionalRequestMetaData["CustomerName"].ShouldBe(model.CustomerAccountName);
            request.AdditionalRequestMetaData.ContainsKey("MobileNumber").ShouldBeTrue();
            request.AdditionalRequestMetaData["MobileNumber"].ShouldBe(model.CustomerMobileNumber);
            request.AdditionalRequestMetaData.ContainsKey("Amount").ShouldBeTrue();
            request.AdditionalRequestMetaData["Amount"].ShouldBe(model.PaymentAmount.ToString());
            request.AdditionalRequestMetaData.ContainsKey("PataPawaPostPaidMessageType").ShouldBeTrue();
            request.AdditionalRequestMetaData["PataPawaPostPaidMessageType"].ShouldBe("ProcessBill");
        }

        [Theory]
        [InlineData("CustomerAccountNumber")]
        [InlineData("customeraccountnumber")]
        [InlineData("CUSTOMERACCOUNTNUMBER")]
        public void ExtractFieldFromMetadata_ExtractField_FieldIsExtracted(String fieldToExtract){
            Dictionary<String, String> testMetaData = new Dictionary<String, String>{
                                                                                        { "CustomerAccountNumber", TestData.CustomerAccountNumber }
                                                                                    };

            String extractedValue = testMetaData.ExtractFieldFromMetadata<String>(fieldToExtract);
            extractedValue.ShouldBe(TestData.CustomerAccountNumber);
        }

        [Fact]
        public void ExtractFieldFromMetadata_ExtractField_KeyNotFound_DefaultValueReturned()
        {
            Dictionary<String, String> testMetaData = new Dictionary<String, String>{
                                                                                        { "CustomerAccountNumber", TestData.CustomerAccountNumber }
                                                                                    };

            String extractedValue = testMetaData.ExtractFieldFromMetadata<String>("TestValue");
            extractedValue.ShouldBeNull();
        }

        [Fact]
        public void ExtractFieldFromMetadata_ExtractField_DictionaryValueNull_DefaultValueReturned()
        {
            Dictionary<String, String> testMetaData = new Dictionary<String, String>{
                                                                                        { "CustomerAccountNumber", null }
                                                                                    };

            String extractedValue = testMetaData.ExtractFieldFromMetadata<String>("CustomerAccountNumber");
            extractedValue.ShouldBeNull();
        }

        [Fact]
        public void MetaData_ToBillDetails_BillDetailsReturned(){
            Dictionary<String, String> testMetaData = new Dictionary<String, String>{
                                                                                        { "customerAccountName", TestData.BillDetails.AccountName },
                                                                                        { "customerAccountNumber", TestData.BillDetails.AccountNumber },
                                                                                        { "customerBillBalance", TestData.BillDetails.Balance },
                                                                                        { "customerBillDueDate", TestData.BillDetails.DueDate }
                                                                                    };
            BillDetails billDetails = testMetaData.ToBillDetails();

            billDetails.AccountName.ShouldBe(TestData.BillDetails.AccountName);
            billDetails.AccountNumber.ShouldBe(TestData.BillDetails.AccountNumber);
            billDetails.Balance.ShouldBe(TestData.BillDetails.Balance);
            billDetails.DueDate.ShouldBe(TestData.BillDetails.DueDate);
        }

        [Fact]
        public void MetaData_ToMeterDetails_MeterDetailsReturned()
        {
            Dictionary<String, String> testMetaData = new Dictionary<String, String>{
                                                                                        { "pataPawaPrePaidCustomerName", TestData.MeterDetails.CustomerName }
                                                                                    };
            MeterDetails meterDetails = testMetaData.ToMeterDetails(TestData.MeterNumber);

            meterDetails.CustomerName.ShouldBe(TestData.MeterDetails.CustomerName);
            meterDetails.MeterNumber.ShouldBe(TestData.MeterNumber);
        }

        [Theory]
        [InlineData("0000", true)]
        [InlineData("0001", false)]
        public void SaleTransactionResponseMessage_IsSuccessfulTransaction_ExpectedValueReturned(String responseCode, Boolean expectedValue){
            SaleTransactionResponseMessage responseMessage = new SaleTransactionResponseMessage{
                                                                                                   ResponseCode = responseCode
                                                                                               };
            responseMessage.IsSuccessfulTransaction().ShouldBe(expectedValue);
        }

        [Theory]
        [InlineData("0000", true)]
        [InlineData("0001", false)]
        public void ReconciliationResponseMessage_IsSuccessfulTransaction_ExpectedValueReturned(String responseCode, Boolean expectedValue)
        {
            ReconciliationResponseMessage responseMessage = new ReconciliationResponseMessage
            {
                                                                ResponseCode = responseCode
                                                            };
            responseMessage.IsSuccessfulReconciliation().ShouldBe(expectedValue);
        }
    }

    public class ResultExtensionsTests{
        [Fact]
        public void FailureExtended_FailureResultReturned(){
            var result = ResultExtensions.FailureExtended("Message", new Exception("ExceptionsMessage"));
            result.IsFailed.ShouldBeTrue();
            result.Message.ShouldBe("Message");
            result.Errors.Single().ShouldBe("ExceptionsMessage");
        }
    }
}
