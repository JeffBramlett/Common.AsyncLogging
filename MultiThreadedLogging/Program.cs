using Common.AsyncLogging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadedLogging
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonLogger.Instance.LogDataPublish += Instance_LogDataPublish;

            ThreadPool.QueueUserWorkItem(new WaitCallback(StartLoop1ToLog));
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartLoop2ToLog));

            Console.ReadKey();

            CommonLogger.Instance.Dispose();
        }

        private static void Instance_LogDataPublish(object sender, LogData e)
        {
            string log = JsonConvert.SerializeObject(e);

            Console.WriteLine(log);
        }

        private static void StartLoop1ToLog(object state)
        {
            for ( var i = 0; i < 100; i++)
            {
                var something = "Loop: 1\tStep:" + i;
                CommonLogger.Instance.LogInfo(typeof(Program), something, Guid.NewGuid().ToString());
            }
        }

        private static void StartLoop2ToLog(object state)
        {
            var logger = state as CommonLogger;
            for (var i = 0; i < 100; i++)
            {
                var something = "Loop: 2\tStep:" + i;
                CommonLogger.Instance.LogInfo(typeof(Program), something, Guid.NewGuid().ToString());
            }
        }

        private static void WriteTheLogEntry(ILogData logEntry)
        {
            ShowLogEntryInConsole(logEntry);
        }
        
        private static void ShowLogEntryInConsole(ILogData logEntry)
        {
            string contentToLog = string.Format("{0} - {1}: {2}", logEntry.Timestamp, logEntry.Message, logEntry.CorrelationId);
            Console.WriteLine(logEntry.ToKeyValuePairs());
        }

    }
}
