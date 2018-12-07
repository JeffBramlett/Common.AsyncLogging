using Common.AsyncLogging;
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
            Logger.Instance.SetWriteAction(WriteTheLogEntry);

            ThreadPool.QueueUserWorkItem(new WaitCallback(StartLoop1ToLog));
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartLoop2ToLog));

            Console.ReadKey();

            Logger.Instance.Dispose();
        }

        private static void StartLoop1ToLog(object state)
        {
            for (var i = 0; i < 100; i++)
            {
                var something = "Loop: 1\tStep:" + i;
                Logger.Instance.LogInfo(typeof(Program), something);
            }
        }

        private static void StartLoop2ToLog(object state)
        {
            var logger = state as Logger;
            for (var i = 0; i < 100; i++)
            {
                var something = "Loop: 2\tStep:" + i;
                Logger.Instance.LogInfo(typeof(Program), something);
            }
        }

        private static void WriteTheLogEntry(LogEntry logEntry)
        {
            ShowLogEntryInConsole(logEntry);
        }

        private static void ShowLogEntryInConsole(LogEntry logEntry)
        {
            string contentToLog = string.Format("{0} - {1}", logEntry.Timestamp, logEntry.Message);
            Console.WriteLine(contentToLog);
        }

    }
}
