using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.ExtensionMethods;

namespace Tests
{
    namespace Regex
    {
        public class RegexTest
        {
            //String checks

            [Fact]
            public void ConsoleArgumentsRegexTests_StringArguments_MatchesRegex()
            {
                //Arrange
                string args = String.Join(" ",new string[] { "-Username", "'john'", "-Password", "'secret'" });

                //Act
                var consoleArgumentsValidator =  ArgsParser.ArgsParser.ConsoleArgumentsValidatorProvider();

                // Assert
                Assert.Matches(consoleArgumentsValidator, args);
            }

            [Fact]
            public void ConsoleArgumentsRegexTests_99StringArguments_MatchesRegex()
            {
                //Arrange
                var argsAsList = new List<string>{ };

                for (int i=1; i < 100 ;i++)
                {
                    argsAsList.Add($"-Username{i} 'john{i}'");
                }

                string args = String.Join(" ", argsAsList);

                //Act
                var consoleArgumentsValidator = ArgsParser.ArgsParser.ConsoleArgumentsValidatorProvider();

                // Assert
                Assert.Matches(consoleArgumentsValidator, args);
            }

            [Fact]
            public void onsoleArgumentsRegexTests_StringsWithMissingQuotes_MatchesRegex()
            {
                //Arrange
                string args = String.Join(" ", new string[] { "-Username", "'john", "-Password", "'secret'" });

                //Act
                var consoleArgumentsValidator = ArgsParser.ArgsParser.ConsoleArgumentsValidatorProvider();

                // Assert
                Assert.DoesNotMatch(consoleArgumentsValidator, args);
            }

            //Integer checks

            [Fact]
            public void ConsoleArgumentsRegexTests_IntegerArguments_MatchesRegex()
            {
                //Arrange
                string args = String.Join(" ", new string[] { "-Username", "123", "-Password", "456" });

                //Act
                var consoleArgumentsValidator = ArgsParser.ArgsParser.ConsoleArgumentsValidatorProvider();

                // Assert
                Assert.Matches(consoleArgumentsValidator, args);
            }

            [Fact]
            public void ConsoleArgumentsRegexTests_99IntegerArguments_MatchesRegex()
            {
                //Arrange
                var argsAsList = new List<string> { };

                for (int i = 1; i < 100; i++)
                {
                    argsAsList.Add($"-Username{i} {i}");
                }

                string args = String.Join(" ", argsAsList);

                //Act
                var consoleArgumentsValidator = ArgsParser.ArgsParser.ConsoleArgumentsValidatorProvider();

                // Assert
                Assert.Matches(consoleArgumentsValidator, args);
            }

            //Double checks

            [Fact]
            public void ConsoleArgumentsRegexTests_DoubleArguments_MatchesRegex()
            {
                //Arrange
                string args = String.Join(" ", new string[] { "-Username", "123.123", "-Password", "456.456" });

                //Act
                var consoleArgumentsValidator = ArgsParser.ArgsParser.ConsoleArgumentsValidatorProvider();

                // Assert
                Assert.Matches(consoleArgumentsValidator, args);
            }

            [Fact]
            public void ConsoleArgumentsRegexTests_99DoubleArguments_MatchesRegex()
            {
                //Arrange
                var argsAsList = new List<string> { };

                for (int i = 1; i < 100; i++)
                {
                    argsAsList.Add($"-Username{i} {i}.{i}");
                }

                string args = String.Join(" ", argsAsList);

                //Act
                var consoleArgumentsValidator = ArgsParser.ArgsParser.ConsoleArgumentsValidatorProvider();

                // Assert
                Assert.Matches(consoleArgumentsValidator, args);
            }

            [Fact]
            public void onsoleArgumentsRegexTests_DoubleArgumentsWithoutDecimalPart_MatchesRegex()
            {
                //Arrange
                string args = String.Join(" ", new string[] { "-Username", "123.", "-Password", "456." });

                //Act
                var consoleArgumentsValidator = ArgsParser.ArgsParser.ConsoleArgumentsValidatorProvider();

                // Assert
                Assert.DoesNotMatch(consoleArgumentsValidator, args);
            }
        }
    }
}
