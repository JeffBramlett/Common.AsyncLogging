using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Executing class for simple file writing (one .log file per day)
    /// </summary>
    public class DefaultFileLog : IDefaultFileLog
    {
        #region fields
        private string _logPath;

        #endregion

        #region Properties

        private string LogPath
        {
            get
            {
                _logPath = string.IsNullOrEmpty(_logPath) ? Environment.CurrentDirectory : _logPath;

                if (!Directory.Exists(_logPath))
                {
                    Directory.CreateDirectory(_logPath);
                }

                return _logPath;
            }
        }

        private string FilePattern { get; set; }

        private string FileName { get; set; }
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="logPath">the directory/folder to contain files</param>
        public DefaultFileLog(string filePattern = "dd-MM-yyyy'.log'", string logPath = "")
        {
            _logPath = logPath;
            FilePattern = filePattern;
            FileName = GetFilename();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Write content to the file
        /// </summary>
        /// <param name="content">the content to write</param>
        public void WriteToLog(string content)
        {
            using (StreamWriter sw = new StreamWriter(FileName, true))
            {
                sw.WriteLine(content);
            }
        }
        #endregion

        #region Privates

        private string GetFilename()
        {
            string filename = DateTime.Now.ToString(FilePattern);

            return Path.Combine(LogPath, filename);
        }
        #endregion
    }
}
 