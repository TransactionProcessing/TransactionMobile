namespace TransactionMobile.Maui.BusinessLogic.Models
{
    public class ContractProductModel
    {
        #region Properties

        public Guid ContractId { get; set; }

        public Boolean IsFixedValue { get; set; }

        public Guid OperatorId { get; set; }

        public String OperatorIdentfier { get; set; }

        public String OperatorName { get; set; }

        public String ProductDisplayText { get; set; }
        public Guid ProductId { get; set; }

        public ProductType ProductType { get; set; }
        public ProductSubType ProductSubType { get; set; }
        public Decimal Value { get; set; }

        #endregion
    }
}