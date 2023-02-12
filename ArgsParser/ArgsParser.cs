using System.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace ArgsParser
{
    public static class ArgsParser
    {
        public static dynamic ParseArguments(string[] args, List<string> expectedParams = null)
        {
            Regex argumentStringValidator = ConsoleArgumentsValidatorProvider();
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

        public static Regex ConsoleArgumentsValidatorProvider()
        {
            return new Regex(@"^(((-\w+)|(-{2}\w+(-\w+)+))\s+((\d+(\.\d+)?)|(""[^""]*"")|(\'[^\']*\')|(\`[^\`]*\`))\s*)*$");
        }
    }
}