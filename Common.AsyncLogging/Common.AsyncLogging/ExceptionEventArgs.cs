using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AsyncLogging
{
    public class ExceptionEventArgs: EventArgs
    {
        public Exception Exception { get; private set; }

        public ExceptionEventArgs()
        {

        }

        public ExceptionEventArgs(Exception ex)
        {
            Exception = ex;
        }

    }
}
