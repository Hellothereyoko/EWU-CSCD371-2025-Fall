using Xunit;
using Moq;
using System;
using System.IO;

namespace CanHazFunny.Tests
{
    public class JesterTests
    {
        [Fact]
        public void GetJoke_ReturnsJokeFromJokeService()
        {
            // Arrange
            var mockJokeService = new Mock<IJokeService>();
            var mockOutputService = new Mock<IOutputService>();
            var expectedJoke = "Why did the programmer quit? He didn't get arrays!";

            mockJokeService.Setup(js => js.GetJoke()).Returns(expectedJoke);
            var jester = new Jester(mockOutputService.Object, mockJokeService.Object);


            // Act
            var result = jester.getJoke();


            // Assert
            Assert.Equal(expectedJoke, result);
            mockJokeService.Verify(js => js.GetJoke(), Times.Once);
        }

        [Fact]
        public void TellJoke_CallsOutputServiceWithJoke()
        {
            // Arrange
            var mockJokeService = new Mock<IJokeService>();
            var mockOutputService = new Mock<IOutputService>();
            var testJoke = "Test joke";

            mockJokeService.Setup(js => js.GetJoke()).Returns(testJoke);
            var jester = new Jester(mockOutputService.Object, mockJokeService.Object);


            // Act
            jester.TellJoke();


            // Assert
            mockOutputService.Verify(os => os.Write(testJoke), Times.Once);
            mockJokeService.Verify(js => js.GetJoke(), Times.Once);
        }
    }

    public class JokeServiceTests
    {
        [Fact]
        public void GetJoke_ReturnsNonEmptyString()
        {
            // Arrange
            var jokeService = new JokeService();


            // Act
            var joke = jokeService.GetJoke();


            // Assert
            Assert.NotNull(joke);
            Assert.NotEmpty(joke);
        }

        [Fact]
        public void GetJoke_ChuckNorrisFilter()
        {

            // In a real scenario, you'd want to inject HttpClient or use an interface
            // For now, we can test the logic indirectly

            var jokeService = new JokeService();
            var joke = jokeService.GetJoke();

            // Assert that if we get a joke, it shouldn't contain Chuck Norris
            // or it should be the filtered message
            if (joke != "No Chuck Norris Jokes!")
            {
                Assert.DoesNotContain("Chuck Norris", joke);
                Assert.DoesNotContain("Chuck", joke);
                Assert.DoesNotContain("Norris", joke);
            }
        }
    }

    public class ConsoleOutputServiceTests
    {
        [Fact]
        public void Write_FormatsMessageWithResponsePrefix()
        {
            // Arrange
            var outputService = new ConsoleOutputService();
            var testMessage = "This is a test joke";
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);


            // Act
            ((IOutputService)outputService).Write(testMessage);


            // Assert
            var output = stringWriter.ToString().Trim();
            Assert.Equal("Response: " + testMessage, output);
            Assert.Equal("Response: " + testMessage, outputService.output);
        }

        [Fact]
        public void Write_StoresOutputInPublicField()
        {
            // Arrange
            var outputService = new ConsoleOutputService();
            var testMessage = "Another test";
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);


            // Act
            ((IOutputService)outputService).Write(testMessage);


            // Assert
            Assert.Equal("Response: " + testMessage, outputService.output);
        }
    }

    public class IJokeServiceImplementationTests
    {
        [Fact]
        public void JokeServiceImplementation_GetJoke_CallsUnderlyingJokeService()
        {
            // Arrange
            var jokeServiceImpl = new jokeService();


            // Act
            var joke = jokeServiceImpl.GetJoke();


            // Assert
            Assert.NotNull(joke);
            Assert.NotEmpty(joke);
        }
    }

    public class IntegrationTests
    {
        
        [Fact]
        public void Jester_IntegrationTest_WithRealServices()
        {
            // Arrange
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            var outputService = new ConsoleOutputService();
            var jokeService = new JokeService();
            var jester = new Jester(outputService, jokeService);


            // Act
            jester.TellJoke();


            // Assert
            var consoleOutput = stringWriter.ToString().Trim();
            Assert.NotEmpty(outputService.output);
        }

    }

}

//Encoded using IntelliSense & modified by the dev to fit into the existing architecture