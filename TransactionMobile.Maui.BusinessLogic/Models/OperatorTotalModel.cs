﻿using System.Diagnostics.CodeAnalysis;

namespace TransactionMobile.Maui.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class OperatorTotalModel
{
    public Guid ContractId { get; set; }
    public String OperatorIdentifier { get; set; }
    public Decimal TransactionValue { get; set; }
    public Int32 TransactionCount { get; set; }
}