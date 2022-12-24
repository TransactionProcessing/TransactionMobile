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
}

public interface ILoogingService
{
    
    Task PostDiagnosticLogs(String deviceIdentifier,
                            List<LogMessage> logMessages,
                            CancellationToken cancellationToken);
}