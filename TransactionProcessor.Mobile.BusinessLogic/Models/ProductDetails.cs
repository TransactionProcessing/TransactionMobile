using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionProcessor.Mobile.BusinessLogic.Models
{
    public class ProductDetails
    {
        #region Properties

        public Guid ContractId { get; set; }

        public Guid OperatorId { get; set; }

        public Guid ProductId { get; set; }

        #endregion
    }
}
