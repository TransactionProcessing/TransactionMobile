namespace TransactionMobile.Maui.BusinessLogic.Models;

public enum ProductType
{
    /// <summary>
    /// The not set
    /// </summary>
    NotSet = 0,

    /// <summary>
    /// The mobile topup
    /// </summary>
    MobileTopup,

    /// <summary>
    /// The mobile wallet
    /// </summary>
    MobileWallet,

    /// <summary>
    /// The bill payment
    /// </summary>
    BillPayment,

    /// <summary>
    /// The voucher
    /// </summary>
    Voucher
}