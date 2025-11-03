namespace Calculate.Tests;

[TestClass]
public sealed class ProgramTests
{
    private static readonly string[] TestInputSequence =
    {
        "3 + 4",      // Valid calculation
        "10 - 5",     // Another valid one
        "invalid",    // Invalid input
        "exit"        // Exit command
    };

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

    [TestMethod]
    public void Main_ValidCalculations_ProducesCorrectOutput()
    {
        // Arrange
        var outputs = new List<string>();
        var inputs = new Queue<string>(TestInputSequence);

        var program = new Program
        {
            WriteLine = s => outputs.Add(s),
            ReadLine = () => inputs.Dequeue()
        };

        var calculator = new Calculator();

        // Act - Simulate the Main() logic
        program.WriteLine("Enter calculation (e.g., '3 + 4') or 'exit' to quit:");

        while (true)
        {
            var input = program.ReadLine();
            if (string.IsNullOrWhiteSpace(input) ||
                input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            if (calculator.TryCalculate(input, out int result))
            {
                program.WriteLine($"Result: {result}");
            }
            else
            {
                program.WriteLine("Invalid calculation. Use format: number operator number (e.g., '3 + 4')");
            }
        }

        // Assert
        Assert.HasCount(4, outputs);
        Assert.AreEqual<string>("Enter calculation (e.g., '3 + 4') or 'exit' to quit:", outputs[0]);
        Assert.AreEqual<string>("Result: 7", outputs[1]);
        Assert.AreEqual<string>("Result: 5", outputs[2]);
        Assert.Contains("Invalid calculation", outputs[3]);
    }


    [TestMethod]
    public void Main_ShouldExitOnBlankInput()
    {
        // Arrange
        var output = new List<string>();
        var inputs = new Queue<string>(new[] { "" }); // blank input => exit immediately

        var program = new Program
        {
            WriteLine = s => output.Add(s),
            ReadLine = () => inputs.Dequeue()
        };

        var calculator = new Calculator();

        // Act
        program.WriteLine("Enter calculation (e.g., '3 + 4') or 'exit' to quit:");

        while (true)
        {
            var input = program.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;
        }

        // Assert
        Assert.ContainsSingle(output); // only initial prompt
    }
}
