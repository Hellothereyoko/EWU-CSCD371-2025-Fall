namespace Calculate.Tests;

[TestClass]
public class CalculatorTests
{
    [TestMethod]
    public void Add_ProperValues_ReturnsProperly()
    {
        // Arrange
        int a = 5;
        int b = 10;
        // Act
        Calculator.Add(a, b, out int result);
        // Assert
        Assert.AreEqual(15, result);
    }

    [TestMethod]
    public void Add_NegativeValues_ReturnsProperly()
    {
        // Arrange
        int a = -5;
        int b = -10;
        // Act
        Calculator.Add(a, b, out int result);
        // Assert
        Assert.AreEqual(-15, result);
    }

    [TestMethod]
    public void Subtract_ProperValues_ReturnsProperly()
    {
        // Arrange
        int a = 10;
        int b = 5;
        // Act
        Calculator.Subtract(a, b, out int result);
        // Assert
        Assert.AreEqual(5, result);
    }

    [TestMethod]
    public void Subtract_NegativeValues_ReturnsProperly()
    {
        // Arrange
        int a = -10;
        int b = -5;
        // Act
        Calculator.Subtract(a, b, out int result);
        // Assert
        Assert.AreEqual(-5, result);
    }

    [TestMethod]
    public void Multiple_ProperValues_ReturnsProperly()
    {
        // Arrange
        int a = 5;
        int b = 10;
        // Act
        Calculator.Multiple(a, b, out int result);
        // Assert
        Assert.AreEqual(50, result);
    }

    [TestMethod]
    public void Multiple_NegativeValues_ReturnsProperly()
    {
        // Arrange
        int a = -5;
        int b = -10;
        // Act
        Calculator.Multiple(a, b, out int result);
        // Assert
        Assert.AreEqual(50, result);
    }

    [TestMethod]
    public void Divide_ProperValues_ReturnsProperly()
    {
        // Arrange
        int a = 10;
        int b = 2;
        // Act
        Calculator.Divide(a, b, out double result);
        // Assert
        Assert.AreEqual(5, result);
    }

    [TestMethod]
    public void Divide_DivideByZero_ThrowsException()
    {
        // Arrange
        int a = 10;
        int b = 0;
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => Calculator.Divide(a, b, out double result));
    }

    [TestMethod]
    public void Divide_NegativeValues_ReturnsProperly()
    {
        // Arrange
        int a = -10;
        int b = -2;
        // Act
        Calculator.Divide(a, b, out double result);
        // Assert
        Assert.AreEqual(5, result);
    }
}
