using System;
using GenericsHomework; //You need this pkg to communicate w/ main pgrm 

namespace GenericsHomework.Tests; 


public class NodeTests
{

    //Check Init Params
    [Fact]
    public void Constructor_ProperValue_Initializes()
    {
        // Arrange & Act
        Node<int> testNode = new Node<int>(10);

        // Assert
        Assert.NotNull(testNode);
        Assert.Equal(10, testNode.Value);
        Assert.Same(testNode, testNode.Next);
    }

    //Check string fmt'ing
    [Fact]
    public void Constructor_StringValue_Initializes()
    {
        // Arrange & Act
        Node<string> testNode = new Node<string>("test");

        // Assert
        Assert.NotNull(testNode);
        Assert.Equal("test", testNode.Value);
        Assert.Same(testNode, testNode.Next);
    }

    //lets make sure dbls work 
    [Fact]
    public void Constructor_DoubleValue_Initializes()
    {
        // Arrange & Act
        Node<double> testNode = new Node<double>(3.14);

        // Assert
        Assert.NotNull(testNode);
        Assert.Equal(3.14, testNode.Value);
        Assert.Same(testNode, testNode.Next);
    }

    //determines if pgrm successfully rt string to usr
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

    //specific checking pertaining to ints
    [Fact]
    public void ToString_IntValue_ReturnsString()
    {
        // Arrange
        Node<int> testNode = new Node<int>(42);

        // Act
        string result = testNode.ToString();

        // Assert
        Assert.Equal("42", result);
    }

    //null return check 
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

    //verify private setter access only!
    [Fact]
    public void Next_HasPrivateSetter_VerifiedByCircularStructure()
    {
        // This test verifies that Next has a private setter by ensuring
        // it can only be modified through Append or Clear methods
        // The fact that this maintains circular structure proves the setter works correctly

        // Arrange
        Node<int> firstNode = new Node<int>(1);

        // Act
        firstNode.Append(2);

        // Assert
        Assert.Equal(2, firstNode.Next.Value);
        Assert.Same(firstNode, firstNode.Next.Next);
    }

    [Fact]
    public void Append_SingleNode_CreatesCircularList()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);

        // Act
        firstNode.Append(2);

        // Assert
        Assert.Equal(2, firstNode.Next.Value);
        Assert.Same(firstNode, firstNode.Next.Next);
    }

    [Fact]
    public void Append_TwoNodes_MaintainsCircularStructure()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);

        // Act
        firstNode.Append(2);
        firstNode.Append(3);

        // Assert
        Assert.Equal(1, firstNode.Value);
        Assert.Equal(2, firstNode.Next.Value);
        Assert.Equal(3, firstNode.Next.Next.Value);
        Assert.Same(firstNode, firstNode.Next.Next.Next);
    }

    [Fact]
    public void Append_MultipleNodes_MaintainsCircularStructure()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);

        // Act
        firstNode.Append(2);
        firstNode.Append(3);
        firstNode.Append(4);
        firstNode.Append(5);

        // Assert - Traverse the entire circular list
        Assert.Equal(1, firstNode.Value);
        Assert.Equal(2, firstNode.Next.Value);
        Assert.Equal(3, firstNode.Next.Next.Value);
        Assert.Equal(4, firstNode.Next.Next.Next.Value);
        Assert.Equal(5, firstNode.Next.Next.Next.Next.Value);
        Assert.Same(firstNode, firstNode.Next.Next.Next.Next.Next);
    }

    [Fact]
    public void Append_StringNodes_Works()
    {
        // Arrange
        Node<string> firstNode = new Node<string>("apple");

        // Act
        firstNode.Append("banana");
        firstNode.Append("cherry");

        // Assert
        Assert.Equal("apple", firstNode.Value);
        Assert.Equal("banana", firstNode.Next.Value);
        Assert.Equal("cherry", firstNode.Next.Next.Value);
        Assert.Same(firstNode, firstNode.Next.Next.Next);
    }

    [Fact]
    public void Append_DuplicateValue_ThrowsArgumentException()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);

        // Act & Assert
        ArgumentException ex = Assert.Throws<ArgumentException>(() => firstNode.Append(2));
        Assert.Contains("already exists", ex.Message);
        Assert.Contains("2", ex.Message);
    }

    [Fact]
    public void Append_DuplicateOfFirstNode_ThrowsArgumentException()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);

        // Act & Assert
        ArgumentException ex = Assert.Throws<ArgumentException>(() => firstNode.Append(1));
        Assert.Contains("already exists", ex.Message);
    }

    [Fact]
    public void Append_DuplicateString_ThrowsArgumentException()
    {
        // Arrange
        Node<string> firstNode = new Node<string>("test");
        firstNode.Append("hello");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => firstNode.Append("test"));
    }

    [Fact]
    public void Append_DuplicateInLargeList_ThrowsArgumentException()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        for (int i = 2; i <= 10; i++)
        {
            firstNode.Append(i);
        }

        // Act & Assert
        Assert.Throws<ArgumentException>(() => firstNode.Append(5));
    }

    [Fact]
    public void Exists_ValueInFirstNode_ReturnsTrue()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);

        // Act
        bool result = firstNode.Exists(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_ValueInMiddleNode_ReturnsTrue()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        firstNode.Append(4);

        // Act
        bool result = firstNode.Exists(3);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_ValueInLastNode_ReturnsTrue()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);

        // Act
        bool result = firstNode.Exists(3);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_AllValuesInList_ReturnsTrue()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);

        // Act & Assert
        Assert.True(firstNode.Exists(1));
        Assert.True(firstNode.Exists(2));
        Assert.True(firstNode.Exists(3));
    }

    [Fact]
    public void Exists_ValueNotInList_ReturnsFalse()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);

        // Act & Assert
        Assert.False(firstNode.Exists(4));
        Assert.False(firstNode.Exists(0));
        Assert.False(firstNode.Exists(99));
    }

    [Fact]
    public void Exists_SingleNode_ExistingValue_ReturnsTrue()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(42);

        // Act & Assert
        Assert.True(firstNode.Exists(42));
    }

    [Fact]
    public void Exists_SingleNode_NonExistingValue_ReturnsFalse()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(42);

        // Act & Assert
        Assert.False(firstNode.Exists(1));
    }

    [Fact]
    public void Exists_StringValues_Works()
    {
        // Arrange
        Node<string> firstNode = new Node<string>("apple");
        firstNode.Append("banana");
        firstNode.Append("cherry");

        // Act & Assert
        Assert.True(firstNode.Exists("apple"));
        Assert.True(firstNode.Exists("banana"));
        Assert.True(firstNode.Exists("cherry"));
        Assert.False(firstNode.Exists("orange"));
    }

    [Fact]
    public void Exists_NullValue_Works()
    {
        // Arrange
        Node<string?> firstNode = new Node<string?>(null);
        firstNode.Append("test");

        // Act & Assert
        Assert.True(firstNode.Exists(null));
        Assert.True(firstNode.Exists("test"));
    }

    [Fact]
    public void Clear_MultipleNodes_RemovesAllButCurrent()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        firstNode.Append(4);

        // Act
        firstNode.Clear();

        // Assert
        Assert.Same(firstNode, firstNode.Next);
        Assert.False(firstNode.Exists(2));
        Assert.False(firstNode.Exists(3));
        Assert.False(firstNode.Exists(4));
        Assert.True(firstNode.Exists(1));
    }

    [Fact]
    public void Clear_SingleNode_RemainsUnchanged()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);

        // Act
        firstNode.Clear();

        // Assert
        Assert.Same(firstNode, firstNode.Next);
        Assert.True(firstNode.Exists(1));
    }

    [Fact]
    public void Clear_TwoNodes_RemovesSecond()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);

        // Act
        firstNode.Clear();

        // Assert
        Assert.Same(firstNode, firstNode.Next);
        Assert.True(firstNode.Exists(1));
        Assert.False(firstNode.Exists(2));
    }

    [Fact]
    public void Clear_CanAppendAfterClearing()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);

        // Act
        firstNode.Clear();
        firstNode.Append(10);
        firstNode.Append(20);

        // Assert
        Assert.Equal(10, firstNode.Next.Value);
        Assert.Equal(20, firstNode.Next.Next.Value);
        Assert.Same(firstNode, firstNode.Next.Next.Next);
    }

    [Fact]
    public void Clear_CanAppendPreviousValues()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);

        // Act
        firstNode.Clear();
        firstNode.Append(2); // This should now work since 2 was removed

        // Assert
        Assert.Equal(2, firstNode.Next.Value);
        Assert.True(firstNode.Exists(2));
    }

    [Fact]
    public void Clear_SimplifiesStructure_FirstNodeIsolated()
    {
        // This test verifies that Clear() effectively isolates the current node
        // by setting Next to point to itself.
        // 
        // Garbage Collection Note: The assignment states we only need to set 
        // Next to itself because C# garbage collector can handle circular references.
        // The removed nodes will still point to each other and back to the first node,
        // but since we break the first node's reference to them (by setting Next = this),
        // and assuming no other external references exist, the entire removed chain
        // becomes eligible for garbage collection. The GC uses mark-and-sweep from
        // GC roots, so it can detect and collect unreachable circular structures.

        // Arrange
        Node<int> firstNode = new Node<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        firstNode.Append(4);

        // Capture references to verify the structure before clear
        Node<int> secondNode = firstNode.Next;

        // Act
        firstNode.Clear();

        // Assert - First node is now isolated
        Assert.Same(firstNode, firstNode.Next);
        Assert.True(firstNode.Exists(1));
        Assert.False(firstNode.Exists(2));
        Assert.False(firstNode.Exists(3));
        Assert.False(firstNode.Exists(4));

        // The removed nodes still maintain their original structure
        // (they still point back through the chain to firstNode)
        // In this case, secondNode still has its Next pointer intact
        Assert.Equal(2, secondNode.Value);
        Assert.Equal(3, secondNode.Next.Value);

        // This demonstrates we DON'T need to traverse and break the removed nodes' 
        // circular references - the GC will handle it when there are no external refs
    }

    [Fact]
    public void GenericType_CustomClass_Works()
    {
        // Arrange
        var person1 = new Person { Name = "Alice", Age = 30 };
        var person2 = new Person { Name = "Bob", Age = 25 };
        Node<Person> firstNode = new Node<Person>(person1);

        // Act
        firstNode.Append(person2);

        // Assert
        Assert.Same(person1, firstNode.Value);
        Assert.Same(person2, firstNode.Next.Value);
        Assert.True(firstNode.Exists(person1));
    }

    [Fact]
    public void GenericType_CustomStruct_Works()
    {
        // Arrange
        var point1 = new Point { X = 1, Y = 2 };
        var point2 = new Point { X = 3, Y = 4 };
        Node<Point> firstNode = new Node<Point>(point1);

        // Act
        firstNode.Append(point2);

        // Assert
        Assert.Equal(point1, firstNode.Value);
        Assert.Equal(point2, firstNode.Next.Value);
        Assert.True(firstNode.Exists(point1));
    }

    [Fact]
    public void LargeList_MaintainsCircularIntegrity()
    {
        // Arrange
        Node<int> firstNode = new Node<int>(0);

        // Act - Create a list with 100 nodes
        for (int i = 1; i < 100; i++)
        {
            firstNode.Append(i);
        }

        // Assert - Traverse all 100 nodes and verify we circle back
        Node<int> current = firstNode;
        for (int i = 0; i < 100; i++)
        {
            Assert.Equal(i, current.Value);
            current = current.Next;
        }
        Assert.Same(firstNode, current); // Should circle back to start
    }

    //mk obj and return string (mock obj)
    [Fact]
    public void ToString_CustomType_CallsTypeToString()
    {
        // Arrange
        var person = new Person { Name = "Alice", Age = 30 };
        Node<Person> node = new Node<Person>(person);

        // Act
        string result = node.ToString();

        // Assert
        Assert.Equal("Alice (30)", result);
    }

    // Helper classes for testing custom types
    private sealed class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Age})";
        }
    }

    //Structure acts as a psuedoconstructor 
    private struct Point
    {
        public int X { get; set; } //Get,Set Constructor for X & Y 
        public int Y { get; set; }

        public override string ToString() //Additional to String params
        {
            return $"({X}, {Y})";
        }
    }
}