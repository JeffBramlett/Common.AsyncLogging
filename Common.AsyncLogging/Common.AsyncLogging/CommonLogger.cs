﻿using System;
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
    public class CommonLogger : AbstractLogger<LogData>, ICommonLogger<LogData>
    {

        #region Ctors and Dtors
        /// <summary>
        /// Singleton Ctor (must set the write action for log entries)
        /// </summary>
        /// <param name="allowedLogLevels">Bitwise enum to set allowed logging from configuration</param>
        public CommonLogger():
            base()
        {
        }

        /// <summary>
        /// Ctor for setting Allowed log levels
        /// </summary>
        /// <param name="allowedLogLevels">the allowed log levels</param>
        public CommonLogger(LogLevels allowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error):
            base(allowedLogLevels) 
        {
        }

        #endregion

        #region Singleton
        private static Lazy<CommonLogger> _instance = new Lazy<CommonLogger>();

        public static CommonLogger Instance
        {
            get { return _instance.Value; }
        }
        #endregion

        #region protected

        protected override LogData CreateLogEntry(LogLevels loglevel, Type type, string message = "", string correlationId = "", IDictionary<string, object> extendedProperties = null, TimeSpan? elaspedTime = null, Exception ex = null, string filepath = "", string caller = "", int lineNo = 0)
        {
            var logEntry = new LogData()
            {
                Application = Application.Clone(),
                Message = message,
                CorrelationId = correlationId,
                Exception = ex,
                Level = loglevel,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                ThreadName = Thread.CurrentThread.Name,
                Timestamp = DateTimeOffset.Now,
                Module = new ModuleData()
                {
                    CallerFile = filepath.Substring(filepath.LastIndexOf(Path.DirectorySeparatorChar) + 1),
                    CallerMethod = caller,
                    LineNo = lineNo,
                    ModuleName = type.Name
                }
            };

            if (elaspedTime != null)
                logEntry.ElaspedTime = elaspedTime.Value.ToString();

            logEntry.ExtendedProperties = extendedProperties;

            return logEntry;
        }


        #endregion
    }
}
