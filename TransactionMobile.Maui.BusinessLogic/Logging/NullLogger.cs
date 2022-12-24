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
        
        public void Log<TState>(LogLevel logLevel,
                                EventId eventId,
                                TState state,
                                Exception exception,
                                Func<TState, Exception, String> formatter) {
            
        }

        public Boolean IsEnabled(LogLevel logLevel) {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull {
            return null;
        }
    }
}