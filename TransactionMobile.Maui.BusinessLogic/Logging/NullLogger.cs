namespace Shared.Logger
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ILogger" />
    public class NullLogger : ILogger
    {
        public static NullLogger Instance
        {
            get
            {
                return new NullLogger();
            }
        }

        public Boolean IsInitialised { get; set; }

        public void LogCritical(Exception exception) {
            
        }

        public void LogDebug(String message) {
            
        }

        public void LogError(Exception exception) {
            
        }

        public void LogInformation(String message) {
            
        }

        public void LogTrace(String message) {
            
        }

        public void LogWarning(String message) {
            
        }
    }
}