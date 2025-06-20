using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
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