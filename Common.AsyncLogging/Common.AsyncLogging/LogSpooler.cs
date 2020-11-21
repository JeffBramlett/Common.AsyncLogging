using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AsyncLogging
{
    /// <summary>
    /// An asynchronous generic executing class that controls spooling -- 
    /// putting jobs on a queue and taking them off one at a time. This 
    /// class will not block the adding of items in the spool.  And will 
    /// execute the delegate in a thread-safe manner.
    /// Author: Jeff Bramlett (jeffrey.bramlett@gmail.com)
    /// </summary>
    /// <remarks>
    /// There is ONLY one thread that executes the Action delegate.  It
    /// is assumed that the Action delegate blocks. Thus the spooler will 
    /// ONLY execute each item as fast as the Action delegate executes on one
    /// item.
    /// </remarks>
    /// <typeparam name="T">The Type of object to provide spooling for</typeparam>
    public class LogSpooler<T> : ILogSpooler<T> where T:class
    {
        #region Class Scope Members
        bool _isDisposed;

        /// <summary>
        /// Threadsafe collection class
        /// </summary>
        readonly ConcurrentQueue<ItemMetaData> _inputs;
        #endregion

        #region Properties
        /// <summary>
        /// True if there are Currently more items in the queue
        /// </summary>
        public bool HasMore
        {
            get
            {
                return _inputs.Count > 0;
            }
        }
        #endregion

        #region Delegates and Events
        /// <summary>
        /// Delegate to use when getting notification that an exception has occurred
        /// </summary>
        /// <param name="sender">could be either the spooler or the object containing the callback (the callback produced the exception)</param>
        /// <param name="ex">the exception caught</param>
        public delegate void ExceptionEncounteredDelegate(object sender, Exception ex);

        /// <summary>
        /// Get notification that an exception has occurred
        /// </summary>
        public event ExceptionEncounteredDelegate ExceptionEncountered;

        public delegate void SpoolerEmptyDelegate();

        public event SpoolerEmptyDelegate SpoolerEmpty;

        protected Action<T> SpoolerAction { get; set; }
        #endregion

        #region Ctors and Dtors

        public LogSpooler()
        {
            _inputs = new ConcurrentQueue<ItemMetaData>();
            SpoolerAction = ProcessSpooledItem;
        }

        protected virtual void ProcessSpooledItem(T item)
        {

        }

        /// <summary>
        /// Default Ctor.  The callback is required.
        /// </summary>
        /// <param name="spoolerAction">the delegate to receive each item from the spool</param>
        public LogSpooler(Action<T> spoolerAction)
        {
            if (spoolerAction == null)
            {
                throw new NullReferenceException("Spooler Action cannot be null");
            }

            _inputs = new ConcurrentQueue<ItemMetaData>();
            SpoolerAction = spoolerAction;

        }

        /// <summary>
        /// Finalizer . . . 
        /// </summary>
        ~LogSpooler()
        {
            Dispose(false);
        }

        #endregion

        #region Public
        /// <summary>
        /// Adds an Item of type T to the queue
        /// </summary>
        /// <param name="item">the item of type T</param>
        /// <param name="itemCausesStop">if true the spooler stops after this item and must be restarted</param>
        public void AddItem(T item, bool itemCausesStop = false)
        {
            try
            {
                ItemMetaData storedItem = new ItemMetaData()
                {
                    HoldOnItem = itemCausesStop,
                    Item = item
                };

                _inputs.Enqueue(storedItem);
                StartProcess();
                //_trafficEvent.Set();
            }
            catch (Exception ex)
            {
                NotifyException(this, ex);
            }
        }

        /// <summary>
        /// Stops the spooler and empties the Queue
        /// </summary>
        public void Reset()
        {
            try
            {
                ItemMetaData ignoredData;
                while (_inputs.TryDequeue(out ignoredData))
                {
                    // do nothing; just clear the queue
                }
            }
            catch (Exception ex)
            {
                NotifyException(this, ex);
            }
        }

        #endregion

        #region Virtuals
        /// <summary>
        /// Virtual method to override for adding general disposal by extending classes
        /// </summary>
        public virtual void GeneralDispose()
        {

        }

        /// <summary>
        /// Virtual method to override for adding dispose called using the Dispose() by extending classes
        /// </summary>
        public virtual void DeterministicDispose()
        {

        }
        /// <summary>
        /// Virtual method to override for adding Finalizer disposal by calling the Destructor (finalize) on extending classes
        /// </summary>
        public virtual void FinalizeDispose()
        {

        }
        #endregion

        #region Privates
        Task runningTask;
        /// <summary>
        /// Starts the thread that spools the items off the queue (if not yet started)
        /// </summary>
        private void StartProcess()
        {
            if(runningTask == null || runningTask.IsCompleted)
            {
                runningTask = Task.Factory.StartNew(() => ProcessWhileHasInput());
            }
        }

        /// <summary>
        /// The method the thread executes.  The thread executes the Callback delegate for each item it takes off the queue.
        /// </summary>
        private void ProcessWhileHasInput()
        {
            try
            {
                // Keep the thread alive unless the exit event is signaled
                while (true)
                {
                    ItemMetaData itemData;
                    while (_inputs.TryDequeue(out itemData))
                    {
                        SpoolerAction(itemData.Item);
                    }

                    SpoolerEmpty?.Invoke();

                    break;
                }
            }
            catch (Exception ex)
            {
                // Normal call to abort for the thread, call in finalize will necessarily end here if 
                // the thread has not already been stopped
                NotifyException(this, ex);
            }
        }

        /// <summary>
        /// Call the event (if there are any listeners)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        private void NotifyException(object sender, Exception ex)
        {
            ExceptionEncountered?.Invoke(sender, ex);
        }

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Dispose implementation to type of disposing
        /// </summary>
        /// <param name="disposing">True for deterministic, false for finalization</param>
        private void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            // General cleanup logic here
            GeneralDispose();

            if (disposing) // Deterministic only cleanup
            {
                DeterministicDispose();
            }
            else // Finalizer only cleanup
            {
                FinalizeDispose();
            }

            _isDisposed = true;
        }

        /// <summary>
        /// Release all resources (clears List)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Inner classes
        private class ItemMetaData
        {
            /// <summary>
            /// If true spooler stops after this item
            /// </summary>
            public bool HoldOnItem { get; set; }

            /// <summary>
            /// The item in the spool
            /// </summary>
            public T Item { get; set; }
        }
        #endregion
    }
}
