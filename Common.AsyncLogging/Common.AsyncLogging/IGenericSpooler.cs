using System;

namespace Common.AsyncLogging
{
    /// <summary>
    /// Contract for a spooler, if you want to make your own.
    /// Author: Jeff Bramlett (jeffrey.bramlett@gmail.com)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericSpooler<T> : IDisposable
    {
        /// <summary>
        /// Add Item to the spool
        /// </summary>
        /// <param name="item"></param>
        void AddItem(T item, bool itemCausesStop = false);


        /// <summary>
        /// Some exception happened in the spool, either in the spooler or in the message handler
        /// </summary>
        event GenericSpooler<T>.ExceptionEncounteredDelegate ExceptionEncountered;
    }
}
