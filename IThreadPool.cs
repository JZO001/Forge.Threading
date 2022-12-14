using Forge.Configuration;
using Forge.Threading.Options;
using System;
using System.Threading;

namespace Forge.Threading
{

    /// <summary>Represents a threadpool</summary>
    public interface IThreadPool : IDisposable
    {

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>Gets or sets the minimum thread number.</summary>
        /// <value>The minimum thread number.</value>
        int MinThreadNumber { get; set; }

        /// <summary>Gets or sets the maximum thread number.</summary>
        /// <value>The maximum thread number.</value>
        int MaxThreadNumber { get; set; }

        /// <summary>Gets or sets the shut down idle thread time.</summary>
        /// <value>The shut down idle thread time.</value>
        int ShutDownIdleThreadTime { get; set; }

        /// <summary>Gets a value indicating whether this instance is read only.</summary>
        /// <value>
        ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        bool IsReadOnly { get; }

        /// <summary>Configures the threadpool with property item.</summary>
        /// <param name="propertyItem">The property item.</param>
        void Configure(IPropertyItem propertyItem);

        /// <summary>Configures the threadpool with options.</summary>
        /// <param name="options">The options.</param>
        void Configure(ThreadPoolOptions options);

        /// <summary>Gets the number of existing threads in the threadpool</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        int GetThreadCount();

        /// <summary>
        /// Queues the user work item.
        /// </summary>
        /// <param name="callBack">The call back.</param>
        void QueueUserWorkItem(WaitCallback callBack);

        /// <summary>
        /// Queues the user work item.
        /// </summary>
        /// <param name="callBack">The call back.</param>
        /// <param name="state">The state.</param>
        void QueueUserWorkItem(WaitCallback callBack, object state);

    }

}
