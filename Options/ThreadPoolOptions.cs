using System;

namespace Forge.Threading.Options
{

    /// <summary>ThreadPool options</summary>
    public class ThreadPoolOptions
    {

        /// <summary>Gets or sets the minimum thread number.</summary>
        /// <value>The minimum thread number.</value>
        public int MinThreadNumber { get; set; } = Environment.ProcessorCount;

        /// <summary>Gets or sets the maximum thread number.</summary>
        /// <value>The maximum thread number.</value>
        public int MaxThreadNumber { get; set; } = 32768;

        /// <summary>Gets or sets the shut down idle thread time.</summary>
        /// <value>The shut down idle thread time.</value>
        public int ShutDownIdleThreadTime { get; set; } = 120000;

    }

}
