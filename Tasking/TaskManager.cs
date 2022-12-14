/* *********************************************************************
 * Date: 28 Feb 2013
 * Created by: Zoltan Juhasz
 * E-Mail: forge@jzo.hu
***********************************************************************/

using Forge.Configuration;
using Forge.Legacy;
using Forge.Logging.Abstraction;
using Forge.Reflection;
using Forge.Shared;
using Forge.Threading.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Forge.Threading.Tasking
{

    /// <summary>
    /// Executes a task and than call back with the result
    /// </summary>
    public sealed class TaskManager : MBRBase
    {

        #region Field(s)

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(TaskManager));

        private static readonly ThreadPool mThreadPool = new ThreadPool("TaskManager_ThreadPool");

        private readonly Dictionary<int, List<QueueItem>> mOrderedItems = new Dictionary<int, List<QueueItem>>();

        private readonly List<QueueItem> mSequentialQueuedItems = null;
        private bool mIsExecuting = false;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskManager"/> class.
        /// </summary>
        public TaskManager()
            : this(ChaosTheoryEnum.OrderByTaskDelegateTarget)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskManager"/> class.
        /// </summary>
        public TaskManager(ChaosTheoryEnum chaosTheory)
        {
            ChaosTheoryMode = chaosTheory;
            if (chaosTheory == ChaosTheoryEnum.Sequential)
            {
                mSequentialQueuedItems = new List<QueueItem>();
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the chaos theory mode.
        /// </summary>
        /// <value>
        /// The chaos theory mode.
        /// </value>
        public ChaosTheoryEnum ChaosTheoryMode { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [invoke UI at action].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [invoke UI at action]; otherwise, <c>false</c>.
        /// </value>
        public bool InvokeUIAtAction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [invoke UI at return].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [invoke UI at return]; otherwise, <c>false</c>.
        /// </value>
        public bool InvokeUIAtReturn { get; set; }

        #endregion

        #region Public method(s)

        /// <summary>Configures the thread pool.</summary>
        /// <param name="propertyItem">The property item.</param>
        [DebuggerStepThrough]
        public static void ConfigureThreadPool(IPropertyItem propertyItem)
        {
            mThreadPool.Configure(propertyItem);
        }

        /// <summary>Configures the thread pool.</summary>
        /// <param name="options">The options.</param>
        [DebuggerStepThrough]
        public static void ConfigureThreadPool(ThreadPoolOptions options)
        {
            mThreadPool.Configure(options);
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute(System.Action taskDelegate, ReturnCallback returnDelegate)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute(System.Action taskDelegate, ReturnCallback returnDelegate, object state)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1>(System.Action<T1> taskDelegate, ReturnCallback returnDelegate, T1 p1)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1>(System.Action<T1> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2>(System.Action<T1, T2> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2>(System.Action<T1, T2> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3>(System.Action<T1, T2, T3> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3>(System.Action<T1, T2, T3> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4>(System.Action<T1, T2, T3, T4> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3, p4 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4>(System.Action<T1, T2, T3, T4> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3, p4 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5>(System.Action<T1, T2, T3, T4, T5> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3, p4, p5 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5>(System.Action<T1, T2, T3, T4, T5> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3, p4, p5 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6>(System.Action<T1, T2, T3, T4, T5, T6> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3, p4, p5, p6 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6>(System.Action<T1, T2, T3, T4, T5, T6> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3, p4, p5, p6 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6, T7>(System.Action<T1, T2, T3, T4, T5, T6, T7> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3, p4, p5, p6, p7 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6, T7>(System.Action<T1, T2, T3, T4, T5, T6, T7> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3, p4, p5, p6, p7 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="T8">The type of the 8.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        /// <param name="p8">The p8.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6, T7, T8>(System.Action<T1, T2, T3, T4, T5, T6, T7, T8> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3, p4, p5, p6, p7, p8 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="T8">The type of the 8.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        /// <param name="p8">The p8.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6, T7, T8>(System.Action<T1, T2, T3, T4, T5, T6, T7, T8> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3, p4, p5, p6, p7, p8 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<TResult>(System.Func<TResult> taskDelegate, ReturnCallback returnDelegate)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<TResult>(System.Func<TResult> taskDelegate, ReturnCallback returnDelegate, object state)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, TResult>(System.Func<T1, TResult> taskDelegate, ReturnCallback returnDelegate, T1 p1)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, TResult>(System.Func<T1, TResult> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, TResult>(System.Func<T1, T2, TResult> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, TResult>(System.Func<T1, T2, TResult> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, TResult>(System.Func<T1, T2, T3, TResult> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, TResult>(System.Func<T1, T2, T3, TResult> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, TResult>(System.Func<T1, T2, T3, T4, TResult> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3, p4 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        public ITaskResult Execute<T1, T2, T3, T4, TResult>(System.Func<T1, T2, T3, T4, TResult> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3, p4 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, TResult>(System.Func<T1, T2, T3, T4, T5, TResult> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3, p4, p5 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, TResult>(System.Func<T1, T2, T3, T4, T5, TResult> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3, p4, p5 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6, TResult>(System.Func<T1, T2, T3, T4, T5, T6, TResult> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3, p4, p5, p6 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6, TResult>(System.Func<T1, T2, T3, T4, T5, T6, TResult> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3, p4, p5, p6 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6, T7, TResult>(System.Func<T1, T2, T3, T4, T5, T6, T7, TResult> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3, p4, p5, p6, p7 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6, T7, TResult>(System.Func<T1, T2, T3, T4, T5, T6, T7, TResult> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3, p4, p5, p6, p7 });
        }

        /// <summary>
        /// Executes the specified task delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="T8">The type of the 8.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        /// <param name="p8">The p8.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(System.Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> taskDelegate, ReturnCallback returnDelegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return Execute(taskDelegate, returnDelegate, null, new object[] { p1, p2, p3, p4, p5, p6, p7, p8 });
        }

        /// <summary>Executes the specified task delegate.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="T8">The type of the 8.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="taskDelegate">The task delegate.</param>
        /// <param name="returnDelegate">The return delegate.</param>
        /// <param name="state">The state.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        /// <param name="p8">The p8.</param>
        [DebuggerStepThrough]
        public ITaskResult Execute<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(System.Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> taskDelegate, ReturnCallback returnDelegate, object state, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return Execute(taskDelegate, returnDelegate, state, new object[] { p1, p2, p3, p4, p5, p6, p7, p8 });
        }

        #endregion

        #region Private method(s)

        private ITaskResult Execute(Delegate taskDelegate, Delegate returnDelegate, object state, params object[] inParameters)
        {
            if (taskDelegate == null)
            {
                ThrowHelper.ThrowArgumentNullException("taskDelegate");
            }
            if (returnDelegate == null && ChaosTheoryMode == ChaosTheoryEnum.OrderByReturnDelegateTarget)
            {
                ThrowHelper.ThrowArgumentNullException("returnDelegate");
            }

            QueueItem item = new QueueItem() { TaskDelegate = taskDelegate, ReturnDelegate = returnDelegate, InParameters = inParameters };

            TaskResult result = new TaskResult(item.InParameters);
            result.AsyncState = state;
            item.Result = result;

            switch (ChaosTheoryMode)
            {
                case ChaosTheoryEnum.Chaos:
                    {
                        mThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ExecuteTask), item);
                    }
                    break;

                case ChaosTheoryEnum.OrderByTaskDelegateTarget:
                    {
                        BeginExecution(taskDelegate.Target.GetHashCode(), item);
                    }
                    break;

                case ChaosTheoryEnum.OrderByReturnDelegateTarget:
                    {
                        BeginExecution(returnDelegate.Target.GetHashCode(), item);
                    }
                    break;

                case ChaosTheoryEnum.Sequential:
                    {
                        BeginExecution(item);
                    }
                    break;
            }

            return result;
        }

        private void BeginExecution(int targetHash, QueueItem item)
        {
            lock (mOrderedItems)
            {
                List<QueueItem> list = null;
                if (mOrderedItems.ContainsKey(targetHash))
                {
                    list = mOrderedItems[targetHash];
                }
                else
                {
                    list = new List<QueueItem>();
                    mOrderedItems[targetHash] = list;
                }

                list.Add(item);
                if (list.Count == 1)
                {
                    mThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ExecuteTask), item);
                }
            }
        }

        private void BeginExecution(QueueItem item)
        {
            lock (mSequentialQueuedItems)
            {
                mSequentialQueuedItems.Add(item);
                if (!mIsExecuting)
                {
                    mIsExecuting = true;
                    mThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ExecuteTask), item);
                }
            }
        }

        private void ExecuteTask(object state)
        {
            QueueItem item = state as QueueItem;
            Exception exception = null;
            object methodResult = null;

            try
            {
                if (InvokeUIAtAction && UIReflectionHelper.IsObjectWinFormsControl(item.TaskDelegate.Target))
                {
                    methodResult = UIReflectionHelper.InvokeOnWinFormsControl(item.TaskDelegate.Target, item.TaskDelegate, item.InParameters); //((Control)item.TaskDelegate.Target).Invoke(item.TaskDelegate, item.InParameters);
                }
                else if (InvokeUIAtAction && UIReflectionHelper.IsObjectWPFDependency(item.TaskDelegate.Target))
                {
                    //DependencyObject ctrl = (DependencyObject)item.TaskDelegate.Target;
                    //methodResult = ctrl.Dispatcher.Invoke(item.TaskDelegate, item.InParameters);
                    methodResult = UIReflectionHelper.InvokeOnWPFDependency(item.TaskDelegate.Target, item.TaskDelegate, item.InParameters);
                }
                else
                {
                    methodResult = item.TaskDelegate.Method.Invoke(item.TaskDelegate.Target, item.InParameters);
                }
            }
            catch (TargetInvocationException ex)
            {
                exception = ex.InnerException;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            TaskResult result = item.Result;
            result.Exception = exception;
            if (!item.TaskDelegate.Method.ReturnType.Equals(typeof(void)))
            {
                result.Result = methodResult;
            }
            result.IsCompleted = true;
            result.WaitHandle.Set();

            if (item.ReturnDelegate != null)
            {
                try
                {
                    if (InvokeUIAtReturn && UIReflectionHelper.IsObjectWinFormsControl(item.ReturnDelegate.Target))
                    {
                        //((Control)item.ReturnDelegate.Target).Invoke(item.ReturnDelegate, new object[] { result });
                        UIReflectionHelper.InvokeOnWinFormsControl(item.ReturnDelegate.Target, item.ReturnDelegate, new object[] { result });
                    }
                    else if (InvokeUIAtReturn && UIReflectionHelper.IsObjectWPFDependency(item.ReturnDelegate.Target))
                    {
                        //DependencyObject ctrl = (DependencyObject)item.ReturnDelegate.Target;
                        //ctrl.Dispatcher.Invoke(item.ReturnDelegate, new object[] { result });
                        UIReflectionHelper.InvokeOnWPFDependency(item.ReturnDelegate.Target, item.ReturnDelegate, new object[] { result });
                    }
                    else
                    {
                        item.ReturnDelegate.Method.Invoke(item.ReturnDelegate.Target, new object[] { result });
                    }
                }
                catch (Exception e)
                {
                    if (LOGGER.IsErrorEnabled) LOGGER.Error(e.Message, e);
                }
            }

            if (ChaosTheoryMode == ChaosTheoryEnum.Sequential)
            {
                lock (mSequentialQueuedItems)
                {
                    mSequentialQueuedItems.RemoveAt(0);
                    if (mSequentialQueuedItems.Count > 0)
                    {
                        mThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ExecuteTask), mSequentialQueuedItems[0]);
                    }
                    else
                    {
                        mIsExecuting = false;
                    }
                }
            }
            else if (ChaosTheoryMode != ChaosTheoryEnum.Chaos)
            {
                lock (mOrderedItems)
                {
                    int hashCode = ChaosTheoryMode == ChaosTheoryEnum.OrderByTaskDelegateTarget ? item.TaskDelegate.Target.GetHashCode() : item.ReturnDelegate.Target.GetHashCode();
                    List<QueueItem> list = mOrderedItems[hashCode];
                    list.RemoveAt(0);
                    if (list.Count > 0)
                    {
                        mThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ExecuteTask), list[0]);
                    }
                }
            }
        }

        #endregion

        #region Nested class(es)

        private sealed class QueueItem
        {

            /// <summary>
            /// Gets or sets the task delegate.
            /// </summary>
            /// <value>
            /// The task delegate.
            /// </value>
            public Delegate TaskDelegate { get; set; }

            /// <summary>
            /// Gets or sets the return delegate.
            /// </summary>
            /// <value>
            /// The return delegate.
            /// </value>
            public Delegate ReturnDelegate { get; set; }

            /// <summary>
            /// Gets or sets the in parameters.
            /// </summary>
            /// <value>
            /// The in parameters.
            /// </value>
            public object[] InParameters { get; set; }

            /// <summary>Gets or sets the result.</summary>
            /// <value>The result.</value>
            public TaskResult Result { get; set; }

        }

        #endregion

    }

}
