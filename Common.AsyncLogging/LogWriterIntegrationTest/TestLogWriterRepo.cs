using Common.AsyncLogging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace LogWriterIntegrationTest
{
    class TestLogWriterRepo : AbstractLoggerWithWriters<LogData>
    {
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
    }
}
