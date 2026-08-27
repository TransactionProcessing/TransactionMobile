using System.Security.Cryptography;
using System.Text;

namespace TransactionProcessor.Mobile.UiTestBackend;

public sealed record BackendSeed
{
    public List<ClientSeed> Clients { get; init; } = [];
    public List<UserSeed> Users { get; init; } = [];
    public List<EstateSeed> Estates { get; init; } = [];
    public List<OperatorSeed> Operators { get; init; } = [];
    public List<ContractSeed> Contracts { get; init; } = [];
    public List<ProductSeed> Products { get; init; } = [];
    public List<MerchantSeed> Merchants { get; init; } = [];
    public List<MerchantOperatorSeed> MerchantOperators { get; init; } = [];
    public List<MerchantContractSeed> MerchantContracts { get; init; } = [];
    public List<DeviceSeed> Devices { get; init; } = [];
    public List<BillSeed> Bills { get; init; } = [];
    public List<MeterSeed> Meters { get; init; } = [];
    public List<DepositSeed> Deposits { get; init; } = [];
    public List<ReceiptSeed> PreseededReceipts { get; init; } = [];
    public List<ReportTransactionSeed> ReportTransactions { get; init; } = [];

    public BackendSeed CloneSeed()
    {
        return this with
        {
            Clients = [.. this.Clients],
            Users = [.. this.Users],
            Estates = [.. this.Estates],
            Operators = [.. this.Operators],
            Contracts = [.. this.Contracts],
            Products = [.. this.Products],
            Merchants = [.. this.Merchants],
            MerchantOperators = [.. this.MerchantOperators],
            MerchantContracts = [.. this.MerchantContracts],
            Devices = [.. this.Devices],
            Bills = [.. this.Bills],
            Meters = [.. this.Meters],
            Deposits = [.. this.Deposits],
            PreseededReceipts = [.. this.PreseededReceipts],
            ReportTransactions = [.. this.ReportTransactions]
        };
    }
}

public sealed record ClientSeed
{
    public string ClientId { get; init; } = string.Empty;
    public string ClientName { get; init; } = string.Empty;
    public string Secret { get; init; } = string.Empty;
    public List<string> GrantTypes { get; init; } = [];
    public bool IsAppClient { get; init; }
}

public sealed record UserSeed
{
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? GivenName { get; init; }
    public string? FamilyName { get; init; }
}

public sealed record EstateSeed
{
    public string EstateName { get; init; } = string.Empty;
    public string EstateReference { get; init; } = string.Empty;
}

public sealed record OperatorSeed
{
    public string EstateName { get; init; } = string.Empty;
    public string OperatorName { get; init; } = string.Empty;
    public bool RequireCustomMerchantNumber { get; init; }
    public bool RequireCustomTerminalNumber { get; init; }
}

public sealed record ContractSeed
{
    public string EstateName { get; init; } = string.Empty;
    public string OperatorName { get; init; } = string.Empty;
    public string ContractDescription { get; init; } = string.Empty;
}

public sealed record ProductSeed
{
    public string EstateName { get; init; } = string.Empty;
    public string OperatorName { get; init; } = string.Empty;
    public string ContractDescription { get; init; } = string.Empty;
    public string ProductName { get; init; } = string.Empty;
    public string DisplayText { get; init; } = string.Empty;
    public decimal? Value { get; init; }
    public string ProductType { get; init; } = string.Empty;
    public string? CalculationType { get; init; }
    public string? FeeDescription { get; init; }
    public decimal? FeeValue { get; init; }
}

public sealed record MerchantSeed
{
    public string EstateName { get; init; } = string.Empty;
    public string MerchantName { get; init; } = string.Empty;
    public string AddressLine1 { get; init; } = string.Empty;
    public string AddressLine2 { get; init; } = string.Empty;
    public string AddressLine3 { get; init; } = string.Empty;
    public string AddressLine4 { get; init; } = string.Empty;
    public string Town { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string ContactName { get; init; } = string.Empty;
    public string ContactEmailAddress { get; init; } = string.Empty;
    public string ContactPhoneNumber { get; init; } = string.Empty;
    public string? SettlementSchedule { get; init; }
}

public sealed record MerchantOperatorSeed
{
    public string EstateName { get; init; } = string.Empty;
    public string MerchantName { get; init; } = string.Empty;
    public string OperatorName { get; init; } = string.Empty;
    public string MerchantNumber { get; init; } = string.Empty;
    public string TerminalNumber { get; init; } = string.Empty;
}

public sealed record MerchantContractSeed
{
    public string EstateName { get; init; } = string.Empty;
    public string MerchantName { get; init; } = string.Empty;
    public string ContractDescription { get; init; } = string.Empty;
}

public sealed record DeviceSeed
{
    public string EstateName { get; init; } = string.Empty;
    public string MerchantName { get; init; } = string.Empty;
    public string? DeviceIdentifier { get; init; }
}

public sealed record BillSeed
{
    public string AccountNumber { get; init; } = string.Empty;
    public string AccountName { get; init; } = string.Empty;
    public DateTime DueDate { get; init; } = DateTime.UtcNow.Date.AddDays(3);
    public decimal Amount { get; init; }
}

public sealed record MeterSeed
{
    public string MeterNumber { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
}

public sealed record DepositSeed
{
    public string EstateName { get; init; } = string.Empty;
    public string MerchantName { get; init; } = string.Empty;
    public string Reference { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTime DateTime { get; init; } = DateTime.UtcNow;
}

public enum RequestKind
{
    NotSet,
    Logon,
    MerchantDeposit,
    BillPaymentGetAccount,
    BillPaymentGetMeter,
    BillPaymentMakePayment,
    Voucher,
    MobileTopup

}

public sealed record ReceiptSeed
{
    public string Reference { get; init; } = string.Empty;
    public string ReceiptReference { get; init; } = string.Empty;
    public RequestKind TransactionType { get; init; } = RequestKind.NotSet;
    public string Product { get; init; } = string.Empty;
    public string Operator { get; init; } = string.Empty;
    public string Status { get; init; } = "Success";
    public decimal Amount { get; init; }
    public DateTime TransactionDateTime { get; init; } = DateTime.UtcNow;
}

public sealed record ReportTransactionSeed
{
    public string Reference { get; init; } = string.Empty;
    public RequestKind TransactionType { get; init; } = RequestKind.NotSet;
    public string Product { get; init; } = string.Empty;
    public string Operator { get; init; } = string.Empty;
    public string Status { get; init; } = "Success";
    public decimal Amount { get; init; }
    public DateTime TransactionDateTime { get; init; } = DateTime.UtcNow;
    public string ReceiptReference { get; init; } = string.Empty;
}

internal static class SeedIdGenerator
{
    public static Guid CreateGuid(string value)
    {
        byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(value));
        Span<byte> guidBytes = stackalloc byte[16];
        hash.AsSpan(0, 16).CopyTo(guidBytes);
        return new Guid(guidBytes);
    }

    public static int CreateInt(string value)
    {
        byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(value.ToUpperInvariant()));
        int valueHash = BitConverter.ToInt32(hash, 0);
        return Math.Abs(valueHash == int.MinValue ? int.MaxValue : valueHash);
    }
}
