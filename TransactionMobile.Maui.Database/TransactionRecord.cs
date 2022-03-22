namespace TransactionMobile.Maui.Database
{
    using SQLite;

    public class TransactionRecord
    {
        public String ApplicationVersion { get; set; }
        
        public Guid ContractId { get; set; }

        public String CustomerAccountNumber { get; set; }

        public String CustomerEmailAddress { get; set; }

        public String DeviceIdentifier { get; set; }

        public String OperatorIdentifier { get; set; }

        public Guid ProductId { get; set; }

        public Decimal Amount { get; set; }

        public DateTime TransactionDateTime { get; set; }

        [PrimaryKey, AutoIncrement]
        public Int32 TransactionNumber { get; set; }

        public String RecipientMobileNumber { get; set; }
        public String RecipientEmailAddress { get; set; }

        public Int32 TransactionType { get; set; }

        public Guid EstateId { get; set; }
        public Guid MerchantId { get; set; }
        public Boolean IsSuccessful { get; set; }

        public String ResponseMessage { get; set; }

    }
}
