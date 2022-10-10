namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using ClientProxyBase;
    using Common;
    using Models;
    using Newtonsoft.Json;
    using Shared.Logger;
    using TransactionProcessorACL.DataTransferObjects;
    using TransactionProcessorACL.DataTransferObjects.Responses;

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

        public async Task<PerformBillPaymentGetAccountResponseModel> PerformBillPaymentGetAccount(PerformBillPaymentGetAccountModel model,
                                                                                                  CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform bill payment get account transaction");

            SaleTransactionResponseMessage responseMessage = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), cancellationToken);

            Logger.LogInformation("Bill payment get account transaction performed successfully");

            // Convert to model
            PerformBillPaymentGetAccountResponseModel response = new PerformBillPaymentGetAccountResponseModel {
                                                                                                                   BillDetails = responseMessage
                                                                                                                       .AdditionalResponseMetaData.ToBillDetails(),
                                                                                                                   IsSuccessful = responseMessage.ResponseCode == "0000"
                                                                                                               };

            return response;
        }

        public async Task<Boolean> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model,
                                                                 CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform bill payment make payment transaction");

            SaleTransactionResponseMessage response = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), cancellationToken);

            Logger.LogInformation("Bill payment make payment transaction performed successfully");

            return response.ResponseCode == "0000";
        }

        public async Task<PerformLogonResponseModel> PerformLogon(PerformLogonRequestModel model,
                                                                  CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform logon transaction");

            SaleTransactionResponseMessage response =  await this.SendTransactionRequest<LogonTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToLogonTransactionRequest(), cancellationToken);
            
            Logger.LogInformation("Logon transaction performed successfully");

            return new PerformLogonResponseModel {
                                                     EstateId = response.EstateId,
                                                     IsSuccessful = response.ResponseCode == "0000",
                                                     MerchantId = response.MerchantId,
                                                     ResponseMessage = response.ResponseMessage,
                                                 };
        }

        public async Task<Boolean> PerformMobileTopup(PerformMobileTopupRequestModel model,
                                                      CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform mobile top-up transaction");

            SaleTransactionResponseMessage response = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), cancellationToken);

            Logger.LogInformation("Mobile top-up transaction performed successfully");

            return response.ResponseCode == "0000";
        }

        public async Task<Boolean> PerformReconciliation(PerformReconciliationRequestModel model,
                                                         CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform reconciliation transaction");

            ReconciliationResponseMessage response = await this.SendTransactionRequest<ReconciliationRequestMessage, ReconciliationResponseMessage>(model.ToReconciliationRequest(), cancellationToken);

            Logger.LogInformation("Reconciliation transaction performed successfully");

            return response.ResponseCode == "0000";
        }

        public async Task<Boolean> PerformVoucherIssue(PerformVoucherIssueRequestModel model,
                                                       CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform voucher transaction");

            SaleTransactionResponseMessage response = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), cancellationToken);

            Logger.LogInformation("Voucher transaction performed successfully");

            return response.ResponseCode == "0000";
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

        private async Task<TResponse> SendTransactionRequest<TRequest, TResponse>(TRequest request,
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
                
                return JsonConvert.DeserializeObject<TResponse>(content);

            }
            catch(Exception ex) {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error posting transaction.", ex);

                Logger.LogError(exception);

                throw exception;
            }
        }

        #endregion
    }
}