using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Contract for Logger with multiple writers loaded in runtime
    /// </summary>
    /// <typeparam name="T">the log entry type for the logger</typeparam>
    public interface ILoggerWithWriters<T> where T : class, new()
    {
        /// <summary>
        /// Logging Levels that are to be logged
        /// </summary>
        LogLevels AllowedLogLevels { get; set; }

        /// <summary>
        /// The application data for logging context
        /// </summary>
        ApplicationData Application { get; }

        /// <summary>
        /// Event for Exceptions that occur when loading writers
        /// </summary>
        event EventHandler LoadExceptionOccurred;

        /// <summary>
        /// Event for writing log exceptions
        /// </summary>
        event EventHandler WriteLogExceptionOccurred;

        /// <summary>
        /// Add a writer to receive and write log of type T
        /// </summary>
        /// <param name="logWriter">the log writer to add</param>
        void AddWriter(IWriteLogData<T> logWriter);

        /// <summary>
        /// Load writers from an Assembly
        /// </summary>
        /// <param name="assembly">the assembly to load writers from</param>
        /// <returns>true if successful, false otherwise</returns>
        bool LoadFromAssembly(Assembly assembly);

        /// <summary>
        /// Load writers from Assembly by filename
        /// </summary>
        /// <param name="filename">the filename of the assembly to load</param>
        /// <returns>true if successful, false othwerwise</returns>
        bool LoadFromAssemblyFileName(string filename);

        /// <summary>
        /// Load writers from path and subfolders
        /// </summary>
        /// <param name="path">the starting folder path for writers</param>
        /// <param name="recurseSubPaths">true to also search and load subfolders</param>
        void LoadFromPath(string path, bool recurseSubPaths = true);

        /// <summary>
        /// Log to Debug Level
        /// </summary>
        /// <param name="type">the caller type</param>
        /// <param name="message">(optional) the message to log</param>
        /// <param name="correlationId">(optional) the correlation id</param>
        /// <param name="extendedProperties">(optional) extended properties - for custom values</param>
        /// <param name="elaspedTime">(optional) elasped time (TimeSpan)</param>
        /// <param name="ex">(optional) exception to include in log</param>
        /// <param name="filepath">(from compiler) the filepath of the caller</param>
        /// <param name="caller">(from compiler) the caller (method name)</param>
        /// <param name="lineNo">(from compiler) the line no of the call to logger</param>
        void LogDebug(Type type, string message = "", string correlationId = "", IDictionary<string, object> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0);

        /// <summary>
        /// Log to Error Level
        /// </summary>
        /// <param name="type">the caller type</param>
        /// <param name="message">(optional) the message to log</param>
        /// <param name="correlationId">(optional) the correlation id</param>
        /// <param name="extendedProperties">(optional) extended properties - for custom values</param>
        /// <param name="elaspedTime">(optional) elasped time (TimeSpan)</param>
        /// <param name="ex">(optional) exception to include in log</param>
        /// <param name="filepath">(from compiler) the filepath of the caller</param>
        /// <param name="caller">(from compiler) the caller (method name)</param>
        /// <param name="lineNo">(from compiler) the line no of the call to logger</param>
        void LogError(Type type, string message = "", string correlationId = "", IDictionary<string, object> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0);
        
        /// <summary>
        /// Log to Fatal Level
        /// </summary>
        /// <param name="type">the caller type</param>
        /// <param name="message">(optional) the message to log</param>
        /// <param name="correlationId">(optional) the correlation id</param>
        /// <param name="extendedProperties">(optional) extended properties - for custom values</param>
        /// <param name="elaspedTime">(optional) elasped time (TimeSpan)</param>
        /// <param name="ex">(optional) exception to include in log</param>
        /// <param name="filepath">(from compiler) the filepath of the caller</param>
        /// <param name="caller">(from compiler) the caller (method name)</param>
        /// <param name="lineNo">(from compiler) the line no of the call to logger</param>
        void LogFatal(Type type, string message = "", string correlationId = "", IDictionary<string, object> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0);

        /// <summary>
        /// Log to Info Level
        /// </summary>
        /// <param name="type">the caller type</param>
        /// <param name="message">(optional) the message to log</param>
        /// <param name="correlationId">(optional) the correlation id</param>
        /// <param name="extendedProperties">(optional) extended properties - for custom values</param>
        /// <param name="elaspedTime">(optional) elasped time (TimeSpan)</param>
        /// <param name="ex">(optional) exception to include in log</param>
        /// <param name="filepath">(from compiler) the filepath of the caller</param>
        /// <param name="caller">(from compiler) the caller (method name)</param>
        /// <param name="lineNo">(from compiler) the line no of the call to logger</param>
        void LogInfo(Type type, string message = "", string correlationId = "", IDictionary<string, object> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0);

        /// <summary>
        /// Log to Warning Level
        /// </summary>
        /// <param name="type">the caller type</param>
        /// <param name="message">(optional) the message to log</param>
        /// <param name="correlationId">(optional) the correlation id</param>
        /// <param name="extendedProperties">(optional) extended properties - for custom values</param>
        /// <param name="elaspedTime">(optional) elasped time (TimeSpan)</param>
        /// <param name="ex">(optional) exception to include in log</param>
        /// <param name="filepath">(from compiler) the filepath of the caller</param>
        /// <param name="caller">(from compiler) the caller (method name)</param>
        /// <param name="lineNo">(from compiler) the line no of the call to logger</param>
        void LogWarning(Type type, string message = "", string correlationId = "", IDictionary<string, object> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, [CallerFilePath] string filepath = "", [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0);
        
        /// <summary>
        /// Set a Writer (previously added to this logger) as active or inactive
        /// </summary>
        /// <param name="writerName">the name of the writer</param>
        /// <param name="isActive">true for active, false for inactive</param>
        void SetWriterActive(string writerName, bool isActive);

        /// <summary>
        /// Update the Writer with a new version
        /// </summary>
        /// <param name="logWriter">the new logWriter</param>
        void UpdateWriter(IWriteLogData<T> logWriter);
    }
}