using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Executing class for simple file writing (one .log file per day)
    /// </summary>
    public class DefaultFileLog
    {
        #region fields
        private string _logPath;
        #endregion

        #region Properties

        private string LogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logPath) || !Directory.Exists(_logPath))
                {
                    _logPath = Environment.CurrentDirectory;
                }

                return _logPath;
            }
        }
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="logPath">the directory/folder to contain files</param>
        public DefaultFileLog(string logPath = "")
        {
            _logPath = logPath;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Write content to the file
        /// </summary>
        /// <param name="content">the content to write</param>
        public void WriteToLog(string content)
        {
            string filename = GetFilename();
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine(content);
            }
        }
        #endregion

        #region Privates

        private string GetFilename()
        {
            string filename = string.Format("{0}-{1}-{2}.log", DateTime.Today.Day, DateTime.Today.Month,
                DateTime.Today.Year);

            return Path.Combine(LogPath, filename);
        }
        #endregion
    }
}
