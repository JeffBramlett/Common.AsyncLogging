using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Abstract Log Writer  Repository with loading methods
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractLoggerWithWriters<T> : LogSpooler<T>, ILoggerWithWriters<T> where T : class, new()
    {
        #region Private
        private ApplicationData _appMetaData;

        private List<IWriteLogData<T>> _logWriters;
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

        #region Private Properties
        private List<IWriteLogData<T>> LogWriters
        {
            get
            {
                _logWriters = _logWriters ?? new List<IWriteLogData<T>>();
                return _logWriters;
            }
        }
        #endregion

        #region Events
        public event EventHandler LoadExceptionOccurred;
        public event EventHandler WriteLogExceptionOccurred;
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Singleton Ctor (must set the write action for log entries)
        /// </summary>
        /// <param name="allowedLogLevels">Bitwise enum to set allowed logging from configuration</param>
        public AbstractLoggerWithWriters()
        {
            SpoolerAction = WriteLogEntry;
            AllowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error;
        }

        /// <summary>
        /// Ctor overload for setting the allowed log levels
        /// </summary>
        /// <param name="allowedLogLevels">the allowed log levels</param>
        public AbstractLoggerWithWriters(LogLevels allowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error)
        {
            SpoolerAction = WriteLogEntry;
            AllowedLogLevels = allowedLogLevels;
        }

        #endregion

        #region Public logging
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
        #endregion

        #region Public Running Mods
        /// <summary>
        /// Sets the IsActive for a log writer by name
        /// </summary>
        /// <param name="writerName">the name of the writer</param>
        /// <param name="isActive">true to make it active and false otherwise</param>
        public void SetWriterActive(string writerName, bool isActive)
        {
            var writer = LogWriters.First(w => w.Name == writerName);
            if (writer != null)
            {
                writer.IsActive = isActive;
            }
        }
        #endregion

        #region Public loading
        /// <summary>
        /// Add Writer to Logger
        /// </summary>
        /// <param name="logWriter">Writer of log Data</param>
        public void AddWriter(IWriteLogData<T> logWriter)
        {
            if (!LogWriters.Any(w => w.Name == logWriter.Name))
            {
                LogWriters.Add(logWriter);
                LogWriters.Sort();
            }
            else
            {
                UpdateWriter(logWriter);
            }
        }

        /// <summary>
        /// Update the Writer by Name
        /// </summary>
        /// <param name="logWriter">the new LogWriter with the same name</param>
        public void UpdateWriter(IWriteLogData<T> logWriter)
        {
            var writer = LogWriters.First(w => w.Name == logWriter.Name);
            if (writer != null)
            {
                writer = logWriter;
            }
        }

        /// <summary>
        /// Load Log Writers from files in a path
        /// </summary>
        /// <param name="path">the path to the Log Writers</param>
        /// <param name="recurseSubPaths">true to load Writers from sub paths</param>
        public void LoadFromPath(string path, bool recurseSubPaths = true)
        {
            try
            {
                string dllFileFilter = "*.dll";
                string[] dllFiles = Directory.GetFiles(path, dllFileFilter, SearchOption.TopDirectoryOnly);
                foreach (var file in dllFiles)
                {
                    LoadFromAssemblyFileName(file);
                }

                string exeFileFilter = "*.exe";
                string[] exeFiles = Directory.GetFiles(path, exeFileFilter, SearchOption.TopDirectoryOnly);
                foreach (var file in exeFiles)
                {
                    LoadFromAssemblyFileName(file);
                }

                if (recurseSubPaths)
                {
                    string[] dirs = Directory.GetDirectories(path);
                    foreach (string dir in dirs)
                    {
                        LoadFromPath(dir);
                    }
                }
            }
            catch (Exception ex)
            {
                LoadExceptionOccurred?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Load Writers from Assembly by filename
        /// </summary>
        /// <param name="filename">the filename of the assembly to contain LogWriters</param>
        /// <returns>true if successful, false otherwise</returns>
        public bool LoadFromAssemblyFileName(string filename)
        {
            try
            {
                Assembly tempAssembly = Assembly.LoadFile(filename);

                return LoadFromAssembly(tempAssembly);
            }
            catch (Exception ex)
            {
                LoadExceptionOccurred?.Invoke(this, new ExceptionEventArgs(ex));
                return false;
            }
        }

        /// <summary>
        /// Load LogWriters from an Assembly
        /// </summary>
        /// <param name="assembly">the Assembly containing LogWriters</param>
        /// <returns>true if successfulr</returns>
        public bool LoadFromAssembly(Assembly assembly)
        {
            bool hasLoaded = false;
            try
            {
                var types = assembly.GetExportedTypes()
                    .Where(t => t.IsClass
                        && !t.IsAbstract
                        && t.IsDefined(typeof(LogDataWriterAttribute))
                        && typeof(IWriteLogData<T>).IsAssignableFrom(t));

                foreach (var type in types)
                {
                    IWriteLogData<T> activatedObject;
                    if (ActivateType(type, out activatedObject))
                    {
                        AddWriter(activatedObject);
                        hasLoaded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LoadExceptionOccurred?.Invoke(this, new ExceptionEventArgs(ex));
            }

            return hasLoaded;
        }
        #endregion

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
        #endregion

        #region Privates
        private void WriteLogEntry(T logEntry)
        {
            foreach (var writer in LogWriters.Where(w => w.IsActive))
            {
                Task.Run(() =>
                {
                    try
                    {
                        if (writer.CanWriteLogData(logEntry))
                        {
                            writer.WriteLogData(logEntry);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogExceptionOccurred?.Invoke(this, new ExceptionEventArgs(ex));
                    }
                });
            }
        }

        private bool ActivateType(Type type, out IWriteLogData<T> instantiatedObject)
        {
            instantiatedObject = (IWriteLogData<T>)Activator.CreateInstance(type);
            return true;
        }

        #endregion

    }
}
