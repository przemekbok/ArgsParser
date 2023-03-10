namespace Tests
{
    namespace ConsoleAppTests
    {
        public class ParseArgumentsTests
        {
            [Fact]
            public void ParseArguments_WrongArgumentsFormat_ThrowsExpectedException()
            {
                //Arrange
                string[] args = new string[] { "-Username", "'john'", "Password", "'secret'" };

                //Act
                Action act = () => ArgsParser.ArgsParser.ParseArguments(args);
                ArgumentException exception = Assert.Throws<ArgumentException>(act);

                // Assert
                Assert.Equal("Incorrect arguments format!", exception.Message);
            }

            [Fact]
            public void ParseArguments_MissingDashSignOnArgumentWithMoreThaOneWord_ThrowsExpectedException()
            {
                //Arrange
                string[] args = new string[] { "-Username", "'john'", "-Super-Secret-Password", "'secret'" };

                //Act
                Action act = () => ArgsParser.ArgsParser.ParseArguments(args);
                ArgumentException exception = Assert.Throws<ArgumentException>(act);

                // Assert
                Assert.Equal("Incorrect arguments format!", exception.Message);
            }

            [Fact]
            public void ParseArguments_ArgumentWithMoreThanOneWord_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { "-Username", "'john'", "--Super-Secret-Password", "'secret'" };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args);

                // Assert
                Assert.Equal("john", result.Username);
                Assert.Equal("secret", result.SuperSecretPassword);
            }

            [Fact]
            public void ParseArguments_WithExpectedParams_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { "-Username", "'john'", "-Password", "'secret'" };
                List<string> expectedParams = new List<string> { "Username", "Password" };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args, expectedParams);

                // Assert
                Assert.Equal("john", result.Username);
                Assert.Equal("secret", result.Password);
            }

            [Fact]
            public void ParseArguments_WithUnexpectedParam_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { "-Username", "'john'", "-Password", "'secret'", "-Age", "30" };
                List<string> expectedParams = new List<string> { "-Username", "-Password" };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args, expectedParams);

                // Assert
                Assert.Equal("john", result.Username);
                Assert.Equal("secret", result.Password);
                Assert.True(((IDictionary<String, object>)result).ContainsKey("Age"));
            }

            [Fact]
            public void ParseArguments_WithOneMissingAndOneUnexpectedParam_ThrowsExpectedException()
            {
                // Arrange
                string[] args = new string[] { "-Username", "'john'", "-Age", "30" };
                List<string> expectedParams = new List<string> { "-Username", "-Password" };

                // Act
                Action act = () => ArgsParser.ArgsParser.ParseArguments(args, expectedParams);
                ArgumentException exception = Assert.Throws<ArgumentException>(act);

                // Assert
                Assert.Equal($"Argument Password is missing!", exception.Message);
            }

            [Fact]
            public void ParseArguments_WithoutExpectedParam_ThrowsExpectedException()
            {
                // Arrange
                string[] args = new string[] {"-Age", "30" };
                List<string> expectedParams = new List<string> { "-Username", "-Password" };

                // Act
                Action act = () => ArgsParser.ArgsParser.ParseArguments(args, expectedParams);
                ArgumentException exception = Assert.Throws<ArgumentException>(act);

                // Assert
                Assert.Equal($"Arguments {String.Join(" and ", expectedParams.Select(param => param.Replace("-",String.Empty)))} are missing!", exception.Message);
            }

            [Fact]
            public void ParseArguments_WithExpectedParamsOneWithMoreThanOneWord_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { "-Username", "'john'", "--Super-Secret-Password", "'secret'" };
                List<string> expectedParams = new List<string> { "-Username", "--Super-Secret-Password" };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args, expectedParams);

                // Assert
                Assert.Equal("john", result.Username);
                Assert.Equal("secret", result.SuperSecretPassword);
            }

            [Fact]
            public void ParseArguments_WithExpectedParamsTwoWithMissmachingSeparator_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { "-Username", "'john'", "--Super-Secret-Password", "'secret'" };
                List<string> expectedParams = new List<string> { "Username", "-Super-Secret-Password" };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args, expectedParams);

                // Assert
                Assert.Equal("john", result.Username);
                Assert.Equal("secret", result.SuperSecretPassword);
            }

            [Fact]
            public void ParseArguments_WithNoExpectedParams_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { "-Username", "'john'", "-Password", "'secret'" };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args);

                // Assert
                Assert.Equal("john", result.Username);
                Assert.Equal("secret", result.Password);
            }

            [Fact]
            public void ParseArguments_WithNoArgs_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args);

                // Assert
                Assert.False(((IDictionary<String, object>)result).ContainsKey("Username"));
                Assert.False(((IDictionary<String, object>)result).ContainsKey("Password"));
            }

            [Fact]
            public void ParseArguments_IntegerArgs_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { "-Username", "12345", "-Password", "12345" };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args);

                // Assert
                Assert.Equal("12345", result.Username);
                Assert.Equal("12345", result.Password);
            }

            [Fact]
            public void ParseArguments_DoubleArgs_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { "-Username", "12345.123", "-Password", "12345.123" };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args);

                // Assert
                Assert.Equal("12345.123", result.Username);
                Assert.Equal("12345.123", result.Password);
            }

            [Fact]
            public void ParseArguments_DoubleArgMissingDecimalPart_ThrowsExpectedException()
            {
                // Arrange
                string[] args = new string[] { "-Username", "12345." };

                // Act
                Action act = () => ArgsParser.ArgsParser.ParseArguments(args);
                ArgumentException exception = Assert.Throws<ArgumentException>(act);

                // Assert
                Assert.Equal("Incorrect arguments format!", exception.Message);
            }

            [Fact]
            public void ParseArguments_EmptyArgs_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { "-Username", "''", "-Password", "''" };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args);

                // Assert
                Assert.Equal("", result.Username);
                Assert.Equal("", result.Password);
            }

            [Fact]
            public void ParseArguments_SameArgs_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { "-Username", "'john'", "-Username", "'secret'" };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args);

                // Assert
                Assert.Equal("secret", result.Username);
            }

            [Fact]
            public void ParseArguments_DateArgs_ReturnsExpectedResult()
            {
                // Arrange
                string[] args = new string[] { "-Username", "'11/11/2011'", "-Password", "`16:00:32 12.12.2012`" };

                // Act
                dynamic result = ArgsParser.ArgsParser.ParseArguments(args);

                // Assert
                // Assert
                Assert.Equal("11/11/2011", result.Username);
                Assert.Equal("16:00:32 12.12.2012", result.Password);
            }

            [Fact]
            public void ParseArguments_DateArgWithoutQuotes_ThrowsExpectedException()
            {
                // Arrange
                string[] args = new string[] { "-Username", "11/11/2011", "-Password", "`16:00:32 12.12.2012`" };

                // Act
                Action act = () => ArgsParser.ArgsParser.ParseArguments(args);
                ArgumentException exception = Assert.Throws<ArgumentException>(act);

                // Assert
                Assert.Equal("Incorrect arguments format!", exception.Message);
            }
        }
    }
}