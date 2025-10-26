namespace GenericsHomework.Tests;

public class NodeTests
{
    [Fact]
    public void Constructor_ProperValue_Initializes()
    {
        // Arrange
        Node<int> testNode = new Node<int>(10);
        // Act & Assert
        Assert.NotNull(testNode);
        Assert.Equal(10, testNode.Value);
        Assert.Equal(testNode, testNode.Next);
    }

    [Fact]
    public void ToString_ReturnsValueString_Success()
    {
        // Arrange
        Node<string> testNode = new Node<string>("TestValue");
        // Act
        string result = testNode.ToString();
        // Assert
        Assert.Equal("TestValue", result);
    }

    [Fact]
    public void ToString_NullValue_ReturnsEmptyString()
    {
        // Arrange
        Node<string> testNode = new Node<string>("test");
        testNode.Value = null!;
        // Act
        string result = testNode.ToString();
        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Append_NextNode_SetsNextProperly()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        // Act
        firstNode.Append(2);
        // Assert
        Assert.Equal(2, firstNode.Next.Value);
    }
}
