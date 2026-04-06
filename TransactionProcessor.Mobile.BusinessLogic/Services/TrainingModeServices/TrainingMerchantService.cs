using System.Diagnostics.CodeAnalysis;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Services.TrainingModeServices;

[ExcludeFromCodeCoverage]
public class TrainingMerchantService : IMerchantService
{
    private record Contract(Guid ContractId, Guid OperatorId, string OperatorIdentfier, string OperatorName);

    private readonly Contract Hospital1Contract = new(Guid.Parse("21DA6AC5-70E6-478E-A5E9-74C9B27F5725"), Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967"), "Hospital 1 Contract", "Hospital 1 Contract");
    private readonly Contract SafaricomContract = new(Guid.Parse("D57DAC9B-4039-4120-B5A8-F7FDF1D3A3C2"), Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967"), "Safaricom", "Safaricom");
    private readonly Contract PataPawaPostPayContract = new(Guid.Parse("0615E7F6-2749-4507-8588-E019E8110C95"), Guid.Parse("C485F21B-EF17-448D-8B8C-E217A07C1863"), "PataPawaPostPay", "PataPawaPostPay");
    private readonly Contract PataPawaPrePayContract = new(Guid.Parse("7AAFFE8D-4EA2-485F-8600-D4D29A0043F0"), Guid.Parse("C485F21B-EF17-448D-8B8C-E217A07C1863"), "PataPawaPrePay", "PataPawaPrePay");

    private static ContractProductModel CreateContractProductModel(Contract contract, string productDisplayText, Guid productId, ProductType productType, decimal? value = null) {
        ContractProductModel product = new() {
            ContractId = contract.ContractId,
            IsFixedValue = value.HasValue,
            OperatorId = contract.OperatorId,
            OperatorIdentfier = contract.OperatorIdentfier,
            OperatorName = contract.OperatorName,
            ProductDisplayText = productDisplayText,
            ProductId = productId,
            ProductType = productType,
        };
        if (value.HasValue) {
            product.Value = value.Value;
        }

        return product;
    }

    private List<ContractProductModel> GetVoucherProducts() {
        List<ContractProductModel> products = new();

        products.Add(CreateContractProductModel(Hospital1Contract, "10 KES", Guid.Parse("CBF55D95-306A-4E85-B367-24FA442998F6"), ProductType.Voucher, 10));
        products.Add(CreateContractProductModel(Hospital1Contract, "20 KES", Guid.Parse("F5F9B63F-9F68-45E8-960B-CE0FC15ED672"), ProductType.Voucher, 20));
        products.Add(CreateContractProductModel(Hospital1Contract, "Custom", Guid.Parse("268CBAF5-95E0-4D4C-9725-3BB2B76E4273"), ProductType.Voucher));

        return products;
    }

    private List<ContractProductModel> GetTopupProducts() {
        List<ContractProductModel> products = new();
        products.Add(CreateContractProductModel(SafaricomContract, "100 KES", Guid.Parse("48D96AE5-A829-498B-8C0D-D7107610EBDC"), ProductType.MobileTopup, 100));
        products.Add(CreateContractProductModel(SafaricomContract, "200 KES", Guid.Parse("63821A48-D35E-451D-9E88-C8DCB548E7ED"), ProductType.MobileTopup, 200));
        products.Add(CreateContractProductModel(SafaricomContract, "Custom", Guid.Parse("63821A48-D35E-451D-9E88-C8DCB548E7ED"), ProductType.MobileTopup));

        return products;
    }

    private List<ContractProductModel> GetBillPaymentProducts() {
        List<ContractProductModel> products = new();

        products.Add(CreateContractProductModel(PataPawaPostPayContract, "Bill Pay (Post)", Guid.Parse("DE92018C-513E-44B2-B96D-F5B3621C48A2"), ProductType.BillPayment));
        products.Add(CreateContractProductModel(PataPawaPrePayContract, "Bill Pay (Pre)", Guid.Parse("A7472650-4420-43BF-8132-3A094EB084FE"), ProductType.BillPayment));

        return products;
    }


    public async Task<Result<List<ContractProductModel>>> GetContractProducts(CancellationToken cancellationToken) {
        List<ContractProductModel> products = new();
        
        products.AddRange(GetVoucherProducts());
        products.AddRange(GetTopupProducts());
        products.AddRange(GetBillPaymentProducts());

        return Result.Success(products);
    }

    public async Task<Result<Decimal>> GetMerchantBalance(CancellationToken cancellationToken) {
        return Result.Success(100.00m);
    }

    public async Task<Result<MerchantDetailsModel>> GetMerchantDetails(CancellationToken cancellationToken) {
        return Result.Success(new MerchantDetailsModel {
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