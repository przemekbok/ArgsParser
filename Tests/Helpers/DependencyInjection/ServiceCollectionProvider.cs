using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Helpers.DependencyInjection
{
    public class ServiceCollectionProvider
    {
        private IServiceCollection Services { get; set; }

        public ServiceCollectionProvider()
        {
            Host
                .CreateDefaultBuilder(new string[0])
                .ConfigureServices(services =>
                {
                    Services = services;
                })
                .Build();
        }

        public IServiceCollection GetServices()
        {
            Services.Clear();
            return Services;
        }
    }
}
