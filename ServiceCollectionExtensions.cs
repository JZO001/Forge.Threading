#if NET40
#else

using System;
using Forge.Threading.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Forge.Threading
{

    /// <summary>ServiceCollection extension methods</summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Registers the Forge Log4Net logging
        /// </summary>
        /// <returns>IServiceCollection</returns>
#if NET461 || NETSTANDARD2_0
        public static IServiceCollection AddForgeThreading(this IServiceCollection services, Action<ThreadPoolOptions> configure)
#else
#nullable enable
        public static IServiceCollection AddForgeThreading(this IServiceCollection services, Action<ThreadPoolOptions>? configure)
#nullable disable
#endif
        {
            return services
                .AddSingleton<IThreadPool, ThreadPool>()
                .Configure<ThreadPoolOptions>(configureOptions => 
                {
                    configure?.Invoke(configureOptions);
                });
        }

    }

}

#endif
