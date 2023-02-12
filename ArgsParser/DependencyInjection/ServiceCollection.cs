using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgsParser.DependencyInjection
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddConsoleArgumentsObject(this IServiceCollection services, string[] args, List<string> expectedParams = null)
        {
            var argumentsObject = ArgsParser.ParseArguments(args, expectedParams);
            var argObjWrapper = new ConsoleArgumentsWrapper(argumentsObject);
            services.AddSingleton<ConsoleArgumentsWrapper>(argObjWrapper);

            return services;
        }

        public static IServiceCollection AddConsoleArgumentsObject<T>(this IServiceCollection services, string[] args) where T : class
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
