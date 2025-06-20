using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services;

using Common;
using Models;
using RequestHandlers;
using SimpleResults;

public interface IConfigurationService
{
    Task<Result<Configuration>> GetConfiguration(String deviceIdentifier,
                                                 CancellationToken cancellationToken);

    Task PostDiagnosticLogs(String deviceIdentifier,
                            List<LogMessage> logMessages,
                            CancellationToken cancellationToken);
}