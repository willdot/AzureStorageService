using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlobStorageService.Service
{
    public static class ServiceSetup
    {
        public static void SetupService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAzureProvider, AzureProvider>();
        }
    }
}