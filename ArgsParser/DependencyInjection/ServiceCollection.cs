using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArgsParser.DependencyInjection
{
    public static class ServiceCollection
    {
        private static string[]? GetArgsFromHostBuilder(HostBuilderContext hostBuilder)
        {
            var configuration = hostBuilder.Configuration;

            var providers = ((ConfigurationRoot)configuration).Providers;
            var cmdLineConfigProv = providers.Where(provider => provider.GetType() == typeof(CommandLineConfigurationProvider)).First() as CommandLineConfigurationProvider;
            var args = typeof(CommandLineConfigurationProvider).GetProperty("Args", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(cmdLineConfigProv) as string[];

            return args;
        }

        public static IServiceCollection AddConsoleArgumentsDynamicConfig(this IServiceCollection services, HostBuilderContext hostBuilder)
        {
            var args = GetArgsFromHostBuilder(hostBuilder);

            return services.AddConsoleArgumentsDynamicConfig(args ?? new string[0]);
        }

        public static IServiceCollection AddConsoleArgumentsConfig<T>(this IServiceCollection services, HostBuilderContext hostBuilder) where T : class
        {
            var args = GetArgsFromHostBuilder(hostBuilder);
            return services.AddConsoleArgumentsConfig<T>(args ?? new string[0]);
        }

        public static IServiceCollection AddConsoleArgumentsDynamicConfig(this IServiceCollection services, string[] args, List<string> expectedParams = null)
        {
            var argumentsObject = ArgsParser.ParseArguments(args, expectedParams);
            var argObjWrapper = new ConsoleArgumentsWrapper(argumentsObject);
            services.AddSingleton<ConsoleArgumentsWrapper>(argObjWrapper);

            return services;
        }

        public static IServiceCollection AddConsoleArgumentsConfig<T>(this IServiceCollection services, string[] args) where T : class
        {
            var genericTypeProps = typeof(T).GetProperties().ToList();
            if (genericTypeProps.Any(prop => prop.PropertyType != typeof(string))) throw new ArgumentException("Configuration class has properties different than String! Please use String and create type converter."); //TODO enable type conversion by adding args

            var expectedParams = typeof(T).GetProperties().Select(prop => prop.Name).ToList(); //TODO check attributes for name mappings

            var argumentsObject = ArgsParser.ParseArguments(args, expectedParams);

            var genericTypeInstance = Activator.CreateInstance(typeof(T));

            var dict = argumentsObject as IDictionary<string, object>;
            genericTypeProps.ForEach(prop =>
            {
                var value = dict[prop.Name];
                prop.SetValue(genericTypeInstance, value);
            });

            var convertedArgObj = genericTypeInstance as T;

            services.AddSingleton<T>(convertedArgObj);

            return services;
        }
    }

    public class ConsoleArgumentsWrapper
    {
        public dynamic Arguments;

        public ConsoleArgumentsWrapper(dynamic argumentsObject)
        {
            Arguments = argumentsObject;
        }
    }
}
