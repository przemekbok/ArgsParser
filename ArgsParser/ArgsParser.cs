using System.Dynamic;
using System.Text.RegularExpressions;

namespace ArgsParser
{
    public static class ArgsParser
    {
        public static dynamic ParseArguments(string[] args, List<string> expectedParams = null)
        {
            Regex argumentStringValidator = new Regex(@"^(((-\w+)|(-{2}\w+(-\w+)+))\s+((\d+(\.\d+)?)|(""[^""]+"")|(\'[^\']+\')|(\`[^\`]+\`))\s*)*$");
            var argsAsAString = String.Join(" ", args);
            if(!argumentStringValidator.IsMatch(argsAsAString)) throw new ArgumentException("Incorrect arguments format!");

            expectedParams = expectedParams ?? new List<string>();
            expectedParams = expectedParams.Select(param => param.Replace("-", String.Empty)).ToList();

            dynamic result = new ExpandoObject();
            var dict = (IDictionary<string, object>)result;

            for (int i = 0; i < args.Length; i += 2)
            {
                if (i + 1 < args.Length)
                {
                    string key = args[i];
                    key = key.Replace("-", String.Empty);
                    string value = args[i + 1];

                    dict[key] = value;
                }
            }

            var argumentDiff = expectedParams.Except(dict.Keys).ToList();
            if (argumentDiff.Count() > 0) throw new ArgumentException($"Arguments {String.Join(" and ", argumentDiff)} are missing!");

            return result;
        }
    }
}