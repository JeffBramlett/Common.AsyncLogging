using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Bitwise Enumeration of logging levels
    /// </summary>
    [Flags]
    public enum LogLevels: short
    {
        Debug = 1,
        Info = 2,
        Warning = 4,
        Error = 8,
        Fatal = 16
    }

    /// <summary>
    /// Data class of a Log Entry
    /// </summary>
    public class LogEntry : ILogEntry
    {
        #region fields
        Application _appMetadata;
        private string _levelName;
        private LogLevels _level;
        private IDictionary<string, object> _extendedProperties;
        private TimeSpan _elasped;
        private bool _elaspedChanged;
        private double _elaspedMilliseconds;
        #endregion

        #region Properties
        /// <summary>
        /// The log level for this log entry
        /// </summary>
        public LogLevels Level 
        {
            get { return _level; }
            set
            {
                _level = value;
                _levelName = value.ToString(); }
        }

        /// <summary>
        /// User assigned correlation id
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// The log message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// (User input) the exception to include with this log
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// User input for the time elasped
        /// </summary>
        public string ElaspedTime
        {
            get { return _elasped.ToString(); }
            set
            {
                TimeSpan ts;
                if (TimeSpan.TryParse(value, out ts))
                {
                    _elasped = ts;
                    _elaspedMilliseconds = ts.TotalMilliseconds;
                    _elaspedChanged = true;
                }
            }
        }

        /// <summary>
        /// User input elasped milliseconds of the elasped time
        /// </summary>
        public double ElaspedMilliseconds
        {
            get { return _elaspedMilliseconds; }
        }

        /// <summary>
        /// When this log was entered
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Current thread id
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// Current thread name
        /// </summary>
        public string ThreadName { get; set; }

        /// <summary>
        /// Custom key/value pair Collection in the logEntry
        /// </summary>
        public IDictionary<string, object> ExtendedProperties
        {
            get
            {
                _extendedProperties = _extendedProperties ?? new Dictionary<string, object>();
                return _extendedProperties;
            }
            set { _extendedProperties = value; }
        }

        /// <summary>
        /// The application data for this log entry
        /// </summary>
        public Application Application
        {
            get
            {
                if (string.IsNullOrEmpty(_appMetadata.ApplicationName))
                {
                    _appMetadata = new Application();
                }

                return _appMetadata;
            }
            set
            {
                _appMetadata = value;
            }
        }

        /// <summary>
        /// The caller metadata for this log entry
        /// </summary>
        public Module Module { get; set; }
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        public LogEntry()
        {
            Timestamp = DateTimeOffset.Now;
        }
        #endregion

        #region Publics
        public virtual string ToKeyValuePairs()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Timestamp={Timestamp}, ");
            sb.Append($"Level={Level}, ");

            if(!string.IsNullOrEmpty(Message))
                sb.Append($"Message={Message}, ");

            if(!string.IsNullOrEmpty(CorrelationId))
                sb.Append($"CorrelationId={CorrelationId}, ");

             if (_elaspedChanged) 
            {
                sb.Append($"ElaspedTime={ElaspedTime}, ");
                sb.Append($"ElaspedMilliseconds={ElaspedMilliseconds}, ");
            }

            if(_extendedProperties != null)
            {
                foreach(var pair in ExtendedProperties)
                {
                    sb.Append($"{pair.Key}={pair.Value}, ");
                }
            }

            if(Exception != null)
            {
                sb.Append($"{Exception.GetType()}={Exception.Message} {Exception.StackTrace}, ");
            }

            sb.Append($"ApplicationName={Application.ApplicationName}, ");
            sb.Append($"ApplicationVersion={Application.Version}, ");
            sb.Append($"ApplicationDomain={Application.ApplicationDomain}, ");
            sb.Append($"MachineName={Application.MachineName}, ");
            sb.Append($"OS={Application.OS}, ");
            sb.Append($"ProcessId={Application.ProcessId}, ");
            sb.Append($"ProcessName={Application.ProcessName}, ");

            sb.Append($"ModuleName={Module.ModuleName}, ");
            sb.Append($"CallerFile={Module.CallerFile}, ");
            sb.Append($"CallerMethod={Module.CallerMethod}, ");
            sb.Append($"LineNo={Module.LineNo}");

            return sb.ToString();
        }
        #endregion
    }
}
