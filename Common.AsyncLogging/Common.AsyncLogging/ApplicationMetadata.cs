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
        /// The environment the application is executing in
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// The operating system the application is executing in
        /// </summary>
        public string OS { get; set; }
        #endregion
    }
}
