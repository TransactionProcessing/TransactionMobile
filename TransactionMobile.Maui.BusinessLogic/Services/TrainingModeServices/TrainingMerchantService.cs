namespace TransactionMobile.Maui.BusinessLogic.Services.TrainingModeServices;

using Models;
using RequestHandlers;
using System.Diagnostics.CodeAnalysis;
using Common;

[ExcludeFromCodeCoverage]
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
                                                                                                                             ProductType = ProductType.BillPayment,
                                                                                                                             ProductSubType = ProductSubType.BillPaymentPostPay
                                                                                                                         },
                                                                                                new ContractProductModel {
                                                                                                                             ContractId = Guid.Parse("7AAFFE8D-4EA2-485F-8600-D4D29A0043F0"),
                                                                                                                             IsFixedValue = false,
                                                                                                                             OperatorId = Guid.Parse("C485F21B-EF17-448D-8B8C-E217A07C1863"),
                                                                                                                             OperatorIdentfier = "PataPawaPrePay",
                                                                                                                             OperatorName = "Pata Pawa PrePay",
                                                                                                                             ProductDisplayText = "Pay Bill",
                                                                                                                             ProductId = Guid.Parse("A7472650-4420-43BF-8132-3A094EB084FE"),
                                                                                                                             ProductType = ProductType.BillPayment,
                                                                                                                             ProductSubType = ProductSubType.BillPaymentPrePay
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