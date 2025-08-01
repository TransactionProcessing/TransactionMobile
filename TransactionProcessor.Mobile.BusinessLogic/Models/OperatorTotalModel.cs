﻿using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class OperatorTotalModel
{
    public Guid ContractId { get; set; }
    public Guid OperatorId { get; set; }
    public Decimal TransactionValue { get; set; }
    public Int32 TransactionCount { get; set; }
}