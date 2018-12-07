using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AsyncLogging
{
    /// <summary>
    /// The caller meta data
    /// </summary>
    public class ModuleMetadata
    {
        /// <summary>
        /// The name of the module (class)
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// The file that the call originated from
        /// </summary>
        public string CallerFile { get; set; }

        /// <summary>
        /// The method in the file that the call originated from
        /// </summary>
        public string CallerMethod { get; set; }

        /// <summary>
        /// The line number in the CallerMethod where the call originated from
        /// </summary>
        public int LineNo { get; set; }

    }
}
