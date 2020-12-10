using Common.AsyncLogging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogWriterIntegrationTest
{
    [LogDataWriter]
    public class DisplayWriter : IWriteLogData<LogData>
    {
        private bool disposedValue;

        public string Name
        {
            get { return "DispayWriter"; }
        }

        public bool IsActive { get; set; }

        public int SortOrder 
        { 
            get{ return 5; }
            set { }
        }

        public DisplayWriter()
        {
            IsActive = true;
        }

        public bool CanWriteLogData(LogData logData)
        {
            return true;
        }

        public int Compare(IWriteLogData<LogData> x, IWriteLogData<LogData> y)
        {
            return x.SortOrder.CompareTo(y);
        }

        public int CompareTo(IWriteLogData<LogData> other)
        {
            return SortOrder - other.SortOrder;
        }

        public void WriteLogData(LogData logData)
        {
            var originColor = Console.ForegroundColor;

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            serializerSettings.Converters.Add(new StringEnumConverter());

            string contents = JsonConvert.SerializeObject(logData, serializerSettings);

            switch (logData.Level)
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
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DisplayWriter()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
