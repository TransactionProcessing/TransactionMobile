namespace TransactionMobile.Maui.BusinessLogic.Models
{
    public class ContractProductModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public Guid ContractId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fixed value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is fixed value; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsFixedValue { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the operator identfier.
        /// </summary>
        /// <value>
        /// The operator identfier.
        /// </value>
        public String OperatorIdentfier { get; set; }

        /// <summary>
        /// Gets or sets the name of the operator.
        /// </summary>
        /// <value>
        /// The name of the operator.
        /// </value>
        public String OperatorName { get; set; }

        /// <summary>
        /// Gets or sets the product display text.
        /// </summary>
        /// <value>
        /// The product display text.
        /// </value>
        public String ProductDisplayText { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the type of the product.
        /// </summary>
        /// <value>
        /// The type of the product.
        /// </value>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public Decimal Value { get; set; }

        #endregion
    }

    public class MerchantDetailsModel
    {
        public Decimal Balance { get; set; }
        public Decimal AvailableBalance { get; set; }
        public String MerchantName { get; set; }
        public DateTime NextStatementDate { get; set; }
        public DateTime LastStatementDate { get; set; }
        public String SettlementSchedule { get; set; }
        public AddressModel Address { get; set; }
        public ContactModel Contact { get; set; }
    }

    public class AddressModel
    {
        public String AddressLine1 { get; set; }
        public String AddressLine2 { get; set; }
        public String AddressLine3 { get; set; }
        public String AddressLine4 { get; set; }
        public String PostalCode { get; set; }
        public String Region { get; set; }
        public String Town { get; set; }
    }

    public class ContactModel
    {
        public String EmailAddress { get; set; }
        public String Name { get; set; }
        public String MobileNumber { get; set; }
    }
}