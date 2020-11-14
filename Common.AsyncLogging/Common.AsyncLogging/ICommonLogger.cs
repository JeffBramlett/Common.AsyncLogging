using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Interface for a Logger with Fluent methods
    /// </summary>
    public interface ICommonLogger : IDisposable
    {
        /// <summary>
        /// Bitwise enum to limit logging by log levels
        /// </summary>
        LogLevels AllowedLogLevels { get; set; }

        /// <summary>
        /// Sets the write action for Log entries (the logger asynchrously spools to this action)
        /// </summary>
        /// <param name="writeAction">the delegate that writes the LogEntry, or, passes to other logger (i.e. Log4Net)</param>
        void SetWriteAction(Action<LogEntry> writeAction);

        /// <summary>
        /// Log a Debug message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="correlationId">User supplied correlation id</param>
        /// <param name="extendedProperites">Custom key-value pair enumeration</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        void LogDebug(Type type, 
            string message = "",
            string correlationId = "",
            IList<KeyValuePair<string, object>> extendedProperites = null,
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            [CallerFilePath] string filepath = "", 
            [CallerMemberName] string caller = "", 
            [CallerLineNumber] int lineNo = 0);

        /// <summary>
        /// Log an info message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="correlationId">The user supplied correlationId</param>
        /// <param name="extendedProperties">Custom key-value pair enumeration</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        void LogInfo(Type type, 
            string message = "",
            string correlationId = "", 
            IList<KeyValuePair<string, object>> extendedProperties = null,
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            [CallerFilePath] string filepath = "",
            [CallerMemberName] string caller = "",
            [CallerLineNumber] int lineNo = 0);

        /// <summary>
        /// Log a Warning message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="correlationId">the user supplied correlation id</param>
        /// <param name="extendedProperties">Custom key-value pair enumeration</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        void LogWarning(Type type, 
            string message = "",
            string correlationId = "",
            IList<KeyValuePair<string, object>> extendedProperties = null,
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            [CallerFilePath] string filepath = "",
            [CallerMemberName] string caller = "",
            [CallerLineNumber] int lineNo = 0);

        /// <summary>
        /// Log an Error message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="correlationId">the user supplied correlation id</param>
        /// <param name="extendedProperties">Custom key-value pair enumeration</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        void LogError(Type type, 
            string message = "",
            string correlationId = "", 
            IList<KeyValuePair<string, object>> extendedProperties = null,
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            [CallerFilePath] string filepath = "",
            [CallerMemberName] string caller = "",
            [CallerLineNumber] int lineNo = 0);

        /// <summary>
        /// Log a Fatal message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="correlationId">the correlation id</param>
        /// <param name="extendedProperties">Custom key-value pair enumeration</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        void LogFatal(Type type, 
            string message = "",
            string correlationId = "", 
            IList<KeyValuePair<string, object>> extendedProperties = null,
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            [CallerFilePath] string filepath = "",
            [CallerMemberName] string caller = "",
            [CallerLineNumber] int lineNo = 0);
    }
}
