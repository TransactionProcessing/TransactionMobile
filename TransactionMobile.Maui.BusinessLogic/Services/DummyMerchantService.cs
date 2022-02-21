namespace TransactionMobile.Maui.BusinessLogic.Services;

using Models;

public class DummyMerchantService : IMerchantService
{
    public async Task<List<ContractProductModel>> GetContractProducts(String accessToken,
                                                                      Guid estateId,
                                                                      Guid merchantId,
                                                                      CancellationToken cancellationToken)
    {
        return new List<ContractProductModel>
               {
                   new ContractProductModel
                   {
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
                   new ContractProductModel
                   {
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
                   new ContractProductModel
                   {
                       ContractId = Guid.Parse("21DA6AC5-70E6-478E-A5E9-74C9B27F5725"),
                       IsFixedValue = false,
                       OperatorId = Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967"),
                       OperatorIdentfier = "Safaricom",
                       OperatorName = "Safaricom",
                       ProductDisplayText = "Custom",
                       ProductId = Guid.Parse("268CBAF5-95E0-4D4C-9725-3BB2B76E4273"),
                       ProductType = ProductType.Voucher
                   },
                   new ContractProductModel
                   {
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
                   new ContractProductModel
                   {
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
                   new ContractProductModel
                   {
                       ContractId = Guid.Parse("D57DAC9B-4039-4120-B5A8-F7FDF1D3A3C2"),
                       IsFixedValue = false,
                       OperatorId = Guid.Parse("2CA5F4C7-34EC-425B-BB53-7EEDF48D9967"),
                       OperatorIdentfier = "Safaricom",
                       OperatorName = "Safaricom",
                       ProductDisplayText = "Custom",
                       ProductId = Guid.Parse("63821A48-D35E-451D-9E88-C8DCB548E7ED"),
                       ProductType = ProductType.MobileTopup
                   },
                   new ContractProductModel
                   {
                       ContractId = Guid.Parse("D57DAC9B-4039-4120-B5A8-F7FDF1D3A3C2"),
                       IsFixedValue = false,
                       OperatorId = Guid.Parse("1CA5F4C7-34EC-425B-BB53-7EEDF48D9968"),
                       OperatorIdentfier = "Safaricom1",
                       OperatorName = "Safaricom1",
                       ProductDisplayText = "Custom",
                       ProductId = Guid.Parse("63821A48-D35E-451D-9E88-C8DCB548E7ED"),
                       ProductType = ProductType.MobileTopup
                   }
               };
    }

    public async Task<Decimal> GetMerchantBalance(String accessToken,
                                                  Guid estateId,
                                                  Guid merchantId,
                                                  CancellationToken cancellationToken)
    {
        return 100;
    }
}