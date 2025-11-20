using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System; // <-- ADDED: Needed for StringComparison (keeps my compiler happy)
using Assignment; 

namespace Assignment.Tests;


//Is responsible for testing Node.cs
[TestClass]
public class NodeTests
{
    //Tests for GetEnumerator method
     [TestMethod]
    public void Node_IteratesCircleOnce()
    {
        var node1 = new Node<int>(1);
        var node2 = new Node<int>(2);
        var node3 = new Node<int>(3);

        node1.Next = node2;
        node2.Next = node3;
        node3.Next = node1; // Close circle

        var result = node1.ToList(); //Append all items to a list

        // FIX 7: Swapped arguments to (expectedCount, collection) BUGGED AF IN EARLIER VERSIONS
        Assert.HasCount(3, result);
        Assert.AreEqual(1, result[0]);
        Assert.AreEqual(3, result[2]);
    }


    //Tests for ChildItems method
    [TestMethod]
    public void Node_HandlesSingleNodeCircle()
    {
        var node = new Node<int>(42);
        node.Next = node; // Points to itself   
        var result = node.ToList();

        //Swapped arguments to (expectedCount, collection)
        Assert.HasCount(1, result);
        Assert.AreEqual(42, result[0]);     
    }
}