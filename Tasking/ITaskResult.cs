using System;
using System.Threading;

namespace Forge.Threading.Tasking
{

    /// <summary>
    /// Task result
    /// </summary>
    public interface ITaskResult
    {

#if NET6_0_OR_GREATER
#nullable enable
#endif

        /// <summary>Gets a value indicating whether this instance is completed.</summary>
        /// <value>
        ///   <c>true</c> if this instance is completed; otherwise, <c>false</c>.</value>
        bool IsCompleted { get; }

        /// <summary>Gets the wait handle.</summary>
        /// <value>The wait handle.</value>
        ManualResetEvent WaitHandle { get; }

        /// <summary>
        /// Gets the in parameters.
        /// </summary>
        /// <value>
        /// The in parameters.
        /// </value>
        object[]
#if NET6_0_OR_GREATER
            ? 
#endif
            InParameters { get; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        Exception
#if NET6_0_OR_GREATER
            ? 
#endif
            Exception { get; }

        /// <summary>Gets the provided state object.</summary>
        /// <value>The state.</value>
        object
#if NET6_0_OR_GREATER
            ? 
#endif
            AsyncState { get; }

        /// <summary>Gets the result.</summary>
        /// <value>The result.</value>
        object
#if NET6_0_OR_GREATER
            ? 
#endif
            Result { get; }

#if NET6_0_OR_GREATER
#nullable disable
#endif

    }

}
