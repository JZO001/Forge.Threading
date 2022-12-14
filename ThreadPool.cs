/* *********************************************************************
 * Date: 23 Jul 2012
 * Created by: Zoltan Juhasz
 * E-Mail: forge@jzo.hu
***********************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Forge.Configuration;
using Forge.Configuration.Shared;
using Forge.Legacy;
using Forge.Logging.Abstraction;
using Forge.Shared;
using Forge.Threading.ConfigSection;
using Forge.Threading.Options;

namespace Forge.Threading
{

    /// <summary>
    /// ThreadPool implementation for specific use cases
    /// </summary>
    public sealed class ThreadPool : MBRBase, IThreadPool
    {

        #region Fields

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ThreadPool));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string mName = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool mIsReadOnly = false;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int mMinThreadNumber = Environment.ProcessorCount;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int mMaxThreadNumber = 32768;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int mShutDownIdleThreadTime = 120000; // 2 minutes

        private readonly Queue<TaskContainer> mTaskContainerQueue = new Queue<TaskContainer>();

        private readonly List<WorkerThread> mExistingThreads = new List<WorkerThread>();

        private readonly List<WorkerThread> mInactiveThreads = new List<WorkerThread>();

        private int mThreadId = 0;

        private int mThreadNumber = 0;

        private readonly object mLockObject = new object();

        private bool mDisposed = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPool"/> class.
        /// </summary>
        public ThreadPool()
        {
            OpenConfiguration();
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPool"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ThreadPool(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) ThrowHelper.ThrowArgumentNullException("name");
            mName = name;
            OpenConfiguration();
            Initialize();
        }

        /// <summary>Initializes a new instance of the <see cref="ThreadPool" /> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="isReadOnly">if set to <c>true</c> [is read only].</param>
        public ThreadPool(string name, bool isReadOnly)
        {
            if (string.IsNullOrWhiteSpace(name)) ThrowHelper.ThrowArgumentNullException("name");
            mName = name;
            OpenConfiguration();
            mIsReadOnly = isReadOnly;
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPool"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="minThreadNumber">The min thread number.</param>
        /// <param name="maxThreadNumber">The max thread number.</param>
        public ThreadPool(string name, int minThreadNumber, int maxThreadNumber)
        {
            if (string.IsNullOrWhiteSpace(name)) ThrowHelper.ThrowArgumentNullException("name");

            mName = name;
            OpenConfiguration();
            if (minThreadNumber >= Environment.ProcessorCount)
            {
                mMinThreadNumber = minThreadNumber;
            }
            if (maxThreadNumber >= Environment.ProcessorCount)
            {
                MaxThreadNumber = maxThreadNumber;
                if (mMaxThreadNumber < mMinThreadNumber)
                {
                    mMaxThreadNumber = mMinThreadNumber;
                }
            }
            Initialize();
        }

        /// <summary>Initializes a new instance of the <see cref="ThreadPool" /> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="minThreadNumber">The minimum thread number.</param>
        /// <param name="maxThreadNumber">The maximum thread number.</param>
        /// <param name="isReadOnly">if set to <c>true</c> [is read only].</param>
        public ThreadPool(string name, int minThreadNumber, int maxThreadNumber, bool isReadOnly)
        {
            if (string.IsNullOrWhiteSpace(name)) ThrowHelper.ThrowArgumentNullException("name");

            mName = name;
            OpenConfiguration();
            if (minThreadNumber >= Environment.ProcessorCount)
            {
                mMinThreadNumber = minThreadNumber;
            }
            if (maxThreadNumber >= Environment.ProcessorCount)
            {
                MaxThreadNumber = maxThreadNumber;
                if (mMaxThreadNumber < mMinThreadNumber)
                {
                    mMaxThreadNumber = mMinThreadNumber;
                }
            }
            mIsReadOnly = isReadOnly;
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPool"/> class.
        /// </summary>
        /// <param name="minThreadNumber">The min thread number.</param>
        /// <param name="maxThreadNumber">The max thread number.</param>
        public ThreadPool(int minThreadNumber, int maxThreadNumber)
        {
            OpenConfiguration();
            if (minThreadNumber >= Environment.ProcessorCount)
            {
                mMinThreadNumber = minThreadNumber;
            }
            if (maxThreadNumber >= Environment.ProcessorCount)
            {
                MaxThreadNumber = maxThreadNumber;
                if (mMaxThreadNumber < mMinThreadNumber)
                {
                    mMaxThreadNumber = mMinThreadNumber;
                }
            }
            Initialize();
        }

        /// <summary>Initializes a new instance of the <see cref="ThreadPool" /> class.</summary>
        /// <param name="minThreadNumber">The minimum thread number.</param>
        /// <param name="maxThreadNumber">The maximum thread number.</param>
        /// <param name="isReadOnly">if set to <c>true</c> [is read only].</param>
        public ThreadPool(int minThreadNumber, int maxThreadNumber, bool isReadOnly)
        {
            OpenConfiguration();
            if (minThreadNumber >= Environment.ProcessorCount)
            {
                mMinThreadNumber = minThreadNumber;
            }
            if (maxThreadNumber >= Environment.ProcessorCount)
            {
                MaxThreadNumber = maxThreadNumber;
                if (mMaxThreadNumber < mMinThreadNumber)
                {
                    mMaxThreadNumber = mMinThreadNumber;
                }
            }
            mIsReadOnly = isReadOnly;
            Initialize();
        }

        /// <summary>Initializes a new instance of the <see cref="ThreadPool" /> class.</summary>
        /// <param name="options">The options.</param>
        public ThreadPool(ThreadPoolOptions options) : this(options.MinThreadNumber, options.MaxThreadNumber)
        {
            ShutDownIdleThreadTime = options.ShutDownIdleThreadTime;
            Initialize();
        }

#if NET40
#else
        /// <summary>Initializes a new instance of the <see cref="ThreadPool" /> class.</summary>
        /// <param name="options">The options.</param>
        public ThreadPool(Microsoft.Extensions.Options.IOptions<ThreadPoolOptions> options)
        {
            if (options.Value.MinThreadNumber >= Environment.ProcessorCount)
            {
                mMinThreadNumber = options.Value.MinThreadNumber;
            }
            if (options.Value.MaxThreadNumber >= Environment.ProcessorCount)
            {
                MaxThreadNumber = options.Value.MaxThreadNumber;
                if (mMaxThreadNumber < mMinThreadNumber)
                {
                    mMaxThreadNumber = mMinThreadNumber;
                }
            }
            ShutDownIdleThreadTime = options.Value.ShutDownIdleThreadTime;
            Initialize();
        }
#endif

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ThreadPool"/> is reclaimed by garbage collection.
        /// </summary>
        ~ThreadPool()
        {
            Dispose(false);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DebuggerHidden]
        public string Name
        {
            get { return mName; }
        }

        /// <summary>
        /// Gets or sets the min thread number.
        /// </summary>
        /// <value>
        /// The min thread number.
        /// </value>
        [DebuggerHidden]
        public int MinThreadNumber
        {
            get { DoDisposeCheck(); return mMinThreadNumber; }
            set
            {
                DoDisposeCheck();
                if (!IsReadOnly)
                {
                    if (value >= Environment.ProcessorCount && value <= mMaxThreadNumber)
                    {
                        if (value > mMinThreadNumber)
                        {
                            lock (mLockObject)
                            {
                                for (int i = mMinThreadNumber; i < value; i++)
                                {
                                    StartNewThread();
                                }
                                mMinThreadNumber = value;
                            }
                        }
                        else
                        {
                            mMinThreadNumber = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the max thread number.
        /// </summary>
        /// <value>
        /// The max thread number.
        /// </value>
        [DebuggerHidden]
        public int MaxThreadNumber
        {
            get { DoDisposeCheck(); return mMaxThreadNumber; }
            set
            {
                DoDisposeCheck();
                if (!IsReadOnly)
                {
                    if (value > Environment.ProcessorCount && value >= mMinThreadNumber)
                    {
                        if (value < mMaxThreadNumber)
                        {
                            // less number of thread allowed
                            lock (mLockObject)
                            {
                                if (mInactiveThreads.Count > 0)
                                {
                                    List<WorkerThread> wtList = new List<WorkerThread>(mInactiveThreads);
                                    for (int i = value; i < mMaxThreadNumber; i++)
                                    {
                                        if (wtList.Count == 0)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            wtList[0].WakeUpEvent.Set();
                                            wtList.RemoveAt(0);
                                        }
                                    }
                                }
                            }
                        }
                        mMaxThreadNumber = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the shut down idle thread time.
        /// </summary>
        /// <value>
        /// The shut down idle thread time.
        /// </value>
        [DebuggerHidden]
        public int ShutDownIdleThreadTime
        {
            get { return mShutDownIdleThreadTime; }
            set
            {
                DoDisposeCheck();
                if (value > 0 && !IsReadOnly)
                {
                    mShutDownIdleThreadTime = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        [DebuggerHidden]
        public bool IsReadOnly
        {
            [DebuggerStepThrough]
            get { DoDisposeCheck(); return mIsReadOnly; }
        }

        #endregion

        #region Public method(s)

        /// <summary>Configures the specified property item.</summary>
        /// <param name="propertyItem">The property item.</param>
        /// <exception cref="System.ArgumentNullException">propertyItem</exception>
        [DebuggerStepThrough]
        public void Configure(IPropertyItem propertyItem)
        {
            if (propertyItem == null) throw new ArgumentNullException("propertyItem");

            if (!mIsReadOnly)
            {
                int intValue = 0;
                if (ConfigurationAccessHelper.ParseIntValue(propertyItem, "MinThreadNumber", Environment.ProcessorCount, int.MaxValue, ref intValue))
                {
                    mMinThreadNumber = intValue;
                }
                if (ConfigurationAccessHelper.ParseIntValue(propertyItem, "MaxThreadNumber", Environment.ProcessorCount, int.MaxValue, ref intValue))
                {
                    mMaxThreadNumber = intValue;
                }
                if (ConfigurationAccessHelper.ParseIntValue(propertyItem, "ShutDownIdleThreadTime", -1, int.MaxValue, ref intValue))
                {
                    if (intValue == -1 || intValue >= 1000)
                    {
                        mShutDownIdleThreadTime = intValue;
                    }
                }
                if (mMaxThreadNumber < mMinThreadNumber)
                {
                    mMaxThreadNumber = mMinThreadNumber;
                }
            }
        }

        /// <summary>Configures the specified options.</summary>
        /// <param name="options">The options.</param>
        /// <exception cref="System.ArgumentNullException">options</exception>
        public void Configure(ThreadPoolOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");

            if (!mIsReadOnly)
            {
                MaxThreadNumber = options.MaxThreadNumber;
                MinThreadNumber = options.MinThreadNumber;
                ShutDownIdleThreadTime = options.ShutDownIdleThreadTime;
            }
        }

        /// <summary>Gets the number of existing threads in the threadpool</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        [DebuggerStepThrough]
        public int GetThreadCount()
        {
            lock (mLockObject)
            {
                return mExistingThreads.Count;
            }
        }

        /// <summary>
        /// Queues the user work item.
        /// </summary>
        /// <param name="callBack">The call back.</param>
        [DebuggerStepThrough]
        public void QueueUserWorkItem(WaitCallback callBack)
        {
            QueueUserWorkItem(callBack, null);
        }

        /// <summary>
        /// Queues the user work item.
        /// </summary>
        /// <param name="callBack">The call back.</param>
        /// <param name="state">The state.</param>
        [DebuggerStepThrough]
        public void QueueUserWorkItem(WaitCallback callBack, object state)
        {
            DoDisposeCheck();
            if (callBack == null) ThrowHelper.ThrowArgumentNullException("callBack");
            lock (mLockObject)
            {
                DoDisposeCheck();
                if (LOGGER.IsDebugEnabled) LOGGER.Debug(string.Format("THREADPOOL({0}): enqueue new task for execution. Currently waiting task(s) in the queue: {1}", string.IsNullOrEmpty(mName) ? GetHashCode().ToString() : mName, mTaskContainerQueue.Count));

                TaskContainer tc = new TaskContainer() { WaitCallback = callBack, State = state };
                WorkerThread wt = null;
                if (mInactiveThreads.Count > 0)
                {
                    foreach (WorkerThread _wt in mInactiveThreads)
                    {
                        if (_wt.Task == null &&
                            _wt.Thread.ThreadState != System.Threading.ThreadState.Aborted &&
                            _wt.Thread.ThreadState != System.Threading.ThreadState.Stopped)
                        {
                            wt = _wt;
                        }
                    }
                }
                else if (mThreadNumber < mMaxThreadNumber)
                {
                    wt = StartNewThread();
                }
                if (wt == null)
                {
                    // no free worker thread
                    mTaskContainerQueue.Enqueue(tc);
                    if (mMaxThreadNumber > mThreadNumber)
                    {
                        // we are full, starting a new thread
                        StartNewThread();
                    }
                }
                else
                {
                    // assign to asap it to somebody
                    mInactiveThreads.Remove(wt);
                    wt.Task = tc;
                    wt.WakeUpEvent.Set();
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private helpers

        private void DoDisposeCheck()
        {
            if (mDisposed) throw new ObjectDisposedException(GetType().FullName);
        }

        private void Dispose(bool disposing)
        {
            mDisposed = true;
            if (disposing)
            {
                int maxItemNumber = 64;
                List<WorkerThread> list = new List<WorkerThread>();
                List<EventWaitHandle> shutdownEvents = new List<EventWaitHandle>();
                List<List<EventWaitHandle>> seList = new List<List<EventWaitHandle>>();
                seList.Add(shutdownEvents);
                lock (mLockObject)
                {
                    foreach (WorkerThread wt in mExistingThreads)
                    {
                        if (wt.Thread.ThreadState != System.Threading.ThreadState.Aborted &&
                            wt.Thread.ThreadState != System.Threading.ThreadState.Stopped)
                        {
                            wt.IsShutdownForce = true;
                            wt.WakeUpEvent.Set();
                            list.Add(wt);
                            if (shutdownEvents.Count == maxItemNumber)
                            {
                                shutdownEvents = new List<EventWaitHandle>();
                                seList.Add(shutdownEvents);
                            }
                            shutdownEvents.Add(wt.ShutdownEvent);
                        }
                    }
                }
                foreach (List<EventWaitHandle> l in seList)
                {
                    if (l.Count > 0)
                    {
                        if (WaitHandle.WaitAll(l.ToArray(), 5000))
                        {
                            l.ForEach(i => i.Close());
                        }
                        l.Clear();
                    }
                }
                seList.Clear();
                if (list.Count > 0)
                {
                    list.ForEach(i => i.Dispose());
                    list.Clear();
                }
            }
        }

        private void OpenConfiguration()
        {
            if (ThreadPoolConfiguration.SectionHandler.RestartOnExternalChanges)
            {
                ThreadPoolConfiguration.SectionHandler.OnConfigurationChanged += new EventHandler<EventArgs>(SectionHandler_OnConfigurationChanged);
            }
            SectionHandler_OnConfigurationChanged(null, null);
        }

        private void SectionHandler_OnConfigurationChanged(object sender, EventArgs e)
        {
            if (LOGGER.IsInfoEnabled) LOGGER.Info(string.Format("THREADPOOL[{0}]: reading values from configuration...", string.IsNullOrEmpty(mName) ? GetHashCode().ToString() : mName));
            foreach (ThreadPoolItem item in ThreadPoolConfiguration.Settings.ThreadPools)
            {
                if (mName.Equals(item.Name) || string.IsNullOrEmpty(item.Name))
                {
                    if (item.MinThreadNumber >= Environment.ProcessorCount)
                    {
                        mMinThreadNumber = item.MinThreadNumber;
                    }
                    if (item.MaxThreadNumber >= Environment.ProcessorCount)
                    {
                        mMaxThreadNumber = item.MaxThreadNumber;
                    }
                    if (item.ShutDownIdleThreadTime == -1 || item.ShutDownIdleThreadTime >= 1000)
                    {
                        mShutDownIdleThreadTime = item.ShutDownIdleThreadTime;
                    }
                    mIsReadOnly = item.SetReadOnlyFlag;
                    if (mMaxThreadNumber < mMinThreadNumber)
                    {
                        mMaxThreadNumber = mMinThreadNumber;
                    }

                    if (item.Name.Equals(Name))
                    {
                        // stop the searching, if I found my configuration
                        break;
                    }
                }
            }
        }

        private void Initialize()
        {
            if (mMinThreadNumber > 0)
            {
                for (int i = 0; i < mMinThreadNumber; i++)
                {
                    StartNewThread();
                }
            }
            if (LOGGER.IsInfoEnabled) LOGGER.Info(string.Format("THREADPOOL: initialized, Name: {0}, MinThreadNumber: {1}, MaxThreadNumber: {2}, ShutDownIdleThreadTime: {3}, IsReadOnly: {4}", string.IsNullOrEmpty(mName) ? GetHashCode().ToString() : mName, MinThreadNumber, MaxThreadNumber, ShutDownIdleThreadTime, IsReadOnly.ToString()));
        }

        private WorkerThread StartNewThread()
        {
            Thread thread = new Thread(new ParameterizedThreadStart(WorkerThreadMain));
            thread.IsBackground = true;
            thread.Name = string.Format("ThreadPool({0})-Thread{1}", string.IsNullOrEmpty(mName) ? GetHashCode().ToString() : mName, Interlocked.Increment(ref mThreadId));
            WorkerThread w = new WorkerThread(thread);
            mInactiveThreads.Add(w);
            mExistingThreads.Add(w);
            Interlocked.Increment(ref mThreadNumber);
            thread.Start(w);
            return w;
        }

        private void WorkerThreadMain(object state)
        {
            WorkerThread wt = (WorkerThread)state;

            if (LOGGER.IsDebugEnabled) LOGGER.Debug(string.Format("THREADPOOL[{0}]: starting new ThreadPool thread ({1}).", string.IsNullOrEmpty(mName) ? GetHashCode().ToString() : mName, Thread.CurrentThread.Name));

            try
            {
                while (!mDisposed && !wt.IsShutdownForce)
                {
                    if (wt.Task == null)
                    {
                        lock (mLockObject)
                        {
                            if (!mDisposed && !wt.IsShutdownForce)
                            {
                                // no stop
                                if (wt.Task == null)
                                {
                                    // no assigned task
                                    if (mMaxThreadNumber < mThreadNumber)
                                    {
                                        // too many threads exist, I have no task assigned, stop...
                                        mInactiveThreads.Remove(wt);
                                        break;
                                    }
                                    else if (mTaskContainerQueue.Count > 0)
                                    {
                                        // has unassigned task(s), request one...
                                        wt.Task = mTaskContainerQueue.Dequeue();
                                        mInactiveThreads.Remove(wt);
                                    }
                                }
                            }
                            else
                            {
                                // shutdown in progress...
                                mInactiveThreads.Remove(wt);
                                break;
                            }
                        }
                    }
                    if (wt.Task != null || wt.WakeUpEvent.WaitOne(mShutDownIdleThreadTime))
                    {
                        // I got a signal or a task assigned to me
                        if (wt.Task != null && !mDisposed && !wt.IsShutdownForce)
                        {
                            Execute(wt);
                        }
                    }
                    else
                    {
                        // I have no signal
                        if (!mDisposed && !wt.IsShutdownForce)
                        {
                            lock (mLockObject)
                            {
                                if (wt.Task == null && mMinThreadNumber > mThreadNumber)
                                {
                                    // not task assigned to me, shut down....
                                    mInactiveThreads.Remove(wt);
                                    break;
                                }
                            }
                            if (wt.Task != null && !mDisposed && !wt.IsShutdownForce)
                            {
                                // I got a task in the meantime...
                                Execute(wt);
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }

            lock (mLockObject)
            {
                Interlocked.Decrement(ref mThreadNumber);
                mInactiveThreads.Remove(wt);
                mExistingThreads.Remove(wt);
                if (wt.IsShutdownForce)
                {
                    // shut down command in sight, shut down
                    wt.ShutdownEvent.Set();
                }
                else
                {
                    wt.Dispose();
                }
            }

            if (LOGGER.IsDebugEnabled) LOGGER.Debug(string.Format("THREADPOOL[{0}]: thread ({1}) has exited.", string.IsNullOrEmpty(mName) ? GetHashCode().ToString() : mName, Thread.CurrentThread.Name));
        }

        private void Execute(WorkerThread wt)
        {
            try
            {
                TaskContainer tc = wt.Task;
                wt.Task = null;
                tc.WaitCallback.Invoke(tc.State);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (LOGGER.IsErrorEnabled) LOGGER.Error(string.Format("THREADPOOL[{0}]: an exception was thrown while executing a queued task. Exception: {1}", string.IsNullOrEmpty(mName) ? GetHashCode().ToString() : mName, ex.ToString()));
            }
            lock (mLockObject)
            {
                mInactiveThreads.Add(wt);
            }
        }

        #endregion

        #region Nested class(es)

        private class TaskContainer
        {

            /// <summary>
            /// Gets or sets the wait callback.
            /// </summary>
            /// <value>
            /// The wait callback.
            /// </value>
            internal WaitCallback WaitCallback { get; set; }

            /// <summary>
            /// Gets or sets the state.
            /// </summary>
            /// <value>
            /// The state.
            /// </value>
            internal object State { get; set; }

            /// <summary>
            /// Gets or sets the worker thread.
            /// </summary>
            /// <value>
            /// The worker thread.
            /// </value>
            internal WorkerThread WorkerThread { get; set; }

        }

        private class WorkerThread : IDisposable
        {

            /// <summary>
            /// Initializes a new instance of the <see cref="WorkerThread"/> class.
            /// </summary>
            /// <param name="thread">The thread.</param>
            internal WorkerThread(Thread thread)
            {
                Thread = thread;
                WakeUpEvent = new AutoResetEvent(false);
                ShutdownEvent = new ManualResetEvent(false);
            }

            /// <summary>
            /// Gets or sets the thread.
            /// </summary>
            /// <value>
            /// The thread.
            /// </value>
            internal Thread Thread { get; private set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is shutdown force.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is shutdown force; otherwise, <c>false</c>.
            /// </value>
            internal bool IsShutdownForce { get; set; }

            /// <summary>
            /// Gets or sets the wake up event.
            /// </summary>
            /// <value>
            /// The wake up event.
            /// </value>
            internal AutoResetEvent WakeUpEvent { get; private set; }

            /// <summary>
            /// Gets or sets the shutdown event.
            /// </summary>
            /// <value>
            /// The shutdown event.
            /// </value>
            internal ManualResetEvent ShutdownEvent { get; private set; }

            /// <summary>
            /// Gets or sets the task.
            /// </summary>
            /// <value>
            /// The task.
            /// </value>
            internal TaskContainer Task { get; set; }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
            /// </summary>
            /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
            /// <returns>
            ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
            /// </returns>
            [MethodImpl(MethodImplOptions.Synchronized)]
            public override bool Equals(object obj)
            {
                if (Thread == null)
                {
                    return false;
                }
                return Thread.Name.Equals(((WorkerThread)obj).Thread.Name);
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            [MethodImpl(MethodImplOptions.Synchronized)]
            public void Dispose()
            {
                Thread = null;
                WakeUpEvent.Close();
                ShutdownEvent.Close();
            }

        }

        #endregion

    }

}
