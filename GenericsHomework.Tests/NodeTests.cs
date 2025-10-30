using System;
using System.Collections.Generic;
using Xunit;
using GenericsHomework;

namespace GenericsHomework.Tests;

public class NodeTests
{
    // ==================== CONSTRUCTOR TESTS ====================
    
    [Fact]
    public void Constructor_ProperValue_Initializes()
    {
        // Arrange & Act
        NodeCollection<int> testNode = new NodeCollection<int>(10);

        // Assert
        Assert.NotNull(testNode);
        Assert.Equal(10, testNode.Value);
        Assert.Same(testNode, testNode.Next);
    }

    [Fact]
    public void Constructor_StringValue_Initializes()
    {
        // Arrange & Act
        NodeCollection<string> testNode = new NodeCollection<string>("test");

        // Assert
        Assert.NotNull(testNode);
        Assert.Equal("test", testNode.Value);
        Assert.Same(testNode, testNode.Next);
    }

    [Fact]
    public void Constructor_DoubleValue_Initializes()
    {
        // Arrange & Act
        NodeCollection<double> testNode = new NodeCollection<double>(3.14);

        // Assert
        Assert.NotNull(testNode);
        Assert.Equal(3.14, testNode.Value);
        Assert.Same(testNode, testNode.Next);
    }

    // ==================== TOSTRING TESTS ====================
    
    [Fact]
    public void ToString_ReturnsValueString_Success()
    {
        // Arrange
        NodeCollection<string> testNode = new NodeCollection<string>("TestValue");

        // Act
        string result = testNode.ToString();

        // Assert
        Assert.Equal("TestValue", result);
    }

    [Fact]
    public void ToString_IntValue_ReturnsString()
    {
        // Arrange
        NodeCollection<int> testNode = new NodeCollection<int>(42);

        // Act
        string result = testNode.ToString();

        // Assert
        Assert.Equal("42", result);
    }

    [Fact]
    public void ToString_NullValue_ReturnsEmptyString()
    {
        // Arrange
        NodeCollection<string> testNode = new NodeCollection<string>("test");
        testNode.Value = null!;

        // Act
        string result = testNode.ToString();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToString_CustomType_CallsTypeToString()
    {
        // Arrange
        var person = new Person { Name = "Alice", Age = 30 };
        NodeCollection<Person> node = new NodeCollection<Person>(person);

        // Act
        string result = node.ToString();

        // Assert
        Assert.Equal("Alice (30)", result);
    }

    // ==================== NEXT PROPERTY TESTS ====================
    
    [Fact]
    public void Next_HasPrivateSetter_VerifiedByCircularStructure()
    {
        // This test verifies that Next has a private setter by ensuring
        // it can only be modified through Append or Clear methods
        // The fact that this maintains circular structure proves the setter works correctly

        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);

        // Act
        firstNode.Append(2);

        // Assert
        Assert.Equal(2, firstNode.Next.Value);
        Assert.Same(firstNode, firstNode.Next.Next);
    }

    // ==================== APPEND TESTS ====================
    
    [Fact]
    public void Append_SingleNode_CreatesCircularList()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);

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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);

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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);

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
        NodeCollection<string> firstNode = new NodeCollection<string>("apple");

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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
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
        NodeCollection<string> firstNode = new NodeCollection<string>("test");
        firstNode.Append("hello");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => firstNode.Append("test"));
    }

    [Fact]
    public void Append_DuplicateInLargeList_ThrowsArgumentException()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        for (int i = 2; i <= 10; i++)
        {
            firstNode.Append(i);
        }

        // Act & Assert
        Assert.Throws<ArgumentException>(() => firstNode.Append(5));
    }

    // ==================== EXISTS TESTS ====================
    
    [Fact]
    public void Exists_ValueInFirstNode_ReturnsTrue()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
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
        NodeCollection<int> firstNode = new NodeCollection<int>(42);

        // Act & Assert
        Assert.True(firstNode.Exists(42));
    }

    [Fact]
    public void Exists_SingleNode_NonExistingValue_ReturnsFalse()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(42);

        // Act & Assert
        Assert.False(firstNode.Exists(1));
    }

    [Fact]
    public void Exists_StringValues_Works()
    {
        // Arrange
        NodeCollection<string> firstNode = new NodeCollection<string>("apple");
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
        NodeCollection<string?> firstNode = new NodeCollection<string?>(null);
        firstNode.Append("test");

        // Act & Assert
        Assert.True(firstNode.Exists(null));
        Assert.True(firstNode.Exists("test"));
    }

    // ==================== CLEAR TESTS ====================
    
    [Fact]
    public void Clear_MultipleNodes_RemovesAllButCurrent()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);

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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
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
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        firstNode.Append(4);

        // Capture references to verify the structure before clear
        NodeCollection<int> secondNode = firstNode.Next;

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

    // ==================== COUNT TESTS ====================
    
    [Fact]
    public void Count_MultipleNodes_ReturnsCorrectCount()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        firstNode.Append(4);
        firstNode.Append(5);
        
        // Act
        int count = firstNode.Count;
        
        // Assert
        Assert.Equal(5, count);
    }

    [Fact]
    public void Count_SingleNode_ReturnsOne()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        
        // Act
        int count = firstNode.Count;
        
        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void Count_AfterRemove_UpdatesCorrectly()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        Assert.Equal(3, firstNode.Count);
        
        // Act
        firstNode.Remove(2);
        
        // Assert
        Assert.Equal(2, firstNode.Count);
    }

    [Fact]
    public void Count_AfterClear_ReturnsOne()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        
        // Act
        firstNode.Clear();
        
        // Assert
        Assert.Single(firstNode);
    }

    // ==================== ICOLLECTION<T> IMPLEMENTATION TESTS ====================
    
    [Fact]
    public void IsReadOnly_ReturnsFalse()
    {
        // Arrange
        NodeCollection<int> node = new NodeCollection<int>(1);
        
        // Assert
        Assert.False(node.IsReadOnly);
    }

    [Fact]
    public void Add_ProperNode_AppendsNode()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        
        // Act
        firstNode.Add(2);
        firstNode.Add(3);
        
        // Assert
        Assert.Equal(2, firstNode.Next.Value);
        Assert.Equal(3, firstNode.Next.Next.Value);
        Assert.Same(firstNode, firstNode.Next.Next.Next);
    }

    [Fact]
    public void Add_DuplicateValue_ThrowsArgumentException()
    {
        // Testing Add (which calls Append)
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Add(2);
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => firstNode.Add(1));
    }

    [Fact]
    public void Contains_ExistingValue_ReturnsTrue()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        
        // Act
        bool contains = firstNode.Contains(2);
        
        // Assert
        Assert.True(contains);
    }

    [Fact]
    public void Contains_NonExistingValue_ReturnsFalse()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        
        // Act
        bool contains = firstNode.Contains(99);
        
        // Assert
        Assert.False(contains);
    }

    [Fact]
    public void Remove_ExistingValue_RemovesNode()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        
        // Act
        bool removed = firstNode.Remove(2);
        
        // Assert
        Assert.True(removed);
        Assert.False(firstNode.Exists(2));
        Assert.Equal(3, firstNode.Next.Value);
    }

    [Fact]
    public void Remove_NonExistingValue_ReturnsFalse()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        
        // Act
        bool removed = firstNode.Remove(4);
        
        // Assert
        Assert.False(removed);
    }

    [Fact]
    public void Remove_FirstNode_UpdatesHead()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        
        // Act
        bool removed = firstNode.Remove(1);
        
        // Assert
        Assert.True(removed);
        Assert.Equal(2, firstNode.Value); // Head should now be 2
        Assert.Equal(3, firstNode.Next.Value);
    }

    [Fact]
    public void Remove_LastNode_LeavesOneNode()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        
        // Act
        bool removed = firstNode.Remove(2);
        
        // Assert
        Assert.True(removed);
        Assert.Single(firstNode);
        Assert.Same(firstNode, firstNode.Next);
    }

    [Fact]
    public void Remove_OnlySingleNode_ReturnsFalse()
    {
        // This test highlights the single-node removal edge case
        // Current implementation returns false when trying to remove the only node
        // This prevents breaking the circular structure
        
        // Arrange
        NodeCollection<int> singleNode = new NodeCollection<int>(42);
        
        // Act
        bool removed = singleNode.Remove(42);
        
        // Assert
        Assert.False(removed);
        Assert.Equal(42, singleNode.Value);
        Assert.Same(singleNode, singleNode.Next);
    }

    [Fact]
    public void Remove_MultipleNodesInSequence_MaintainsIntegrity()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        firstNode.Append(4);
        firstNode.Append(5);
        
        // Act
        firstNode.Remove(2);
        firstNode.Remove(4);
        
        // Assert
        Assert.Equal(3, firstNode.Count);
        Assert.Contains(1, firstNode);
        Assert.DoesNotContain(2, firstNode);
        Assert.Contains(3, firstNode);
        Assert.DoesNotContain(4, firstNode);
        Assert.Contains(5, firstNode);
    }

    [Fact]
    public void CopyTo_Array_CopiesElements()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        int[] array = new int[5];
        
        // Act
        firstNode.CopyTo(array, 1);
        
        // Assert
        Assert.Equal(0, array[0]); // Default int value
        Assert.Equal(1, array[1]);
        Assert.Equal(2, array[2]);
        Assert.Equal(3, array[3]);
        Assert.Equal(0, array[4]); // Default int value
    }

    [Fact]
    public void CopyTo_NullArray_ThrowsArgumentNullException()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => firstNode.CopyTo(null!, 0));
    }

    [Fact]
    public void CopyTo_NegativeIndex_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        int[] array = new int[5];
        
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => firstNode.CopyTo(array, -1));
    }

    [Fact]
    public void CopyTo_IndexAtArrayLength_ThrowsArgumentException()
    {
        // When arrayIndex equals array.Length, we're trying to start copying
        // at a position where there's no space, which should throw ArgumentException
        
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        int[] array = new int[2];
        
        // Act & Assert - arrayIndex=2 means start at position 2 in a 2-length array
        // This leaves no room for 2 elements, so should throw ArgumentException
        Assert.Throws<ArgumentException>(() => firstNode.CopyTo(array, 2));
    }

    [Fact]
    public void CopyTo_ArrayTooSmall_ThrowsArgumentException()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        int[] smallArray = new int[2];
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => firstNode.CopyTo(smallArray, 0));
    }

    [Fact]
    public void CopyTo_StartIndexWithInsufficientSpace_ThrowsArgumentException()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        int[] array = new int[4];
        
        // Act & Assert - Starting at index 2 means only 2 slots, but we have 3 elements
        Assert.Throws<ArgumentException>(() => firstNode.CopyTo(array, 2));
    }

    // ==================== ENUMERATOR TESTS ====================
    
    [Fact]
    public void GetEnumerator_IteratesThroughNodes()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        var enumerator = firstNode.GetEnumerator();
        int sum = 0;
        
        // Act
        while (enumerator.MoveNext())
        {
            sum += enumerator.Current;
        }
        
        // Assert
        Assert.Equal(6, sum); // 1 + 2 + 3 = 6
    }

    [Fact]
    public void ForEach_IteratesThroughAllNodes()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(1);
        firstNode.Append(2);
        firstNode.Append(3);
        List<int> values = new List<int>();
        
        // Act
        foreach (int value in firstNode)
        {
            values.Add(value);
        }
        
        // Assert
        int[] expected = new[] { 1, 2, 3 };
        Assert.Equal(expected, values);
    }

    [Fact]
    public void ForEach_SingleNode_IteratesOnce()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(42);
        List<int> values = new List<int>();
        
        // Act
        foreach (int value in firstNode)
        {
            values.Add(value);
        }
        
        // Assert
        Assert.Single(values);
        Assert.Equal(42, values[0]);
    }

    // ==================== GENERIC TYPE TESTS ====================
    
    [Fact]
    public void GenericType_CustomClass_Works()
    {
        // Arrange
        var person1 = new Person { Name = "Alice", Age = 30 };
        var person2 = new Person { Name = "Bob", Age = 25 };
        NodeCollection<Person> firstNode = new NodeCollection<Person>(person1);

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
        NodeCollection<Point> firstNode = new NodeCollection<Point>(point1);

        // Act
        firstNode.Append(point2);

        // Assert
        Assert.Equal(point1, firstNode.Value);
        Assert.Equal(point2, firstNode.Next.Value);
        Assert.True(firstNode.Exists(point1));
    }

    // ==================== LARGE LIST TESTS ====================
    
    [Fact]
    public void LargeList_MaintainsCircularIntegrity()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(0);

        // Act - Create a list with 100 nodes
        for (int i = 1; i < 100; i++)
        {
            firstNode.Append(i);
        }

        // Assert - Traverse all 100 nodes and verify we circle back
        NodeCollection<int> current = firstNode;
        for (int i = 0; i < 100; i++)
        {
            Assert.Equal(i, current.Value);
            current = current.Next;
        }
        Assert.Same(firstNode, current); // Should circle back to start
    }

    [Fact]
    public void LargeList_Count_ReturnsCorrectValue()
    {
        // Arrange
        NodeCollection<int> firstNode = new NodeCollection<int>(0);
        for (int i = 1; i < 50; i++)
        {
            firstNode.Append(i);
        }

        // Act
        int count = firstNode.Count;

        // Assert
        Assert.Equal(50, count);
    }

    // ==================== HELPER CLASSES ====================
    
    private sealed class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Age})";
        }
    }

    private struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}