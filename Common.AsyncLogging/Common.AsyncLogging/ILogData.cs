using System;
using System.Collections.Generic;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Contract for a LogEntry
    /// </summary>
    public interface ILogData
    {
        /// <summary>
        /// The application data for this log entry
        /// </summary>
        ApplicationData Application { get; set; }

        /// <summary>
        /// User assigned correlation id
        /// </summary>
        string CorrelationId { get; set; }

        /// <summary>
        /// Custom key/value pair Collection in the logEntry
        /// </summary>
        IDictionary<string, object> ExtendedProperties { get; set; }

        int ThreadId { get; set; }

        string ThreadName { get; set; }

        /// <summary>
        /// User input elasped milliseconds of the elasped time
        /// </summary>
        double ElaspedMilliseconds { get; }

        /// <summary>
        /// User input elasped time to report in this log entry
        /// </summary>
        string ElaspedTime { get; set; }

        /// <summary>
        /// (User input) the exception to include with this log
        /// </summary>
        Exception Exception { get; set; }

        /// <summary>
        /// The log level for this log entry
        /// </summary>
        LogLevels Level { get; set; }

        /// <summary>
        /// The log message
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// The caller metadata for this log entry
        /// </summary>
        ModuleData Module { get; set; }

        /// <summary>
        /// When this log was entered
        /// </summary>
        DateTimeOffset Timestamp { get; set; }

        string ToKeyValuePairs();
    }
}