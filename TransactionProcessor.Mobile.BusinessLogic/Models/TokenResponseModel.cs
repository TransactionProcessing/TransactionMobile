using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionProcessor.Mobile.BusinessLogic.Models
{
    [ExcludeFromCodeCoverage]
    public class TokenResponseModel
    {
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }
        public Int64 ExpiryInMinutes { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Configuration
    {
        public String ClientId { get; set; }

        public String ClientSecret { get; set; }

        public String SecurityServiceUri { get; set; }

        public String TransactionProcessorAclUri { get; set; }

        public String TransactionProcessorUri { get; set; }

        public String EstateReportingUri { get; set; }

        public LogLevel LogLevel { get; set; }

        public Boolean EnableAutoUpdates { get; set; }

        public Boolean ShowDebugMessages { get; set; }
    }
}
