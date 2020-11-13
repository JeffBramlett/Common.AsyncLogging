using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Enumeration of logging levels
    /// </summary>
    [Flags]
    public enum LogLevels: short
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 4,
        Fatal = 8
    }

    /// <summary>
    /// Data class of a Log Entry
    /// </summary>
    public class LogEntry : ILogEntry
    {
        #region fields
        ApplicationMetaData _appMetadata;
        private string _levelName;
        private LogLevels _level;
        private IList<CustomPair> _customPairs;
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
        /// (optional) the exception to include with this log
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// (optional) the elasped time to report in this log entry
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
        /// The elasped milliseconds of the elasped time
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
        /// Custom key/value pair Collection in the logEntry
        /// </summary>
        public IList<CustomPair> CustomPairs
        {
            get
            {
                _customPairs = _customPairs ?? new List<CustomPair>();
                return _customPairs;
            }
            set { _customPairs = value; }
        }

        /// <summary>
        /// The application data for this log entry
        /// </summary>
        public ApplicationMetaData ApplicationMetadata
        {
            get
            {
                if (_appMetadata == null)
                {
                    _appMetadata = new ApplicationMetaData();
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
        public ModuleMetadata ModuleMetadata { get; set; }
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

            if(_customPairs != null)
            {
                foreach(var pair in CustomPairs)
                {
                    sb.Append($"{pair.Key}={pair.Value}, ");
                }
            }

            if(Exception != null)
            {
                sb.Append($"{Exception.GetType()}={Exception.Message}, ");
            }

            sb.Append($"ApplicationName={ApplicationMetadata.ApplicationName}, ");
            sb.Append($"ApplicationVersion={ApplicationMetadata.Version}, ");
            sb.Append($"ApplicationDomain={ApplicationMetadata.ApplicationDomain}, ");
            sb.Append($"MachineName={ApplicationMetadata.MachineName}, ");
            sb.Append($"OS={ApplicationMetadata.OS}, ");
            sb.Append($"ProcessId={ApplicationMetadata.ProcessId}, ");
            sb.Append($"ProcessName={ApplicationMetadata.ProcessName}, ");

            sb.Append($"ModuleName={ModuleMetadata.ModuleName}, ");
            sb.Append($"CallerFile={ModuleMetadata.CallerFile}, ");
            sb.Append($"CallerMethod={ModuleMetadata.CallerMethod}, ");
            sb.Append($"LineNo={ModuleMetadata.LineNo}");

            return sb.ToString();
        }
        #endregion
    }
}
