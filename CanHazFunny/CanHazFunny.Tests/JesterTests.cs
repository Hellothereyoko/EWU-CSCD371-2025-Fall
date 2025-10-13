using Xunit;
using Moq;
using System;
using System.IO;
using System.Net.Http;

namespace CanHazFunny.Tests;

// ----------------------------------------------------------------------
// MOCKABLE CLASSES (Used for fast Unit Tests)
// ----------------------------------------------------------------------

// Helper class to override JokeService's network call behavior for fast, targeted testing.
public class MockableJokeService : JokeService
{
    private readonly bool _shouldThrow;
    private readonly string _nextJoke;
    private int _callCount = 0;

    public MockableJokeService(string nextJoke = "Not a Chuck Norris joke", bool shouldThrow = false)
    {
        _nextJoke = nextJoke;
        _shouldThrow = shouldThrow;
    }

    // Overrides the virtual method in JokeService to control API output
    protected override string GetJokeFromApi() 
    {
        if (_shouldThrow)
        {
            // For the error test
            throw new HttpRequestException("Simulated API error");
        }
        
        // Logic for the Chuck Norris filter test:
        // Returns the Chuck Norris joke ONLY on the first call, then returns clean.
        if (_nextJoke.Contains("Chuck Norris") && _callCount == 0)
        {
            _callCount++;
            return _nextJoke; // 1st call: Returns Chuck Norris joke
        }
        
        return "Not a Chuck Norris joke"; // 2nd call: Returns clean joke, breaking the loop
    }
}

// ----------------------------------------------------------------------
// JESTER TESTS (100% LINE & BRANCH COVERAGE)
// ----------------------------------------------------------------------

public class JesterTests
{
    [Fact]
    public void GetJoke_ReturnsJokeFromJokeService()
    {
        // Arrange
        var mockJokeService = new Mock<IJokeService>();
        var mockOutputService = new Mock<IOutput>();
        var expectedJoke = "Why did the programmer quit? He didn't get arrays!";

        mockJokeService.Setup(js => js.GetJoke()).Returns(expectedJoke);
        var jester = new Jester(mockOutputService.Object, mockJokeService.Object);


        // Act
        var result = jester.GetJoke(); 


        // Assert
        Assert.Equal(expectedJoke, result);
        mockJokeService.Verify(js => js.GetJoke(), Times.Once);
    }

    [Fact]
    public void TellJoke_CallsOutputServiceWithJoke()
    {
        // Arrange
        var mockJokeService = new Mock<IJokeService>();
        var mockOutputService = new Mock<IOutput>();
        var testJoke = "Test joke";

        mockJokeService.Setup(js => js.GetJoke()).Returns(testJoke);
        var jester = new Jester(mockOutputService.Object, mockJokeService.Object);


        // Act
        jester.TellJoke();


        // Assert
        mockOutputService.Verify(os => os.WriteLine(testJoke), Times.Once); 
        mockJokeService.Verify(js => js.GetJoke(), Times.Once);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenOutputServiceIsNull()
    {
        // Arrange
        var mockJokeService = new Mock<IJokeService>();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Jester(null!, mockJokeService.Object));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenJokeServiceIsNull()
    {
        // Arrange
        var mockOutputService = new Mock<IOutput>();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Jester(mockOutputService.Object, null!));
    }

  
    [Fact]
public void TellJoke_SkipsChuckNorrisJoke_AndGetsNext()
{
    // Arrange
    var mockJokeService = new Mock<IJokeService>();
    var mockOutputService = new Mock<IOutput>(); // Ensure output service is mocked

    var chuckJoke = "Chuck Norris always wins";
    var goodJoke = "A programmer walks into a bar";

    // Setup the mock to return Chuck Norris first, then a valid joke
    mockJokeService.SetupSequence(js => js.GetJoke())
        .Returns(chuckJoke) 
        .Returns(goodJoke);
        
    // Use the mocked output service
    var jester = new Jester(mockOutputService.Object, mockJokeService.Object);

    // Act
    jester.TellJoke();

    // Assert
    // This now passes because Jester.TellJoke() contains the filtering loop.
    mockJokeService.Verify(js => js.GetJoke(), Times.Exactly(2));

    // NEW ASSERTION: Verify the good joke was the one written
    mockOutputService.Verify(os => os.WriteLine(goodJoke), Times.Once);
    mockOutputService.Verify(os => os.WriteLine(chuckJoke), Times.Never);
}
}

// ----------------------------------------------------------------------
// JOKE SERVICE TESTS (100% BRANCH COVERAGE)
// ----------------------------------------------------------------------

public class JokeServiceTests
{
    [Fact]
    public void GetJoke_ReturnsNonEmptyString()
    {
        // Arrange: Uses the fast Mockable service
        var jokeService = new MockableJokeService();

        // Act
        var joke = jokeService.GetJoke();


        // Assert
        Assert.NotNull(joke);
        Assert.NotEmpty(joke);
    }


    [Fact]
    public void GetJoke_ChuckNorrisFilter()
    {

        // Arrange: JokeService should receive this string from the mock API call.
        var expectedJoke = "Chuck Norris joke";
        var jokeService = new MockableJokeService(nextJoke: expectedJoke);
    
        // Act
        var joke = jokeService.GetJoke();

        // Assert: Since filtering is now Jester's job, JokeService must return the joke it received.
        // The previous assertions (Assert.DoesNotContain/Assert.Equal to "Not a Chuck Norris joke") were removed.
        Assert.Equal(expectedJoke, joke);
    }


    [Fact]
    public void GetJoke_ReturnsErrorMessage_OnError()
    {
        // Arrange: Use mockable service set to throw an exception instantly
        var jokeService = new MockableJokeService(shouldThrow: true);

        // Act
        var result = jokeService.GetJoke();

        // Assert
        Assert.Equal("Error retrieving joke. Please try again later", result);
    }
}

// ----------------------------------------------------------------------
// CONSOLE OUTPUT TESTS
// ----------------------------------------------------------------------

public class ConsoleOutputTests
{
    [Fact]
    public void WriteLine_FormatsMessageCorrectly()
    {
        // Arrange
        var outputService = new ConsoleOutput();
        var testMessage = "This is a test joke";
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);


        // Act
        ((IOutput)outputService).WriteLine(testMessage);


        // Assert
        var output = stringWriter.ToString().Trim();
        Assert.Equal(testMessage, output);
        Assert.Equal(testMessage, outputService.Output);
    }

    [Fact]
    public void WriteLine_StoresOutputInPublicProperty()
    {
        // Arrange
        var outputService = new ConsoleOutput();
        var testMessage = "Another test";
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);


        // Act
        ((IOutput)outputService).WriteLine(testMessage);


        // Assert
        Assert.Equal(testMessage, outputService.Output);
    }
}

// ----------------------------------------------------------------------
// INTEGRATION TESTS (These tests will now execute and cause delays)
// ----------------------------------------------------------------------

public class IntegrationTests
{
    [Fact]
    public void JokeServiceImplementation_GetJoke_CallsUnderlyingJokeService()
    {
        // This test makes a real network call (SLOOOOOW)
        var jokeServiceImpl = new JokeService();


        // Act
        var joke = jokeServiceImpl.GetJoke();


        // Assert
        Assert.NotNull(joke);
        Assert.NotEmpty(joke);
    }

    [Fact]
    public void Jester_IntegrationTest_WithRealServices()
    {
        // This test makes a real network call (SLOOOOOW)
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        var outputService = new ConsoleOutput();
        var jokeService = new JokeService(); 
        var jester = new Jester(outputService, jokeService);


        // Act
        jester.TellJoke();


        // Assert
        var consoleOutput = stringWriter.ToString().Trim();
        Assert.NotEmpty(outputService.Output!);
    }
}