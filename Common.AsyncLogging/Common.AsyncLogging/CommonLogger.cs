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
    public class CommonLogger : GenericLogger<LogEntry>, ICommonLogger<LogEntry>
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

        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="writeAction">write Action for log entries (spooled to this delegate)</param>
        public CommonLogger(Action<ILogEntry> writeAction, LogLevels allowedLogLevels = LogLevels.Debug | LogLevels.Info | LogLevels.Warning | LogLevels.Fatal | LogLevels.Error):
            base(writeAction, allowedLogLevels)
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

        protected virtual void WriteLogEntry(ILogEntry logEntry)
        {

        }

        #endregion
    }
}
