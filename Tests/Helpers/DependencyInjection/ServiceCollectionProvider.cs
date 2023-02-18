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
        
        public ServiceCollectionProvider()
        {
            Host
                .CreateDefaultBuilder(new string[0])
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

        public HostBuilderContext GetHostBuilderWithArgs(string[] args)
        {
            var hostBuilderWithArgs = HostBuilder;

            Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((hostBuilder, services) =>
            {
                hostBuilderWithArgs = hostBuilder;
            })
            .Build();

            return hostBuilderWithArgs;
        }

        public HostBuilderContext GetHostBuilder()
        {
            return HostBuilder;
        }
    }
}
