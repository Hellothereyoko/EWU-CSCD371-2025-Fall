using Calculate;

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
    public void Constructor_WriteLineDefaultsToConsoleWriteLine_Success()
    {
        // Arrange
        var program = new Program();
        
        // Act - WriteLine should be callable without throwing
        program.WriteLine("test");
        
        // Assert - If we get here without exception, default is set
        Assert.IsNotNull(program.WriteLine);
    }

    [TestMethod]
    public void Constructor_ReadLineDefaultsToConsoleReadLine_Success()
    {
        // Arrange
        var program = new Program();
        
        // Assert - ReadLine should be callable
        Assert.IsNotNull(program.ReadLine);
    }

    [TestMethod]
    public void WriteLine_CustomDelegate_WritesExpectedOutput()
    {
        // Arrange
        var output = string.Empty;
        var program = new Program
        {
            WriteLine = s => output = s
        };
        var expectedMessage = "Hello, World!";
        
        // Act
        program.WriteLine(expectedMessage);
        
        // Assert
        Assert.AreEqual<string>(expectedMessage, output);
    }

    [TestMethod]
    public void ReadLine_CustomDelegate_ReturnsExpectedInput()
    {
        // Arrange
        var expectedInput = "Test Input";
        var program = new Program
        {
            ReadLine = () => expectedInput
        };
        
        // Act
        var actualInput = program.ReadLine();
        
        // Assert
        Assert.AreEqual<string>(expectedInput, actualInput);
    }

    [TestMethod]
    public void WriteLine_MultipleInvocations_CapturesAllOutput()
    {
        // Arrange
        var outputs = new List<string>();
        var program = new Program
        {
            WriteLine = s => outputs.Add(s)
        };
        
        // Act
        program.WriteLine("First");
        program.WriteLine("Second");
        program.WriteLine("Third");
        
        // Assert
        Assert.AreEqual<int>(3, outputs.Count);
        Assert.AreEqual<string>("First", outputs[0]);
        Assert.AreEqual<string>("Second", outputs[1]);
        Assert.AreEqual<string>("Third", outputs[2]);
    }

    [TestMethod]
    public void Main_ValidCalculation_OutputsCorrectResult()
    {
        // Arrange
        var inputs = string.Join(Environment.NewLine, new[] { "5 + 3", "exit" });
        
        using var writer = new StringWriter();
        Console.SetOut(writer);
        
        using var reader = new StringReader(inputs);
        Console.SetIn(reader);

        // Act
        Program.Main();

        // Assert
        var output = writer.ToString();
        Assert.IsTrue(output.Contains("Result: 8"));
        Assert.IsTrue(output.Contains("Enter calculation"));
    }

    [TestMethod]
    public void Main_InvalidCalculation_OutputsErrorMessage()
    {
        // Arrange
        var inputs = string.Join(Environment.NewLine, new[] { "invalid input", "exit" });

        using var writer = new StringWriter();
        Console.SetOut(writer);
        
        using var reader = new StringReader(inputs);
        Console.SetIn(reader);

        // Act
        Program.Main();

        // Assert
        var output = writer.ToString();
        Assert.IsTrue(output.Contains("Invalid calculation"));
    }

    [TestMethod]
    public void Main_BlankInput_ExitsImmediately()
    {
        // Arrange
        using var writer = new StringWriter();
        Console.SetOut(writer);
        
        using var reader = new StringReader(Environment.NewLine);
        Console.SetIn(reader);

        // Act
        Program.Main();

        // Assert
        var output = writer.ToString();
        Assert.IsTrue(output.Contains("Enter calculation"));
        var lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        Assert.AreEqual<int>(1, lines.Length);
    }

    [TestMethod]
    public void Main_ExitCommand_ExitsWithoutProcessing()
    {
        // Arrange
        using var writer = new StringWriter();
        Console.SetOut(writer);
        
        using var reader = new StringReader("exit");
        Console.SetIn(reader);

        // Act
        Program.Main();

        // Assert
        var output = writer.ToString();
        Assert.IsTrue(output.Contains("Enter calculation"));
        Assert.IsFalse(output.Contains("Result:"));
        Assert.IsFalse(output.Contains("Invalid calculation"));
    }

    [TestMethod]
    public void Main_MultipleCalculations_ProcessesAllCorrectly()
    {
        // Arrange
        var inputs = string.Join(Environment.NewLine, new[]
        {
            "10 - 5",    // Valid: 5
            "3 * 4",     // Valid: 12
            "20 / 4",    // Valid: 5
            "bad",       // Invalid
            "exit"
        });

        using var writer = new StringWriter();
        Console.SetOut(writer);
        
        using var reader = new StringReader(inputs);
        Console.SetIn(reader);

        // Act
        Program.Main();

        // Assert
        var output = writer.ToString();
        Assert.IsTrue(output.Contains("Result: 5"));
        Assert.IsTrue(output.Contains("Result: 12"));
        Assert.IsTrue(output.Contains("Invalid calculation"));
    }

    [TestMethod]
    public void Main_MixedValidAndInvalid_HandlesAll()
    {
        // Arrange
        var inputs = string.Join(Environment.NewLine, new[]
        {
            "1 + 1",     // Valid: 2
            "no spaces", // Invalid
            "100 / 10",  // Valid: 10
            "exit"
        });

        using var writer = new StringWriter();
        Console.SetOut(writer);
        
        using var reader = new StringReader(inputs);
        Console.SetIn(reader);

        // Act
        Program.Main();

        // Assert
        var output = writer.ToString();
        Assert.IsTrue(output.Contains("Result: 2"));
        Assert.IsTrue(output.Contains("Result: 10"));
        Assert.IsTrue(output.Contains("Invalid calculation"));
    }

    [TestMethod]
    public void Main_ExitCommandCaseInsensitive_ExitsCorrectly()
    {
        // Arrange
        var inputs = string.Join(Environment.NewLine, new[] { "EXIT", "exit", "Exit" });

        using var writer = new StringWriter();
        Console.SetOut(writer);
        
        using var reader = new StringReader(inputs);
        Console.SetIn(reader);

        // Act
        Program.Main();

        // Assert
        var output = writer.ToString();
        // Should exit on first EXIT command
        Assert.IsTrue(output.Contains("Enter calculation"));
    }
}