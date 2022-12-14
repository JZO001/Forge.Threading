/* *********************************************************************
 * Date: 28 Feb 2013
 * Created by: Zoltan Juhasz
 * E-Mail: forge@jzo.hu
***********************************************************************/

using System;
using System.Threading;

namespace Forge.Threading.Tasking
{

    /// <summary>
    /// Task result
    /// </summary>
    [Serializable]
    public class TaskResult : ITaskResult
    {

#if NET6_0_OR_GREATER
#nullable enable
#endif

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskResult"/> class.
        /// </summary>
        /// <param name="inParameters">The in parameters.</param>
        public TaskResult(object[] inParameters)
        {
            InParameters = inParameters;
        }

        #endregion

        #region Public properties

        /// <summary>Gets a value indicating whether this instance is completed.</summary>
        /// <value>
        ///   <c>true</c> if this instance is completed; otherwise, <c>false</c>.</value>
        public bool IsCompleted { get; internal set; }

        /// <summary>Gets the wait handle.</summary>
        /// <value>The wait handle.</value>
        public ManualResetEvent WaitHandle { get; private set; } = new ManualResetEvent(false);

        /// <summary>
        /// Gets the in parameters.
        /// </summary>
        /// <value>
        /// The in parameters.
        /// </value>
        public object[]
#if NET6_0_OR_GREATER
            ? 
#endif
            InParameters { get; private set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception
#if NET6_0_OR_GREATER
            ? 
#endif
            Exception { get; internal set; }

        /// <summary>Gets the provided state object.</summary>
        /// <value>The state.</value>
        public object
#if NET6_0_OR_GREATER
            ? 
#endif
            AsyncState { get; internal set; }

        /// <summary>Gets the result.</summary>
        /// <value>The result.</value>
        public object
#if NET6_0_OR_GREATER
            ? 
#endif
            Result { get; internal set; }

        #endregion

#if NET6_0_OR_GREATER
#nullable disable
#endif

    }

}
