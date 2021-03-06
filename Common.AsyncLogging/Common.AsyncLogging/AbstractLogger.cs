﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Executing abstraction of a generic logger
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractLogger<T> : LogSpooler<T>, ICommonLogger<T> where T: class, new()
    {
        #region Private
        private ApplicationData _appMetaData;
        #endregion

        #region Properties
        /// <summary>
        /// Bitwise enum to limit logging by log levels
        /// </summary>
        public LogLevels AllowedLogLevels { get; set; }

        /// <summary>
        /// The Application metadata
        /// </summary>
        public ApplicationData Application 
        {
            get
            {
                if (_appMetaData == null)
                {
                    _appMetaData = new ApplicationData();
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

        #region Events
        public event EventHandler<T> LogDataPublish;
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Singleton Ctor (must set the write action for log entries)
        /// </summary>
        /// <param name="allowedLogLevels">Bitwise enum to set allowed logging from configuration</param>
        public AbstractLogger()
        {
            SpoolerAction = WriteLogEntry;
            AllowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error;
        }

        /// <summary>
        /// Ctor overload for setting the allowed log levels
        /// </summary>
        /// <param name="allowedLogLevels">the allowed log levels</param>
        public AbstractLogger(LogLevels allowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error)
        {
            SpoolerAction = WriteLogEntry;
            AllowedLogLevels = allowedLogLevels;
        }

        #endregion

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
        public void LogDebug(Type type, string message = "", string correlationId = "", IDictionary<string, object> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Debug))
                return;

            var logEntry = CreateLogEntry(LogLevels.Debug, type, message, correlationId, extendedProperties, elaspedTime, ex, filepath, caller, lineNo);

            AddItem(logEntry);
        }


        /// <summary>
        /// Log an Error message
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
        public void LogError(Type type, string message = "", string correlationId = "", IDictionary<string, object> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Error))
                return;

            var logEntry = CreateLogEntry(LogLevels.Error, type, message, correlationId, extendedProperties, elaspedTime, ex, filepath, caller, lineNo);

            AddItem(logEntry);
        }


        /// <summary>
        /// Log a Fatal message
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
        public void LogFatal(Type type, string message = "", string correlationId = "", IDictionary<string, object> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Fatal))
                return;

            var logEntry = CreateLogEntry(LogLevels.Fatal, type, message, correlationId, extendedProperties, elaspedTime, ex, filepath, caller, lineNo);

            AddItem(logEntry);
        }


        /// <summary>
        /// Log an information message
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
        public void LogInfo(Type type, string message = "", string correlationId = "", IDictionary<string, object> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Info))
                return;

            var logEntry = CreateLogEntry(LogLevels.Info, type, message, correlationId, extendedProperties, elaspedTime, ex, filepath, caller, lineNo);

            AddItem(logEntry);
        }

        /// <summary>
        /// Log a Warning message
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
        public void LogWarning(Type type, string message = "", string correlationId = "", IDictionary<string, object> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0)
        {
            if (!AllowedLogLevels.HasFlag(LogLevels.Warning))
                return;

            var logEntry = CreateLogEntry(LogLevels.Warning, type, message, correlationId, extendedProperties, elaspedTime, ex, filepath, caller, lineNo);

            AddItem(logEntry);
        }


        #region Protected Abstracts
        /// <summary>
        /// Abstract method used to create the logging class
        /// </summary>
        /// <param name="loglevel">the logging level (bitwise)</param>
        /// <param name="type">the module(class) type</param>
        /// <param name="message">the message to log (optional)</param>
        /// <param name="correlationId">User supplied correlation id</param>
        /// <param name="extendedProperites">Custom key-value pair enumeration</param>
        /// <param name="elaspedTime">the elasped time for the LogEntry (optional)</param>
        /// <param name="ex">The exception to include in the LogEntry (optional)</param>
        /// <param name="filepath">The filepath where the log originates (supplied by system)</param>
        /// <param name="caller">The calling method (supplied by system)</param>
        /// <param name="lineNo">The line number where the Log originates (supplied by system)/param>
        /// <returns></returns>
        protected abstract T CreateLogEntry(
            LogLevels loglevel,
            Type type,
            string message = "",
            string correlationId = "",
            IDictionary<string, object> extendedProperties = null,
            TimeSpan? elaspedTime = null,
            Exception ex = null,
            string filepath = "",
            string caller = "",
            int lineNo = 0);

        private void WriteLogEntry(T logEntry)
        {
            PublishLogData(logEntry);
        }
        #endregion

        #region privates
        private void PublishLogData(T logData)
        {
            Task.Run(() =>
            {
                 LogDataPublish?.Invoke(this, logData);
            });
        }
        #endregion

    }
}
