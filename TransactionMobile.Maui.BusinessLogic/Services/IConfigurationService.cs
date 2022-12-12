using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services;

using Models;
using RequestHandlers;

public interface IConfigurationService
{
    /// <summary>
    /// Gets the configuration.
    /// </summary>
    /// <param name="deviceIdentifier">The device identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<Result<Configuration>> GetConfiguration(String deviceIdentifier,
                                                 CancellationToken cancellationToken);

    /// <summary>
    /// Posts the diagnostic logs.
    /// </summary>
    /// <param name="deviceIdentifier">The device identifier.</param>
    /// <param name="logMessages">The log messages.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task PostDiagnosticLogs(String deviceIdentifier,
                            List<LogMessage> logMessages,
                            CancellationToken cancellationToken);
}