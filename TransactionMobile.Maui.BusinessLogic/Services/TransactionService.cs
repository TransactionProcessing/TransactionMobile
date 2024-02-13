namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using ClientProxyBase;
    using Common;
    using Logging;
    using Models;
    using Newtonsoft.Json;
    using RequestHandlers;
    using TransactionProcessorACL.DataTransferObjects;
    using TransactionProcessorACL.DataTransferObjects.Responses;
    using ViewModels;
    using ViewModels.Transactions;

    public class TransactionService : ClientProxyBase, ITransactionService
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

            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), cancellationToken);

            if (result.Success == false)
            {
                Logger.LogWarning("Error performing bill payment - get account transaction");
                return new ErrorResult<PerformBillPaymentGetAccountResponseModel>("Error performing bill payment - get account transaction");

            }

            // TODO: Factory
            PerformBillPaymentGetAccountResponseModel responseModel = new()
                                                                      {
                                                                          IsSuccessful = result.Data.IsSuccessfulTransaction(),
                                                                          BillDetails = result.Data.AdditionalResponseMetaData.ToBillDetails()
                                                                      };

            Logger.LogInformation("Bill payment - get account transaction performed successfully");

            return new SuccessResult<PerformBillPaymentGetAccountResponseModel>(responseModel);
        }

        public async Task<Result<PerformBillPaymentMakePaymentResponseModel>> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model,
                                                                                                            CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform bill payment make payment transaction");

            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), cancellationToken);
            
            if (result.Success == false)
            {
                Logger.LogWarning("Error performing bill payment - make payment transaction");
                return new ErrorResult<PerformBillPaymentMakePaymentResponseModel>("Error performing bill payment - make payment transaction");

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

            return new SuccessResult<PerformBillPaymentMakePaymentResponseModel>(responseModel);
        }

        public async Task<Result<PerformBillPaymentGetMeterResponseModel>> PerformBillPaymentGetMeter(PerformBillPaymentGetMeterModel model, CancellationToken cancellationToken){
            Logger.LogInformation("About to perform bill payment get meter transaction");

            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), cancellationToken);

            if (result.Success == false)
            {
                Logger.LogWarning("Error performing bill payment - get account transaction");
                return new ErrorResult<PerformBillPaymentGetMeterResponseModel>("Error performing bill payment - get meter transaction");

            }

            // TODO: Factory
            PerformBillPaymentGetMeterResponseModel responseModel = new()
                                                                    {
                                                                        IsSuccessful = result.Data.IsSuccessfulTransaction(),
                                                                        MeterDetails = result.Data.AdditionalResponseMetaData.ToMeterDetails()
                                                                    };

            Logger.LogInformation("Bill payment - get meter transaction performed successfully");

            return new SuccessResult<PerformBillPaymentGetMeterResponseModel>(responseModel);
        }

        public async Task<Result<PerformLogonResponseModel>> PerformLogon(PerformLogonRequestModel model,
                                                                          CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform logon transaction");

            Result<LogonTransactionResponseMessage> result =  await this.SendTransactionRequest<LogonTransactionRequestMessage, LogonTransactionResponseMessage>(model.ToLogonTransactionRequest(), cancellationToken);

            if (result.Success == false)
            {
                Logger.LogWarning("Error performing Logon transaction");
                return new ErrorResult<PerformLogonResponseModel>("Error performing Logon transaction");

            }

            // TODO: Factory
            PerformLogonResponseModel responseModel = new()
                                                      {
                                                          EstateId = result.Data.EstateId,
                                                          MerchantId = result.Data.MerchantId,
                                                          ResponseCode = result.Data.ResponseCode,
                                                          ResponseMessage = result.Data.ResponseMessage
                                                      };

            Logger.LogInformation("Logon transaction performed successfully");

            return new SuccessResult<PerformLogonResponseModel>(responseModel);
        }

        public async Task<Result<PerformMobileTopupResponseModel>> PerformMobileTopup(PerformMobileTopupRequestModel model,
                                                                                      CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform mobile top-up transaction");

            Result<SaleTransactionResponseMessage> result =
                await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), cancellationToken);

            if (result.Success == false) {
                Logger.LogWarning("Error performing Mobile top-up transaction");
                return new ErrorResult<PerformMobileTopupResponseModel>("Error performing Mobile top-up transaction");
                
            }

            // TODO: Factory
            PerformMobileTopupResponseModel responseModel = new (){
                                                                                                    EstateId = result.Data.EstateId,
                                                                                                    MerchantId = result.Data.MerchantId,
                                                                                                    ResponseCode = result.Data.ResponseCode,
                                                                                                    ResponseMessage = result.Data.ResponseMessage
                                                                                                };

            Logger.LogInformation("Mobile top-up transaction performed successfully");

            return new SuccessResult<PerformMobileTopupResponseModel>(responseModel);

        }

        public async Task<Result<PerformReconciliationResponseModel>> PerformReconciliation(PerformReconciliationRequestModel model,
                                                                                            CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform reconciliation transaction");

            Result<ReconciliationResponseMessage> result = await this.SendTransactionRequest<ReconciliationRequestMessage, ReconciliationResponseMessage>(model.ToReconciliationRequest(), cancellationToken);
            
            if (result.Success == false)
            {
                Logger.LogWarning("Error performing Reconciliation transaction");
                return new ErrorResult<PerformReconciliationResponseModel>("Error performing Reconciliation transaction");

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

            return new SuccessResult<PerformReconciliationResponseModel>(responseModel);

        }

        public async Task<Result<PerformVoucherIssueResponseModel>> PerformVoucherIssue(PerformVoucherIssueRequestModel model,
                                                                                      CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform voucher transaction");

            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), cancellationToken);
            
            if (result.Success == false)
            {
                Logger.LogWarning("Error performing Voucher transaction");
                return new ErrorResult<PerformVoucherIssueResponseModel>("Error performing Voucher transaction");

            }

            // TODO: Factory
            PerformVoucherIssueResponseModel responseModel = new()
                                                             {
                                                                 EstateId = result.Data.EstateId,
                                                                 MerchantId = result.Data.MerchantId,
                                                                 ResponseCode = result.Data.ResponseCode,
                                                                 ResponseMessage = result.Data.ResponseMessage
                                                             };

            Logger.LogInformation("Voucher transaction performed successfully");

            return new SuccessResult<PerformVoucherIssueResponseModel>(responseModel);
        }
        
        protected override async Task<String> HandleResponse(HttpResponseMessage responseMessage,
                                                             CancellationToken cancellationToken) {
            if (responseMessage.StatusCode == HttpStatusCode.HttpVersionNotSupported) {
                throw new ApplicationException("Application needs to be updated to the latest version");
            }

            return await base.HandleResponse(responseMessage, cancellationToken);
        }

        private String BuildRequestUrl(String route) {
            String baseAddress = this.BaseAddressResolver("TransactionProcessorACL");

            String requestUri = $"{baseAddress}{route}";

            return requestUri;
        }

        private async Task<Result<TResponse>> SendTransactionRequest<TRequest, TResponse>(TRequest request,
                                                                                  CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl("/api/transactions");

            try {
                String requestSerialised = JsonConvert.SerializeObject(request,
                                                                       new JsonSerializerSettings {
                                                                                                      TypeNameHandling = TypeNameHandling.All
                                                                                                  });

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();

                Logger.LogDebug($"Transaction Request details: Uri {requestUri} Payload {requestSerialised} Access Token {accessToken.AccessToken}");

                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);



                Logger.LogDebug($"Transaction Response details:  Status {httpResponse.StatusCode} Payload {content}");

                return new SuccessResult<TResponse>(JsonConvert.DeserializeObject<TResponse>(content));

            }
            catch(Exception ex) {
                // An exception has occurred, add some additional information to the message
                Logger.LogError("Error posting transaction.", ex);

                return new ErrorResult<TResponse>("Error posting transaction");
            }
        }

        #endregion
    }
}