namespace Calculate.Tests;

[TestClass]
public sealed class CalculatorTests
{
    [TestMethod]
    public void MathematicalOperations_ContainsAllOperators_Success()
    {
        // Arrange
        var calculator = new Calculator();
        var expectedOperators = new[] { '+', '-', '*', '/' };

        // Act & Assert
        foreach (var op in expectedOperators)
        {
            Assert.IsTrue(calculator.MathematicalOperations.ContainsKey(op));
        }
    }

    [TestMethod]
    public void TryCalculate_ValidInput_ReturnsTrue()
    {
        // Arrange
        var calculator = new Calculator();
        var testCases = new[]
        {
            ("3 + 4", 7),
            ("10 - 3", 7),
            ("3 * 4", 12),
            ("12 / 3", 4)
        };

        // Act & Assert
        foreach (var (input, expected) in testCases)
        {
            Assert.IsTrue(calculator.TryCalculate(input, out int result));
            Assert.AreEqual<int>(expected, result);
        }
    }

    [TestMethod]
    public void TryCalculate_InvalidInput_ReturnsFalse()
    {
        // Arrange
        var calculator = new Calculator();
        var invalidInputs = new[]
        {
            "3+4",      // No spaces
            "a + b",    // Non-numeric
            "3 + ",     // Missing operand
            "3 $ 4"     // Invalid operator
        };

        // Act & Assert
        foreach (var input in invalidInputs)
        {
            Assert.IsFalse(calculator.TryCalculate(input, out _));
        }
    }
}