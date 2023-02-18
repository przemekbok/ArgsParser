using Tests.Helpers.DependencyInjection;

namespace Tests
{
    namespace ExtensionMethods
    {
        public class AddConsoleArgumentsObjectTests
        {
            private ServiceCollectionProvider serviceCollectionProvider = new ServiceCollectionProvider();

            [Fact]
            public void AddConsoleArgumentsObjectTests_HostBuilderAsAnArgument_ReturnsExpectedResult()
            {
                //Arrange
                string[] args = new string[] { "-Username", "'john'", "-Password", "'secret'" };

                //Act
                var services = serviceCollectionProvider.GetServices();
                var hostBuilder = serviceCollectionProvider.GetHostBuilderWithArgs(args);
                services.AddConsoleArgumentsConfig<MockSimpleConsoleArgumentsClass>(hostBuilder);
                MockSimpleConsoleArgumentsClass? result = services.BuildServiceProvider().GetService(typeof(MockSimpleConsoleArgumentsClass)) as MockSimpleConsoleArgumentsClass;

                // Assert
                Assert.Equal("john", result?.Username);
                Assert.Equal("secret", result?.Password);
            }

            [Fact]
            public void AddConsoleArgumentsObjectTests_SpecifiedOutputType_ReturnsExpectedResult()
            {
                //Arrange
                string[] args = new string[] { "-Username", "'john'", "-Password", "'secret'" };

                //Act
                var services = serviceCollectionProvider.GetServices();
                services.AddConsoleArgumentsConfig<MockSimpleConsoleArgumentsClass>(args);
                MockSimpleConsoleArgumentsClass? result = services.BuildServiceProvider().GetService(typeof(MockSimpleConsoleArgumentsClass)) as MockSimpleConsoleArgumentsClass;

                // Assert
                Assert.Equal("john", result?.Username);
                Assert.Equal("secret", result?.Password);
            }
        }
    }
}
