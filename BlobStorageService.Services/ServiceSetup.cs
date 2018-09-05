using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlobStorageService.Service
{
    /// <summary>
    /// Static class it set up the dependency injection of an AzureProvider.
    /// </summary>
    public static class ServiceSetup
    {
        /// <summary>
        /// Sets up the dependency injection of an AzureProvider.
        /// </summary>
        /// <param name="services">A services instance</param>
        /// <param name="configuration">A configuration instance</param>
        public static void SetupService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAzureProvider, AzureProvider>();
        }
    }
}