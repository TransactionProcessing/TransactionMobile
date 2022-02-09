namespace TransactionMobile.Maui.BusinessLogic.Models
{
    public class ContractOperatorModel
    {
        #region Properties

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

        #endregion
    }

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
}