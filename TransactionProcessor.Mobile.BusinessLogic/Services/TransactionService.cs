using System.Net;
using System.Net.Http.Headers;
using System.Text;
using ClientProxyBase;
using Newtonsoft.Json;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessorACL.DataTransferObjects;
using TransactionProcessorACL.DataTransferObjects.Responses;

namespace TransactionProcessor.Mobile.BusinessLogic.Services
{
    public interface ITransactionService
    {
        #region Methods

        Task<Result<PerformLogonResponseModel>> PerformLogon(PerformLogonRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformMobileTopupResponseModel>> PerformMobileTopup(PerformMobileTopupRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformReconciliationResponseModel>> PerformReconciliation(PerformReconciliationRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformVoucherIssueResponseModel>> PerformVoucherIssue(PerformVoucherIssueRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformBillPaymentGetAccountResponseModel>> PerformBillPaymentGetAccount(PerformBillPaymentGetAccountModel model, CancellationToken cancellationToken);

        Task<Result<PerformBillPaymentMakePaymentResponseModel>> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model, CancellationToken cancellationToken);

        Task<Result<PerformBillPaymentGetMeterResponseModel>> PerformBillPaymentGetMeter(PerformBillPaymentGetMeterModel model, CancellationToken cancellationToken);

        #endregion
    }

    //[ExcludeFromCodeCoverage(Justification = "Need to refactor to allow injection of client for mocking")]
    public class TransactionService : ClientProxyBase.ClientProxyBase, ITransactionService
    {
        #region Fields

        private readonly IApplicationCache ApplicationCache;

        private readonly Func<String, String> BaseAddressResolver;

        #endregion

        #region Constructors

        public TransactionService(Func<String, String> baseAddressResolver,
                                  HttpClient httpClient,
                                  IApplicationCache applicationCache) : base(httpClient) {
            this.BaseAddressResolver = baseAddressResolver;
            this.ApplicationCache = applicationCache;

            // Add the API version header
            this.HttpClient.DefaultRequestHeaders.Add("api-version", "1.0");
        }

        #endregion

        #region Methods

        public async Task<Result<PerformBillPaymentGetAccountResponseModel>> PerformBillPaymentGetAccount(PerformBillPaymentGetAccountModel model,
                                                                                                          CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform bill payment get account transaction");

            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), "api/saletransactions", cancellationToken);

            if (result.IsSuccess == false)
            {
                Logger.LogWarning("Error performing bill payment - get account transaction");
                return Result.Failure("Error performing bill payment - get account transaction");
            }

            PerformBillPaymentGetAccountResponseModel responseModel = new()
                                                                      {
                                                                          BillDetails = result.Data.AdditionalResponseMetadata.ToBillDetails()
                                                                      };

            Logger.LogInformation("Bill payment - get account transaction performed successfully");

            return Result.Success(responseModel);
        }

        public async Task<Result<PerformBillPaymentMakePaymentResponseModel>> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model,
                                                                                                            CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform bill payment make payment transaction");
            
            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(),"api/saletransactions", cancellationToken);
            
            if (result.IsSuccess == false)
            {
                Logger.LogWarning("Error performing bill payment - make payment transaction");
                return Result.Failure("Error performing bill payment - make payment transaction");

            }

            // TODO: Factory
            PerformBillPaymentMakePaymentResponseModel responseModel = new()
                                                                       {
                                                                           EstateId = result.Data.EstateId,
                                                                           MerchantId = result.Data.EstateId,
                                                                           ResponseCode = result.Data.ResponseCode,
                                                                           ResponseMessage = result.Data.ResponseMessage
                                                                       };

            Logger.LogInformation("Bill payment - make payment transaction performed successfully");

            return Result.Success(responseModel);
        }

        public async Task<Result<PerformBillPaymentGetMeterResponseModel>> PerformBillPaymentGetMeter(PerformBillPaymentGetMeterModel model, CancellationToken cancellationToken){
            Logger.LogInformation("About to perform bill payment get meter transaction");

            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), "api/saletransactions", cancellationToken);

            if (result.IsSuccess == false)
            {
                Logger.LogWarning("Error performing bill payment - get meter transaction");
                return Result.Failure("Error performing bill payment - get meter transaction");

            }

            MeterDetails meterDetails = result.Data.AdditionalResponseMetadata switch{
                null => null,
                _ => result.Data.AdditionalResponseMetadata.ToMeterDetails(model.MeterNumber)
            };

            PerformBillPaymentGetMeterResponseModel responseModel = new()
                                                                    {
                                                                        MeterDetails = meterDetails
            };

            Logger.LogInformation("Bill payment - get meter transaction performed successfully");

            return Result.Success(responseModel);
        }

        public async Task<Result<PerformLogonResponseModel>> PerformLogon(PerformLogonRequestModel model,
                                                                          CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform logon transaction");

            Result<LogonTransactionResponseMessage> result =  await this.SendTransactionRequest<LogonTransactionRequestMessage, LogonTransactionResponseMessage>(model.ToLogonTransactionRequest(), "api/logontransactions", cancellationToken);

            if (result.IsSuccess == false)
            {
                Logger.LogWarning("Error performing Logon transaction");
                return Result.Failure("Error performing Logon transaction");
            }

            // TODO: Factory
            PerformLogonResponseModel responseModel = new()
                                                      {
                                                          EstateId = result.Data.EstateId,
                                                          MerchantId = result.Data.MerchantId,
                                                          ResponseCode = result.Data.ResponseCode,
                                                          ResponseMessage = result.Data.ResponseMessage,
                                                      };

            Logger.LogInformation("Logon transaction performed successfully");

            return Result.Success(responseModel);
        }

        public async Task<Result<PerformMobileTopupResponseModel>> PerformMobileTopup(PerformMobileTopupRequestModel model,
                                                                                      CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform mobile top-up transaction");

            Result<SaleTransactionResponseMessage> result =
                await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), "api/saletransactions", cancellationToken);

            if (result.IsSuccess == false) {
                Logger.LogWarning("Error performing Mobile top-up transaction");
                return Result.Failure("Error performing Mobile top-up transaction");
                
            }

            PerformMobileTopupResponseModel responseModel = new (){
                                                                                                    EstateId = result.Data.EstateId,
                                                                                                    MerchantId = result.Data.MerchantId,
                                                                                                    ResponseCode = result.Data.ResponseCode,
                                                                                                    ResponseMessage = result.Data.ResponseMessage
                                                                                                };

            Logger.LogInformation("Mobile top-up transaction performed successfully");

            return Result.Success(responseModel);

        }

        public async Task<Result<PerformReconciliationResponseModel>> PerformReconciliation(PerformReconciliationRequestModel model,
                                                                                            CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform reconciliation transaction");

            Result<ReconciliationResponseMessage> result = await this.SendTransactionRequest<ReconciliationRequestMessage, ReconciliationResponseMessage>(model.ToReconciliationRequest(), "api/reconciliationtransactions", cancellationToken);
            
            if (result.IsSuccess == false)
            {
                Logger.LogWarning("Error performing Reconciliation transaction");
                return Result.Failure("Error performing Reconciliation transaction");

            }

            // TODO: Factory
            PerformReconciliationResponseModel responseModel = new()
                                                               {
                                                                   EstateId = result.Data.EstateId,
                                                                   MerchantId = result.Data.MerchantId,
                                                                   ResponseCode = result.Data.ResponseCode,
                                                                   ResponseMessage = result.Data.ResponseMessage
                                                               };

            Logger.LogInformation("Reconciliation transaction performed successfully");

            return Result.Success(responseModel);

        }

        public async Task<Result<PerformVoucherIssueResponseModel>> PerformVoucherIssue(PerformVoucherIssueRequestModel model,
                                                                                      CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform voucher transaction");

            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), "api/saletransactions", cancellationToken);
            
            if (result.IsSuccess == false)
            {
                Logger.LogWarning("Error performing Voucher transaction");
                return Result.Failure("Error performing Voucher transaction");
            }

            PerformVoucherIssueResponseModel responseModel = new()
                                                             {
                                                                 EstateId = result.Data.EstateId,
                                                                 MerchantId = result.Data.MerchantId,
                                                                 ResponseCode = result.Data.ResponseCode,
                                                                 ResponseMessage = result.Data.ResponseMessage
                                                             };

            Logger.LogInformation("Voucher transaction performed successfully");

            return Result.Success(responseModel);
        }
        
        protected override async Task<Result<String>> HandleResponseX(HttpResponseMessage responseMessage,
                                                                      CancellationToken cancellationToken) {
            if (responseMessage.StatusCode == HttpStatusCode.HttpVersionNotSupported) {
                throw new ApplicationException("Application needs to be updated to the latest version");
            }

            return await base.HandleResponseX(responseMessage, cancellationToken);
        }

        private String BuildRequestUrl(String route) {
            String baseAddress = this.BaseAddressResolver("TransactionProcessorACL");

            String requestUri = $"{baseAddress}/{route}";

            return requestUri;
        }

        private async Task<Result<TResponse>> SendTransactionRequest<TRequest, TResponse>(TRequest request,
                                                                                          String route, 
                                                                                          CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl(route);
            try {
                String requestSerialised = JsonConvert.SerializeObject(request, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();

                Logger.LogDebug($"Transaction Request details: Uri {requestUri} Payload {requestSerialised} Access Token {accessToken.AccessToken}");

                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsSuccess == false) {
                    Logger.LogWarning("Error performing Voucher transaction");
                    return Result.Failure("Error performing Voucher transaction");
                }

                Logger.LogDebug($"Transaction Response details:  Status {httpResponse.StatusCode} Payload {result.Data}");

                var responseData = JsonConvert.DeserializeObject<TResponse>(result.Data);

                return Result.Success(responseData);
            }
            catch (Exception ex) {
                // An exception has occurred, add some additional information to the message
                Logger.LogError("Error posting transaction.", ex);

                return ResultExtensions.FailureExtended("Error posting transaction", ex);
            }
        }

        #endregion
    }
}