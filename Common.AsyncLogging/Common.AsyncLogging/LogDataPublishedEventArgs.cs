using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AsyncLogging
{
    public class LogDataPublishedEventArgs<T>: EventArgs where T : class, new()
    {
        public T LogData { get; set; }
    }
}
