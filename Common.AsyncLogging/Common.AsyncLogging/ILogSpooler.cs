using System;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Contract for a spooler, if you want to make your own.
    /// Author: Jeff Bramlett (jeffrey.bramlett@gmail.com)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILogSpooler<T> : IDisposable where T: class
    {
        /// <summary>
        /// Add Item to the spool
        /// </summary>
        /// <param name="LogItem"></param>
        void AddItem(T LogItem, bool itemCausesStop = false);


        /// <summary>
        /// Some exception happened in the spool, either in the spooler or in the message handler
        /// </summary>
        event LogSpooler<T>.ExceptionEncounteredDelegate ExceptionEncountered;
    }
}
