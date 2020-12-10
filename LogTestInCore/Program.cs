using Common.AsyncLogging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LogTestInCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();

            CommonLogger logger = new CommonLogger();
            logger.LogDataPublish += (sender, logData) => { WriteTheLogEntry(logData); };

            sw.Stop();

            logger.LogDebug(typeof(Program), "Logging test in .Net Core");
            logger.LogInfo(typeof(Program), "Some Info to be logged");
            LogMore(logger);
            LogWithException(logger);

            var val = (short)logger.AllowedLogLevels;

            LogLevels test = (LogLevels)val;

            Console.ReadKey();

            logger.Dispose();
        }

        private static void LogMore(CommonLogger logger)
        {
            IDictionary<string, object> logPairs = new Dictionary<string, object>()
            {
                { "Custom1", 123245 }
            };

            Stopwatch sw = Stopwatch.StartNew();
            logger.LogWarning(typeof(Program), "with MORE", "LM", logPairs, null, null);
            sw.Stop();

            logger.LogInfo(typeof(Program), "How much time to log", "LM", null, sw.Elapsed);
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

        private static DefaultFileLog defaultFileLog = new DefaultFileLog();

        private static void WriteTheLogEntry(ILogData logEntry)
        {
            var originColor = Console.ForegroundColor;

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            serializerSettings.Converters.Add(new StringEnumConverter());

            string contents = JsonConvert.SerializeObject(logEntry, serializerSettings);

            switch (logEntry.Level)
            {
                case LogLevels.Debug:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(contents);
                    break;
                case LogLevels.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(contents);
                    break;
                case LogLevels.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(contents);
                    break;
                case LogLevels.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(contents);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(contents);
                    break;
            }

            Console.ForegroundColor = originColor;
            defaultFileLog.WriteToLog(contents);
        }

    }
}
