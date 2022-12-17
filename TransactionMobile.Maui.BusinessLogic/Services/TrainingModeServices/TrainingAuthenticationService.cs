using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services.TrainingModeServices
{
    using Models;
    using RequestHandlers;
    using TransactionProcessorACL.DataTransferObjects.Responses;
    using ViewModels.Transactions;

    public class TrainingAuthenticationService : IAuthenticationService
    {
        public async Task<Result<TokenResponseModel>> GetToken(String username,
                                                               String password,
                                                               CancellationToken cancellationToken) {
            return new SuccessResult<TokenResponseModel>(new TokenResponseModel {
                                                                                    AccessToken = "Token",
                                                                                    RefreshToken = "RefreshToken",
                                                                                    ExpiryInMinutes = 1
                                                                                });
        }

        public async Task<Result<TokenResponseModel>> RefreshAccessToken(String refreshToken,
                                                                         CancellationToken cancellationToken) {
            return new SuccessResult<TokenResponseModel>(new TokenResponseModel
                                                         {
                                                             AccessToken = "Token",
                                                             RefreshToken = "RefreshToken",
                                                             ExpiryInMinutes = 1
                                                         });
        }
    }

    public class TrainingConfigurationService : IConfigurationService
    {
        public async Task<Result<Configuration>> GetConfiguration(String deviceIdentifier,
                                                                  CancellationToken cancellationToken) {
            return new SuccessResult<Configuration>(new Configuration {
                                                                          ClientId = "dummyClientId",
                                                                          ClientSecret = "dummyClientSecret",
                                                                          EnableAutoUpdates = false,
                                                                          ShowDebugMessages = true,
                                                                          EstateManagementUri = "http://localhost:5000",
                                                                          EstateReportingUri = "http://localhost:5006",
                                                                          LogLevel = LogLevel.Debug,
                                                                          SecurityServiceUri = "http://localhost:5001",
                                                                          TransactionProcessorAclUri = "http://localhost:5003",
                                                                          AppCenterConfig = new AppCenterConfiguration {
                                                                                                AndroidKey = String.Empty,
                                                                                                MacOSKey = String.Empty,
                                                                                                WindowsKey = String.Empty,
                                                                                                iOSKey = String.Empty
                                                                                            }
                                                                      });
        }

        public async Task PostDiagnosticLogs(String deviceIdentifier,
                                             List<LogMessage> logMessages,
                                             CancellationToken cancellationToken) {
            
        }
    }

    public class TrainingMerchantService : IMerchantService
    {
        public async Task<Result<List<ContractProductModel>>> GetContractProducts(CancellationToken cancellationToken) {
            return new SuccessResult<List<ContractProductModel>>(new List<ContractProductModel> {
                                                                                                    new ContractProductModel {
                                                                                                                                 ContractId = Guid.Parse("21DA6AC5-70E6-478E-A5E9-74C9B27F5725"),
                                                                                                                                 IsFixedValue = true,
                                                                                                                                 OperatorId = Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967"),
                                                                                                                                 OperatorIdentfier = "Safaricom",
                                                                                                                                 OperatorName = "Safaricom",
                                                                                                                                 ProductDisplayText = "100 KES",
                                                                                                                                 ProductId = Guid.Parse("CBF55D95-306A-4E85-B367-24FA442998F6"),
                                                                                                                                 ProductType = ProductType.Voucher,
                                                                                                                                 Value = 100
                                                                                                                             },
                                                                                                    new ContractProductModel {
                                                                                                                                 ContractId = Guid.Parse("21DA6AC5-70E6-478E-A5E9-74C9B27F5725"),
                                                                                                                                 IsFixedValue = true,
                                                                                                                                 OperatorId = Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967"),
                                                                                                                                 OperatorIdentfier = "Safaricom",
                                                                                                                                 OperatorName = "Safaricom",
                                                                                                                                 ProductDisplayText = "200 KES",
                                                                                                                                 ProductId = Guid.Parse("F5F9B63F-9F68-45E8-960B-CE0FC15ED672"),
                                                                                                                                 ProductType = ProductType.Voucher,
                                                                                                                                 Value = 200
                                                                                                                             },
                                                                                                    new ContractProductModel {
                                                                                                                                 ContractId = Guid.Parse("21DA6AC5-70E6-478E-A5E9-74C9B27F5725"),
                                                                                                                                 IsFixedValue = false,
                                                                                                                                 OperatorId = Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967"),
                                                                                                                                 OperatorIdentfier = "Safaricom",
                                                                                                                                 OperatorName = "Safaricom",
                                                                                                                                 ProductDisplayText = "Custom",
                                                                                                                                 ProductId = Guid.Parse("268CBAF5-95E0-4D4C-9725-3BB2B76E4273"),
                                                                                                                                 ProductType = ProductType.Voucher
                                                                                                                             },
                                                                                                    new ContractProductModel {
                                                                                                                                 ContractId = Guid.Parse("D57DAC9B-4039-4120-B5A8-F7FDF1D3A3C2"),
                                                                                                                                 IsFixedValue = true,
                                                                                                                                 OperatorId = Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967"),
                                                                                                                                 OperatorIdentfier = "Safaricom",
                                                                                                                                 OperatorName = "Safaricom",
                                                                                                                                 ProductDisplayText = "100 KES",
                                                                                                                                 ProductId = Guid.Parse("48D96AE5-A829-498B-8C0D-D7107610EBDC"),
                                                                                                                                 ProductType = ProductType.MobileTopup,
                                                                                                                                 Value = 100
                                                                                                                             },
                                                                                                    new ContractProductModel {
                                                                                                                                 ContractId = Guid.Parse("D57DAC9B-4039-4120-B5A8-F7FDF1D3A3C2"),
                                                                                                                                 IsFixedValue = true,
                                                                                                                                 OperatorId = Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967"),
                                                                                                                                 OperatorIdentfier = "Safaricom",
                                                                                                                                 OperatorName = "Safaricom",
                                                                                                                                 ProductDisplayText = "200 KES",
                                                                                                                                 ProductId = Guid.Parse("63821A48-D35E-451D-9E88-C8DCB548E7ED"),
                                                                                                                                 ProductType = ProductType.MobileTopup,
                                                                                                                                 Value = 200
                                                                                                                             },
                                                                                                    new ContractProductModel {
                                                                                                                                 ContractId = Guid.Parse("D57DAC9B-4039-4120-B5A8-F7FDF1D3A3C2"),
                                                                                                                                 IsFixedValue = false,
                                                                                                                                 OperatorId = Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967"),
                                                                                                                                 OperatorIdentfier = "Safaricom",
                                                                                                                                 OperatorName = "Safaricom",
                                                                                                                                 ProductDisplayText = "Custom",
                                                                                                                                 ProductId = Guid.Parse("63821A48-D35E-451D-9E88-C8DCB548E7ED"),
                                                                                                                                 ProductType = ProductType.MobileTopup
                                                                                                                             },
                                                                                                    new ContractProductModel {
                                                                                                                                 ContractId = Guid.Parse("D57DAC9B-4039-4120-B5A8-F7FDF1D3A3C2"),
                                                                                                                                 IsFixedValue = false,
                                                                                                                                 OperatorId = Guid.Parse("1CA5F4C7-34EC-425B-BB53-7EEDF48D9968"),
                                                                                                                                 OperatorIdentfier = "Safaricom1",
                                                                                                                                 OperatorName = "Safaricom1",
                                                                                                                                 ProductDisplayText = "Custom",
                                                                                                                                 ProductId = Guid.Parse("63821A48-D35E-451D-9E88-C8DCB548E7ED"),
                                                                                                                                 ProductType = ProductType.MobileTopup
                                                                                                                             },
                                                                                                    new ContractProductModel {
                                                                                                                                 ContractId = Guid.Parse("0615E7F6-2749-4507-8588-E019E8110C95"),
                                                                                                                                 IsFixedValue = false,
                                                                                                                                 OperatorId = Guid.Parse("C485F21B-EF17-448D-8B8C-E217A07C1863"),
                                                                                                                                 OperatorIdentfier = "PataPawaPostPay",
                                                                                                                                 OperatorName = "Pata Pawa PostPay",
                                                                                                                                 ProductDisplayText = "Pay Bill",
                                                                                                                                 ProductId = Guid.Parse("DE92018C-513E-44B2-B96D-F5B3621C48A2"),
                                                                                                                                 ProductType = ProductType.BillPayment
                                                                                                                             }

                                                                                                });
        }

        public async Task<Result<Decimal>> GetMerchantBalance(CancellationToken cancellationToken) {
            return new SuccessResult<Decimal>(100);
        }

        public async Task<Result<MerchantDetailsModel>> GetMerchantDetails(CancellationToken cancellationToken) {
            return new SuccessResult<MerchantDetailsModel>(new MerchantDetailsModel {
                                                                                        Address = new AddressModel {
                                                                                                      AddressLine1 = "test address line 1",
                                                                                                      AddressLine2 = "test address line 2",
                                                                                                      AddressLine3 = "test address line 3",
                                                                                                      AddressLine4 = "test address line 4",
                                                                                                      PostalCode = "TE57 1NG",
                                                                                                      Region = "Region",
                                                                                                      Town = "Town"
                                                                                                  },
                                                                                        Contact = new ContactModel {
                                                                                                      Name = "Test Contact",
                                                                                                      EmailAddress = "stuart_ferguson1@outlook.com",
                                                                                                      MobileNumber = "123456789"
                                                                                                  },
                                                                                        LastStatementDate = new DateTime(2022, 8, 1),
                                                                                        NextStatementDate = new DateTime(2022, 9, 1),
                                                                                        MerchantName = "Dummy Merchant",
                                                                                        SettlementSchedule = "Monthly",
                                                                                        AvailableBalance = 100,
                                                                                        Balance = 99
                                                                                    });
        }
    }

    public class TrainingTransactionService : ITransactionService
    {
        public async Task<Result<PerformLogonResponseModel>> PerformLogon(PerformLogonRequestModel model,
                                                                          CancellationToken cancellationToken) {
            return new SuccessResult<PerformLogonResponseModel>(new PerformLogonResponseModel {
                                                                                                  EstateId = Guid.Parse("D7E52254-E0BE-436A-9A34-CC291DA0D66A"),
                                                                                                  MerchantId = Guid.Parse("DD034A3B-D8EE-45A4-A29F-8774751CEE76"),
                                                                                                  IsSuccessful = true,
                                                                                                  ResponseMessage = "SUCCESS",
                                                                                                  RequireApplicationUpdate = false
                                                                                              });
        }

        public async Task<Result<SaleTransactionResponseMessage>> PerformMobileTopup(PerformMobileTopupRequestModel model,
                                                                                     CancellationToken cancellationToken) {
            SaleTransactionResponseMessage responseMessage = new SaleTransactionResponseMessage {
                                                                                                    ResponseCode = "0000",
                                                                                                    ResponseMessage = "SUCCESS"
                                                                                                };


            return new SuccessResult<SaleTransactionResponseMessage>(responseMessage);
        }

        public async Task<Result<ReconciliationResponseMessage>> PerformReconciliation(PerformReconciliationRequestModel model,
                                                                                       CancellationToken cancellationToken) {
            return new SuccessResult<ReconciliationResponseMessage>(new ReconciliationResponseMessage {
                                                                                                          ResponseMessage = "SUCCESS",
                                                                                                          ResponseCode = "0000"
                                                                                                      });
        }

        public async Task<Result<SaleTransactionResponseMessage>> PerformVoucherIssue(PerformVoucherIssueRequestModel model,
                                                                                      CancellationToken cancellationToken) {
            SaleTransactionResponseMessage responseMessage = new SaleTransactionResponseMessage
                                                             {
                                                                 ResponseCode = "0000",
                                                                 ResponseMessage = "SUCCESS"
                                                             };


            return new SuccessResult<SaleTransactionResponseMessage>(responseMessage);
        }

        public async Task<Result<PerformBillPaymentGetAccountResponseModel>> PerformBillPaymentGetAccount(PerformBillPaymentGetAccountModel model,
                                                                                                          CancellationToken cancellationToken) {
            PerformBillPaymentGetAccountResponseModel responseMessage = new PerformBillPaymentGetAccountResponseModel
            {
                BillDetails = new BillDetails
                {
                    AccountName = "Mr Test Customer",
                    AccountNumber = model.CustomerAccountNumber,
                    Balance = "100.00",
                    DueDate = DateTime.Now.AddDays(3).ToString("dd-MM-yyyy"),

                },
                IsSuccessful = true
            };


            return new SuccessResult<PerformBillPaymentGetAccountResponseModel>(responseMessage);
        }

        public async Task<Result<SaleTransactionResponseMessage>> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model,
                                                                                                CancellationToken cancellationToken) {
            SaleTransactionResponseMessage responseMessage = new SaleTransactionResponseMessage
                                                             {
                                                                 ResponseCode = "0000",
                                                                 ResponseMessage = "SUCCESS"
                                                             };


            return new SuccessResult<SaleTransactionResponseMessage>(responseMessage);
        }
    }
}
