using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using System.Net;
    using System.Net.Http.Headers;
    using ClientProxyBase;
    using Microsoft.Extensions.Caching.Memory;
    using Models;
    using Newtonsoft.Json;
    using TransactionProcessorACL.DataTransferObjects;
    using TransactionProcessorACL.DataTransferObjects.Responses;

    public class TransactionService : ClientProxyBase, ITransactionService
    {
        private readonly Func<String, String> BaseAddressResolver;

        private readonly IMemoryCacheService MemoryCacheService;

        private String BuildRequestUrl(String route)
        {
            String baseAddress = this.BaseAddressResolver("TransactionProcessorACL");

            String requestUri = $"{baseAddress}{route}";

            return requestUri;
        }

        protected override async Task<String> HandleResponse(HttpResponseMessage responseMessage,
                                                             CancellationToken cancellationToken)
        {
            if (responseMessage.StatusCode == HttpStatusCode.HttpVersionNotSupported)
            {
                throw new ApplicationException("Application needs to be updated to the latest version");
            }
            else
            {
                return await base.HandleResponse(responseMessage, cancellationToken);
            }

        }

        public TransactionService(Func<String, String> baseAddressResolver,
                                  HttpClient httpClient,
                                  IMemoryCacheService memoryCacheService) : base(httpClient)
        {
            this.BaseAddressResolver = baseAddressResolver;
            this.MemoryCacheService = memoryCacheService;

            // Add the API version header
            this.HttpClient.DefaultRequestHeaders.Add("api-version", "1.0");
        }

        public async Task<PerformLogonResponseModel> PerformLogon(PerformLogonRequestModel model,
                                                                  CancellationToken cancellationToken)
        {
            PerformLogonResponseModel response = null;

            String requestUri = this.BuildRequestUrl("/api/transactions");

            try
            {
                LogonTransactionRequestMessage logonTransactionRequest = new LogonTransactionRequestMessage
                                                                         {
                                                                             ApplicationVersion = "1.0.5", //model.ApplicationVersion,
                                                                             DeviceIdentifier = model.DeviceIdentifier,
                                                                             TransactionDateTime = model.TransactionDateTime,
                                                                             TransactionNumber = model.TransactionNumber
                                                                         };

                String requestSerialised = JsonConvert.SerializeObject(logonTransactionRequest,
                                                                       new JsonSerializerSettings
                                                                       {
                                                                           TypeNameHandling = TypeNameHandling.All
                                                                       });

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.MemoryCacheService.TryGetValue<TokenResponseModel>("AccessToken", out TokenResponseModel accessToken);
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                LogonTransactionResponseMessage responseMessage = JsonConvert.DeserializeObject<LogonTransactionResponseMessage>(content);

                // Convert to model
                response = new PerformLogonResponseModel
                           {
                               EstateId = responseMessage.EstateId,
                               MerchantId = responseMessage.MerchantId,
                               IsSuccessful = responseMessage.ResponseCode == "0000",
                               RequireApplicationUpdate = responseMessage.RequiresApplicationUpdate,
                               ResponseMessage = responseMessage.ResponseMessage
                           };
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error posting logon transaction.", ex);

                throw exception;
            }

            return response;

        }

        public async Task<Boolean> PerformMobileTopup(PerformMobileTopupRequestModel model,
                                                      CancellationToken cancellationToken)
        {
            Boolean response = false;
            String requestUri = this.BuildRequestUrl("/api/transactions");

            try
            {
                SaleTransactionRequestMessage saleTransactionRequest = new SaleTransactionRequestMessage
                                                                       {
                                                                           ProductId = model.ProductId,
                                                                           OperatorIdentifier = model.OperatorIdentifier,
                                                                           ApplicationVersion = "1.0.5", //model.ApplicationVersion,
                                                                           DeviceIdentifier = model.DeviceIdentifier,
                                                                           ContractId = model.ContractId,
                                                                           TransactionDateTime = model.TransactionDateTime,
                                                                           CustomerEmailAddress = model.CustomerEmailAddress,
                                                                           TransactionNumber = model.TransactionNumber
                                                                       };

                // Add the additional request data
                saleTransactionRequest.AdditionalRequestMetaData = new Dictionary<String, String>();
                saleTransactionRequest.AdditionalRequestMetaData.Add("Amount", model.TopupAmount.ToString());
                saleTransactionRequest.AdditionalRequestMetaData.Add("CustomerAccountNumber", model.CustomerAccountNumber);

                String requestSerialised = JsonConvert.SerializeObject(saleTransactionRequest,
                                                                       new JsonSerializerSettings
                                                                       {
                                                                           TypeNameHandling = TypeNameHandling.All
                                                                       });

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.MemoryCacheService.TryGetValue<TokenResponseModel>("AccessToken", out TokenResponseModel accessToken);
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                SaleTransactionResponseMessage responseMessage = JsonConvert.DeserializeObject<SaleTransactionResponseMessage>(content);

                response = responseMessage.ResponseCode == "0000";
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error posting sale transaction.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<Boolean> PerformReconciliation(PerformReconciliationRequestModel model, CancellationToken cancellationToken)
        {
            Boolean response = false;
            String requestUri = this.BuildRequestUrl("/api/transactions");

            try
            {
                ReconciliationRequestMessage reconciliationRequest = new ReconciliationRequestMessage
                                                                     {
                                                                         ApplicationVersion = model.ApplicationVersion,
                                                                         TransactionDateTime = model.TransactionDateTime,
                                                                         DeviceIdentifier = model.DeviceIdentifier,
                                                                         TransactionCount = model.TransactionCount,
                                                                         TransactionValue = model.TransactionValue,
                                                                         OperatorTotals = new List<OperatorTotalRequest>()
                                                                     };
                foreach (OperatorTotalModel modelOperatorTotal in model.OperatorTotals)
                {
                    reconciliationRequest.OperatorTotals.Add(new OperatorTotalRequest
                                                             {
                                                                 OperatorIdentifier = modelOperatorTotal.OperatorIdentifier,
                                                                 TransactionValue = modelOperatorTotal.TransactionValue,
                                                                 ContractId = modelOperatorTotal.ContractId,
                                                                 TransactionCount = modelOperatorTotal.TransactionCount
                                                             });
                }

                String requestSerialised = JsonConvert.SerializeObject(reconciliationRequest,
                                                                       new JsonSerializerSettings
                                                                       {
                                                                           TypeNameHandling = TypeNameHandling.All
                                                                       });

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.MemoryCacheService.TryGetValue<TokenResponseModel>("AccessToken", out TokenResponseModel accessToken);
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                ReconciliationResponseMessage responseMessage = JsonConvert.DeserializeObject<ReconciliationResponseMessage>(content);

                response = responseMessage.ResponseCode == "0000";
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error posting reconciliation transaction.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<Boolean> PerformVoucherIssue(PerformVoucherIssueRequestModel model,
                                                       CancellationToken cancellationToken)
        {
            Boolean response = false;
            String requestUri = this.BuildRequestUrl("/api/transactions");

            try
            {
                SaleTransactionRequestMessage saleTransactionRequest = new SaleTransactionRequestMessage
                {
                    ProductId = model.ProductId,
                    OperatorIdentifier = model.OperatorIdentifier,
                    ApplicationVersion = "1.0.5", //model.ApplicationVersion,
                    DeviceIdentifier = model.DeviceIdentifier,
                    ContractId = model.ContractId,
                    TransactionDateTime = model.TransactionDateTime,
                    CustomerEmailAddress = model.CustomerEmailAddress,
                    TransactionNumber = model.TransactionNumber
                };

                // Add the additional request data
                saleTransactionRequest.AdditionalRequestMetaData = new Dictionary<String, String>();
                saleTransactionRequest.AdditionalRequestMetaData.Add("Amount", model.VoucherAmount.ToString());
                saleTransactionRequest.AdditionalRequestMetaData.Add("RecipientEmail", model.RecipientEmailAddress);
                saleTransactionRequest.AdditionalRequestMetaData.Add("RecipientMobile", model.RecipientMobileNumber);

                String requestSerialised = JsonConvert.SerializeObject(saleTransactionRequest,
                                                                       new JsonSerializerSettings
                                                                       {
                                                                           TypeNameHandling = TypeNameHandling.All
                                                                       });

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.MemoryCacheService.TryGetValue<TokenResponseModel>("AccessToken", out TokenResponseModel accessToken);
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                SaleTransactionResponseMessage responseMessage = JsonConvert.DeserializeObject<SaleTransactionResponseMessage>(content);

                response = responseMessage.ResponseCode == "0000";
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error posting sale transaction.", ex);

                throw exception;
            }

            return response;
        }
    }
}
