using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Helpers.DependencyInjection
{
    public class ServiceCollectionProvider
    {
        private HostBuilderContext HostBuilder;
        private IServiceCollection Services;
        
        public ServiceCollectionProvider(string[] args)
        {
            Host
                .CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilder,services) =>
                {
                    HostBuilder = hostBuilder;
                    Services = services;
                })
                .Build();
        }

        public IServiceCollection GetServices()
        {
            Services.Clear();
            return Services;
        }

        public HostBuilderContext GetHostBuilder()
        {
            return HostBuilder;
        }
    }
}
