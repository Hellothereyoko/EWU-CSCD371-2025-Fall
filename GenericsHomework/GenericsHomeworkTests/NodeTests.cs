namespace GenericsHomework.Tests;

public class NodeTests
{
    [Fact]
    public void Constructor_ProperValue_InitializesCorrectly()
    {
        // Arrange
        Node<int> node = new Node<int>(10);
        // Act & Assert
        Assert.NotNull(node);
        Assert.Equal(10, node.Value);
        Assert.Equal(node, node.Next);
    }

    [Fact]
    public void ToString_OverridesCorrectly_PrintsCorrectly()
    {
        // Arrange
        Node<string> node = new Node<string>("Test");
        // Act
        string result = node.ToString();
        // Assert
        Assert.Equal("Test", result);
    }

    [Fact]
    public void ToString_NullValue_ReturnsEmptyString()
    {
        // Arrange
        Node<string> node = new Node<string>("test");
        node.Value = null!;
        // Act
        string result = node.ToString();
        // Assert
        Assert.Equal(string.Empty, result);
    }
}
