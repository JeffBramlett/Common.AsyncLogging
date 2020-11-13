using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Data class for Application Meta data
    /// </summary>
    public class ApplicationMetaData
    {
        #region Auto Properties
        /// <summary>
        /// The entry point application
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// The domain of the application
        /// </summary>
        public string ApplicationDomain { get; set; }

        /// <summary>
        /// Application Version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The environment the application is executing in
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// The operating system the application is executing in
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// The current process id
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// The current process name
        /// </summary>
        public string ProcessName { get; set; }
        #endregion
    }
}
