﻿using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models
{
    [ExcludeFromCodeCoverage]
    public class PerformMobileTopupRequestModel
    {
        // TODO: should we have a base transaction request model ?

        #region Properties

        public String ApplicationVersion { get; set; }

        public Guid ContractId { get; set; }

        public String CustomerAccountNumber { get; set; }

        public String CustomerEmailAddress { get; set; }

        public String DeviceIdentifier { get; set; }

        public Guid OperatorId { get; set; }

        public Guid ProductId { get; set; }

        public Decimal TopupAmount { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public String TransactionNumber { get; set; }

        #endregion
    }
}