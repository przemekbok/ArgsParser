using System.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace ArgsParser
{
    public static class ArgsParser
    {
        public static IServiceCollection AddConsoleArgumentsObject(this IServiceCollection services, string[] args, List<string> expectedParams = null)
        {
            var argumentsObject = ArgsParser.ParseArguments(args, expectedParams);
            var argObjWrapper = new ConsoleArguments(argumentsObject);
            services.AddSingleton<ConsoleArguments>(argObjWrapper);

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

        public static dynamic ParseArguments(string[] args, List<string> expectedParams = null)
        {
            Regex argumentStringValidator = new Regex(@"^(((-\w+)|(-{2}\w+(-\w+)+))\s+((\d+(\.\d+)?)|(""[^""]*"")|(\'[^\']*\')|(\`[^\`]*\`))\s*)*$");
            var argsAsAString = String.Join(" ", args);
            if(!argumentStringValidator.IsMatch(argsAsAString)) throw new ArgumentException("Incorrect arguments format!");

            expectedParams = expectedParams ?? new List<string>();
            expectedParams = expectedParams.Select(param => param.Replace("-", String.Empty)).ToList();

            dynamic result = new ExpandoObject();
            var dict = (IDictionary<string, object>)result;

            for (int i = 0; i < args.Length; i += 2)
            {
                string key = args[i];
                key = key.Replace("-", String.Empty);
                string value = args[i + 1];
                value = value.Trim('"').Trim('\'').Trim('`');

                dict[key] = value;
            }

            var argumentDiff = expectedParams.Except(dict.Keys).ToList();
            if (argumentDiff.Count() == 1) throw new ArgumentException($"Argument {argumentDiff.First()} is missing!");
            if (argumentDiff.Count() > 1) throw new ArgumentException($"Arguments {String.Join(" and ", argumentDiff)} are missing!");

            return result;
        }
    }

    public class ConsoleArguments
    {
        private readonly dynamic ArgumentsObject;

        public ConsoleArguments(dynamic argumentsObject)
        {
            ArgumentsObject = argumentsObject;
        }

        public dynamic GetArgumentsObject()
        {
            return ArgumentsObject;
        }
    }
}