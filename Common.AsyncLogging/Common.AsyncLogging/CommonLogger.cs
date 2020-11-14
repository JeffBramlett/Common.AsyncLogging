using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Singleton or Object of a Logger
    /// </summary>
    public class CommonLogger : GenericSpooler<LogEntry>, ICommonLogger
    {
        #region Private
        private ApplicationMetaData _appMetaData;
        #endregion

        #region Properties
        /// <summary>
        /// Bitwise enum to limit logging by log levels
        /// </summary>
        public LogLevels AllowedLogLevels { get; set; }

        private ApplicationMetaData AppMetaData
        {
            get
            {
                if (_appMetaData == null)
                {
                    _appMetaData = new ApplicationMetaData();
                    try
                    {
                        _appMetaData.MachineName = Environment.MachineName;
                        _appMetaData.OS = Environment.OSVersion.VersionString;

                        _appMetaData.ProcessId = Process.GetCurrentProcess().Id;
                        _appMetaData.ProcessName = Process.GetCurrentProcess().ProcessName;

                        Assembly asm = Assembly.GetEntryAssembly();
                        _appMetaData.ApplicationName = asm.GetName().Name;
                        _appMetaData.ApplicationDomain = AppDomain.CurrentDomain.FriendlyName;
                        _appMetaData.Version = asm.GetName().Version.ToString();

                    }
                    catch 
                    {
                    }
                }
                return _appMetaData;
            }
        }
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Singleton Ctor (must set the write action for log entries)
        /// </summary>
        /// <param name="allowedLogLevels">Bitwise enum to set allowed logging from configuration</param>
        public CommonLogger()
        {
            SetWriteAction(DefaultWriteLogEntry);
            AllowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error;
        }

        public CommonLogger(LogLevels allowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error) 
        {
            SetWriteAction(DefaultWriteLogEntry);
            AllowedLogLevels = allowedLogLevels;
        }

        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="writeAction">write Action for log entries (spooled to this delegate)</param>
        public CommonLogger(Action<LogEntry> writeAction, LogLevels allowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error) :
            base(writeAction)
        {
            AllowedLogLevels = allowedLogLevels;
        }
        #endregion

        #region Singleton
        private static Lazy<CommonLogger> _instance = new Lazy<CommonLogger>();

        public static ICommonLogger Instance
        {
            get { return _instance.Value; }
        }
        #endregion

        #region Publics
        /// <summary>
        /// Sets the write action for Log entries (the logger asynchrously spools to this action)
        /// </summary>
        /// <param name="writeAction">the delegate that writes the LogEntry, or, passes to other logger (i.e. Log4Net)</param>
        public void SetWriteAction(Action<LogEntry> writeAction)
        {
            if (writeAction == null)
                throw new ArgumentNullException(nameof(writeAction));

            SpoolerAction = writeAction;
        }

        /// <summary>
        /// Log a Debug message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="correlationId">a user supplied correlation id</param>
        /// <param name="extendedProperties">Custom key-value pair enumeration</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        public void LogDebug(Type type, 
            string message = "",
            string correlationId = "",
            IList<KeyValuePair<string, object>> extendedProperties = null,
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            [CallerFilePath] string filepath = "", 
            [CallerMemberName] string caller = "", 
            [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Debug))
                return;
            
            var logEntry = new LogEntry()
            {
                ApplicationMetadata = AppMetaData.Clone(),
                Message = message,
                Exception = ex,
                CorrelationId = correlationId,
                Level = LogLevels.Debug,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                ThreadName = Thread.CurrentThread.Name,
                Timestamp = DateTimeOffset.Now,
                ModuleMetadata = new ModuleMetadata()
                {
                    CallerFile = filepath.Substring(filepath.LastIndexOf(Path.DirectorySeparatorChar) + 1),
                    CallerMethod = caller,
                    LineNo = lineNo,
                    ModuleName = type.Name
                }
            };

            if (elaspedTime != null)
                logEntry.ElaspedTime = elaspedTime.Value.ToString();

            if(extendedProperties != null)
            {
                logEntry.ExtendedProperties = new List<KeyValuePair<string, object>>(extendedProperties).ToArray();
            }


            AddItem(logEntry);
        }

        /// <summary>
        /// Log an Error message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="correlationId">a user supplied correlation id</param>
        /// <param name="extendedProperties">Custom key-value pair enumeration</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        public void LogError(Type type, 
            string message = "",
            string correlationId = "",
            IList<KeyValuePair<string, object>> extendedProperties = null,
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            [CallerFilePath] string filepath = "", 
            [CallerMemberName] string caller = "", 
            [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Error))
                return;

            var logEntry = new LogEntry()
            {
                ApplicationMetadata = AppMetaData.Clone(),
                Message = message,
                CorrelationId = correlationId,
                Exception = ex,
                Level = LogLevels.Error,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                ThreadName = Thread.CurrentThread.Name,
                Timestamp = DateTimeOffset.Now,
                ModuleMetadata = new ModuleMetadata()
                {
                    CallerFile = filepath.Substring(filepath.LastIndexOf(Path.DirectorySeparatorChar) + 1),
                    CallerMethod = caller,
                    LineNo = lineNo,
                    ModuleName = type.Name
                }
            };

            if (elaspedTime != null)
                logEntry.ElaspedTime = elaspedTime.Value.ToString();

            if (extendedProperties != null)
            {
                logEntry.ExtendedProperties = new List<KeyValuePair<string, object>>(extendedProperties).ToArray();
            }

            AddItem(logEntry);
        }

        /// <summary>
        /// Log a Fatal message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="correlationId">a user supplied correlation id</param>
        /// <param name="extendedProperties">Custom key-value pair enumeration</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        public void LogFatal(Type type, 
            string message = "",
            string correlationId = "",
            IList<KeyValuePair<string, object>> extendedProperties = null,
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            [CallerFilePath] string filepath = "", 
            [CallerMemberName] string caller = "", 
            [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Fatal))
                return;

            var logEntry = new LogEntry()
            {
                ApplicationMetadata = AppMetaData.Clone(),
                Message = message,
                CorrelationId = correlationId,
                Exception = ex,
                Level = LogLevels.Fatal,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                ThreadName = Thread.CurrentThread.Name,
                Timestamp = DateTimeOffset.Now,
                ModuleMetadata = new ModuleMetadata()
                {
                    CallerFile = filepath.Substring(filepath.LastIndexOf(Path.DirectorySeparatorChar) + 1),
                    CallerMethod = caller,
                    LineNo = lineNo,
                    ModuleName = type.Name
                }
            };

            if (elaspedTime != null)
                logEntry.ElaspedTime = elaspedTime.Value.ToString();

            if (extendedProperties != null)
            {
                logEntry.ExtendedProperties = new List<KeyValuePair<string, object>>(extendedProperties).ToArray();
            }

            AddItem(logEntry);
        }

        /// <summary>
        /// Log an info message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="correlationId">a user supplied correlation id</param>
        /// <param name="extendedProperties">Custom key-value pair enumeration</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        public void LogInfo(Type type, 
            string message = "",
            string correlationId = "",
            IList<KeyValuePair<string, object>> extendedProperties = null,
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            [CallerFilePath] string filepath = "", 
            [CallerMemberName] string caller = "", 
            [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Info))
                return;

            var logEntry = new LogEntry()
            {
                ApplicationMetadata = AppMetaData.Clone(),
                Message = message,
                CorrelationId = correlationId,
                Exception = ex,
                Level = LogLevels.Info,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                ThreadName = Thread.CurrentThread.Name,
                Timestamp = DateTimeOffset.Now,
                ModuleMetadata = new ModuleMetadata()
                {
                    CallerFile = filepath.Substring(filepath.LastIndexOf(Path.DirectorySeparatorChar) + 1),
                    CallerMethod = caller,
                    LineNo = lineNo,
                    ModuleName = type.Name
                }
            };

            if (elaspedTime != null)
                logEntry.ElaspedTime = elaspedTime.Value.ToString();

            if (extendedProperties != null)
            {
                logEntry.ExtendedProperties = new List<KeyValuePair<string, object>>(extendedProperties).ToArray();
            }

            AddItem(logEntry);
        }

        /// <summary>
        /// Log a Warning message
        /// </summary>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="correlationId">a user supplied correlation id</param>
        /// <param name="extendedProperties">Custom key-value pair enumeration</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        public void LogWarning(Type type, 
            string message = "",
            string correlationId = "",
            IList<KeyValuePair<string, object>> extendedProperties = null,
            TimeSpan? elaspedTime = null, 
            Exception ex = null,
            [CallerFilePath] string filepath = "", 
            [CallerMemberName] string caller = "", 
            [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Warning))
                return;

            var logEntry = new LogEntry()
            {
                ApplicationMetadata = AppMetaData.Clone(),
                Message = message,
                CorrelationId = correlationId,
                Exception = ex,
                Level = LogLevels.Warning,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                ThreadName = Thread.CurrentThread.Name,
                Timestamp = DateTimeOffset.Now,
                ModuleMetadata = new ModuleMetadata()
                {
                    CallerFile = filepath.Substring(filepath.LastIndexOf(Path.DirectorySeparatorChar) + 1),
                    CallerMethod = caller,
                    LineNo = lineNo,
                    ModuleName = type.Name
                }
            };

            if (elaspedTime != null)
                logEntry.ElaspedTime = elaspedTime.Value.ToString();

            if (extendedProperties != null)
            {
                logEntry.ExtendedProperties = new List<KeyValuePair<string, object>>(extendedProperties).ToArray();
            }

            AddItem(logEntry);
        }

        #endregion

        #region privates

        private void DefaultWriteLogEntry(LogEntry logEntry)
        {
            WriteLogEntry(logEntry);
        }

        protected virtual void WriteLogEntry(LogEntry logEntry)
        {

        }

        #endregion

        #region Disposable Support
        public override void GeneralDispose()
        {
            
        }
        #endregion
    }
}
