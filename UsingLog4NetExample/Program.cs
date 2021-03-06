﻿using Common.AsyncLogging;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingLog4NetExample
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonLogger logger = new CommonLogger();
            logger.LogDataPublish += Logger_LogDataPublish;

            LogMore(logger);
            LogWithException(logger);

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();

            logger.Dispose();

        }

        private static void Logger_LogDataPublish(object sender, LogData e)
        {
            WriteTheLogEntry(e);
        }

        private static void LogMore(CommonLogger logger)
        {
            Stopwatch sw = Stopwatch.StartNew();
            logger.LogDebug(typeof(Program), "with MORE");
            sw.Stop();

            logger.LogInfo(typeof(Program), "How much time to log", "", null, sw.Elapsed);
        }

        private static void LogWithException(CommonLogger logger)
        {
            try
            {
                var zero = 0;
                var exception = 4 / zero;  // throws divide by zero exception
            }
            catch (Exception ex)
            {
                logger.LogError(typeof(Program), "Divide by Zero test", "", null, null, ex);
            }
        }

        #region Write Logs to Log4Net
        private static void WriteTheLogEntry(ILogData logEntry)
        {
            ILog toLog4Net = LogManager.GetLogger(logEntry.Application.ApplicationName);
            
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

        private static string TranslateLogEntryToJson(ILogData logEntry)
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            serializerSettings.Converters.Add(new StringEnumConverter());

            return JsonConvert.SerializeObject(logEntry, serializerSettings);
        }

        #endregion
    }

    public static class Extensions
    {
    }
}
