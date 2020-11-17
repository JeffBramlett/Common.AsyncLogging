using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Common.AsyncLogging
{
    public class GenericLogger<T> : GenericSpooler<T>, ICommonLogger<T> where T : ILogEntry, new()
    {
        #region Private
        private ApplicationMetaData _appMetaData;
        #endregion

        #region Properties
        /// <summary>
        /// Bitwise enum to limit logging by log levels
        /// </summary>
        public LogLevels AllowedLogLevels { get; set; }

        public ApplicationMetaData AppMetaData
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
        public GenericLogger()
        {
            SetWriteAction(DefaultWriteLogEntry);
            AllowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error;
        }

        /// <summary>
        /// Ctor overload for setting the allowed log levels
        /// </summary>
        /// <param name="allowedLogLevels">the allowed log levels</param>
        public GenericLogger(LogLevels allowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error)
        {
            SetWriteAction(DefaultWriteLogEntry);
            AllowedLogLevels = allowedLogLevels;
        }

        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="writeAction">write Action for log entries (spooled to this delegate)</param>
        public GenericLogger(Action<T> writeAction, LogLevels allowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error)
        {
            SetWriteAction(writeAction);
            AllowedLogLevels = allowedLogLevels;
        }
        #endregion


        public void LogDebug(Type type, string message = "", string correlationId = "", IList<KeyValuePair<string, object>> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Debug))
                return;

            var logEntry = CreateLogEntry(LogLevels.Debug, type, message, correlationId, extendedProperties, elaspedTime, ex, filepath, caller, lineNo);

            AddItem(logEntry);
        }

        public void LogError(Type type, string message = "", string correlationId = "", IList<KeyValuePair<string, object>> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Error))
                return;

            var logEntry = CreateLogEntry(LogLevels.Error, type, message, correlationId, extendedProperties, elaspedTime, ex, filepath, caller, lineNo);

            AddItem(logEntry);
        }

        public void LogFatal(Type type, string message = "", string correlationId = "", IList<KeyValuePair<string, object>> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Fatal))
                return;

            var logEntry = CreateLogEntry(LogLevels.Fatal, type, message, correlationId, extendedProperties, elaspedTime, ex, filepath, caller, lineNo);

            AddItem(logEntry);
        }

        public void LogInfo(Type type, string message = "", string correlationId = "", IList<KeyValuePair<string, object>> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Info))
                return;

            var logEntry = CreateLogEntry(LogLevels.Info, type, message, correlationId, extendedProperties, elaspedTime, ex, filepath, caller, lineNo);

            AddItem(logEntry);
        }

        public void LogWarning(Type type, string message = "", string correlationId = "", IList<KeyValuePair<string, object>> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Warning))
                return;

            var logEntry = CreateLogEntry(LogLevels.Warning, type, message, correlationId, extendedProperties, elaspedTime, ex, filepath, caller, lineNo);

            AddItem(logEntry);
        }

        public void SetWriteAction(Action<T> writeAction)
        {
            if (writeAction == null)
                throw new ArgumentNullException(nameof(writeAction));

            SpoolerAction = writeAction;
        }
        #region privates

        private void DefaultWriteLogEntry(T logEntry)
        {
            WriteLogEntry(logEntry);
        }

        protected virtual T CreateLogEntry(
            LogLevels loglevel,
            Type type, 
            string message = "", 
            string correlationId = "", 
            IList<KeyValuePair<string, object>> extendedProperties = null, 
            TimeSpan? elaspedTime = null, 
            Exception ex = null, 
            string filepath = "", 
            string caller = "", 
            int lineNo = 0)
        {
            var logEntry = new T()
            {
                ApplicationMetadata = AppMetaData.Clone(),
                Message = message,
                CorrelationId = correlationId,
                Exception = ex,
                Level = loglevel,
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

            return logEntry;
        }

        protected virtual void WriteLogEntry(T logEntry)
        {

        }

        #endregion

    }
}
