using System;
using System.Collections.Generic;

namespace Common.AsyncLogging
{
    public interface ILogEntry
    {
        ApplicationMetaData ApplicationMetadata { get; set; }
        string CorrelationId { get; set; }
        IList<CustomPair> CustomPairs { get; set; }
        double ElaspedMilliseconds { get; }
        string ElaspedTime { get; set; }
        Exception Exception { get; set; }
        LogLevels Level { get; set; }
        string Message { get; set; }
        ModuleMetadata ModuleMetadata { get; set; }
        DateTimeOffset Timestamp { get; set; }
    }
}