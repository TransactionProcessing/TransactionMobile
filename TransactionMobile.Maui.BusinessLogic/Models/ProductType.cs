namespace TransactionMobile.Maui.BusinessLogic.Models;

public enum ProductType
{
    NotSet = 0,
    MobileTopup,
    MobileWallet,
    BillPayment,
    Voucher
}

public enum ProductSubType
{
    NotSet = 0,
    MobileTopup,
    MobileWallet,
    BillPaymentPostPay,
    BillPaymentPrePay,
    Voucher
}