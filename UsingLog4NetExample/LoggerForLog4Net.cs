using Common.Logging;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UsingLog4NetExample
{
    class WriterForLog4Net: IWriteLogEntry
    {
        public void WriteTheLogEntry(LogEntry logEntry)
        {
            ILog toLog4Net = LogManager.GetLogger(logEntry.ApplicationMetadata.ApplicationName);
            var logContent = TranslateLogEntryToJson(logEntry);

            switch (logEntry.Level)
            {
                case LogLevels.Debug:
                    toLog4Net.Debug(logContent);
                    break;
                case LogLevels.Info:
                    toLog4Net.Info(logContent);
                    break;
                case LogLevels.Warning:
                    toLog4Net.Warn(logContent);
                    break;
                case LogLevels.Error:
                    toLog4Net.Error(logContent);
                    break;
                case LogLevels.Fatal:
                    toLog4Net.Fatal(logContent);
                    break;
            }
        }

        private string TranslateLogEntryToJson(LogEntry logEntry)
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            serializerSettings.Converters.Add(new StringEnumConverter());

            return JsonConvert.SerializeObject(logEntry, serializerSettings);
        }
    }
}
