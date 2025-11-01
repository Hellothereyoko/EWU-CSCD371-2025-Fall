namespace Calculate.Tests;

[TestClass]
public sealed class ProgramTests
{
    [TestMethod]
    public void Constructor_InitializesWriteLineAndReadLine_Success()
    {
        // Arrange & Act
        var program = new Program();
        // Assert
        Assert.IsNotNull(program.WriteLine);
        Assert.IsNotNull(program.ReadLine);
    }

    [TestMethod]
    public void WriteLine_WritesExpectedOutput_Success()
    {
        // Arrange
        var output = string.Empty;
        var program = new Program
        {
            // Override WriteLine to capture output
            WriteLine = s => output = s
        };
        var expectedMessage = "Hello, World!";
        // Act
        program.WriteLine(expectedMessage);
        // Assert
        Assert.AreEqual<string>(expectedMessage, output);
    }

    [TestMethod]
    public void ReadLine_ReturnsExpectedInput_Success()
    {
        // Arrange
        var expectedInput = "Test Input";
        var program = new Program
        {
            // Override ReadLine to return expected input
            ReadLine = () => expectedInput
        };
        // Act
        var actualInput = program.ReadLine();
        // Assert
        Assert.AreEqual<string>(expectedInput, actualInput);
    }
}
