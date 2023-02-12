namespace Tests
{
    namespace ExtensionMethods
    {
        public class AddConsoleArgumentsObjectTests
        {
            private ServiceCollectionProvider serviceCollectionProvider = new ServiceCollectionProvider();

            [Fact]
            public void AddConsoleArgumentsObjectTests_SpecifiedOutputType_ReturnsExpectedResult()
            {
                //Arrange
                string[] args = new string[] { "-Username", "'john'", "-Password", "'secret'" };

                //Act
                var services = serviceCollectionProvider.GetServices();
                services.AddConsoleArgumentsObject<MockSimpleConsoleArgumentsClass>(args);
                MockSimpleConsoleArgumentsClass? result = services.BuildServiceProvider().GetService(typeof(MockSimpleConsoleArgumentsClass)) as MockSimpleConsoleArgumentsClass;

                // Assert
                Assert.Equal("john", result?.Username);
                Assert.Equal("secret", result?.Password);
            }
        }
    }
}
