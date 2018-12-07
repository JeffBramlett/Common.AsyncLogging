using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Interface for a Logger with Fluent methods
    /// </summary>
    public interface ILogger : IDisposable
    {
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
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="customPairs">Custom key-value pair enumeration</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        void LogDebug(Type type, 
            string message = "", 
            TimeSpan? elaspedTime = null, 
            Exception ex = null, 
            IEnumerable<CustomPair> customPairs = null,
            [CallerFilePath] string filepath = "", 
            [CallerMemberName] string caller = "", 
            [CallerLineNumber] int lineNo = 0);

        /// <summary>
        /// Log an info message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="customPairs">Custom key-value pair enumeration</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        void LogInfo(Type type, 
            string message = "", 
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            IEnumerable<CustomPair> customPairs = null,
            [CallerFilePath] string filepath = "",
            [CallerMemberName] string caller = "",
            [CallerLineNumber] int lineNo = 0);

        /// <summary>
        /// Log a Warning message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="customPairs">Custom key-value pair enumeration</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        void LogWarning(Type type, 
            string message = "", 
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            IEnumerable<CustomPair> customPairs = null,
            [CallerFilePath] string filepath = "",
            [CallerMemberName] string caller = "",
            [CallerLineNumber] int lineNo = 0);

        /// <summary>
        /// Log an Error message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="customPairs">Custom key-value pair enumeration</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        void LogError(Type type, 
            string message = "", 
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            IEnumerable<CustomPair> customPairs = null,
            [CallerFilePath] string filepath = "",
            [CallerMemberName] string caller = "",
            [CallerLineNumber] int lineNo = 0);

        /// <summary>
        /// Log a Fatal message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="customPairs">Custom key-value pair enumeration</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        void LogFatal(Type type, 
            string message = "", 
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            IEnumerable<CustomPair> customPairs = null,
            [CallerFilePath] string filepath = "",
            [CallerMemberName] string caller = "",
            [CallerLineNumber] int lineNo = 0);
    }
}
