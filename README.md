# Common.AsyncLogging
.Net Standard common asynchronous logging abstractions to make logging calls asynchronous in an application.  The abstration allows for any logging library (Log4Net and etc) to be used as the library abstractions will "spool" the log entries.
## Usage
Use the Logger class in this library as an instance or as a static singleton to enter log information that is then composed and "spooled" to a delegate you define (output to file, output to other logging frameworks (Log4Net and etc.).
### Instance
    Logger logger = new Logger(WriteTheLogEntry);
    logger.LogDebug(typeof(Program), "Logging test in .Net Core");
    logger.LogInfo(typeof(Program), "Some Info to be logged");
    . . .
    logger.Dispose();
    . . .
    
    private static void WriteTheLogEntry(LogEntry logEntry)
    {
        var originColor = Console.ForegroundColor;
        JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
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
    }
    
Output Example:

    {"Level":"Debug","Message":"Logging test in .Net Core","Timestamp":"2018-12-06T20:27:02.7359067-06:00","ApplicationMetadata":{"ApplicationName":"LogTestInCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null","Environment":"DESKTOP-QMR3K93","OS":"Win32NT"},"ModuleMetadata":{"ModuleName":"Program","CallerFile":"Main","CallerMethod":"Program.cs","LineNo":20}}

    {"Level":"Info","Message":"Some Info to be logged","Timestamp":"2018-12-06T20:27:02.7540645-06:00","ApplicationMetadata":{"ApplicationName":"LogTestInCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null","Environment":"DESKTOP-QMR3K93","OS":"Win32NT"},"ModuleMetadata":{"ModuleName":"Program","CallerFile":"Main","CallerMethod":"Program.cs","LineNo":21}}

### Static (thread-safe) Singleton
    Logger.Instance.SetWriteAction(WriteTheLogEntry);
    . . .
    Logger.Instance.LogInfo(typeof(Program), "This is some message);
    . . .
    Logger.Instance.Dispose();
