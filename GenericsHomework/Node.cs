using System;
using System.Collections.Generic;

namespace GenericsHomework;

public class Node<T>
{
    public T Value { get; set; } //Get-Set T-Value

    public Node<T> Next { get; private set; } //Make next a private init obj

    public Node(T value) //Instantiate values for best access practice 
    {
        Value = value;
        Next = this;
    }

    public override string ToString()
    {
        return Value?.ToString() ?? String.Empty; //checks if val present: if not, it returns an empty string to be handled
    }

    /// <summary>
    /// Appends a new node with the specified value to the circular list.
    /// Throws ArgumentException if the value already exists in the list.
    /// </summary>
    public void Append(T value)
    {
        // Check for duplicates before appending
        if (Exists(value))
        {
            throw new ArgumentException($"Value '{value}' already exists in the list.", nameof(value)); //ERR CONDITION
        }

        Node<T> newNode = new Node<T>(value);
        
        // Find the last node in the circular list (the one that points back to this)
        Node<T> current = this;
        while (current.Next != this)
        {
            current = current.Next;
        }
        
        // Insert new node between last node and this (first) node
        current.Next = newNode;
        newNode.Next = this;
    }

    /// <summary>
    /// Checks if a value exists anywhere in the circular list.
    /// </summary>
    public bool Exists(T value)
    {
        Node<T> current = this;

        // Check first node
        if (EqualityComparer<T>.Default.Equals(current.Value, value))
        {
            return true;
        }

        // Traverse the rest of the circular list
        current = current.Next;
        while (current != this)
        {
            if (EqualityComparer<T>.Default.Equals(current.Value, value))
            {
                return true;
            }
            current = current.Next;
        }

        return false;
    }


    /// Garbage Collection Note:
    /// -----------------------------------------------------------------------
    /// We only need to set Next to itself because in C#, the garbage collector
    /// can detect and collect circular references. Even though removed nodes
    /// form a circular chain pointing to each other, they will be collected
    /// once there are no external references to any node in that chain.
    /// The GC uses a mark-and-sweep algorithm that traces from GC roots,
    /// so unreachable circular structures are properly collected.
    /// Therefore, we don't need to manually break the removed nodes' circular
    /// references - the GC will handle them automatically.
    



    /// Rm all nodes from the list except the current node.
    /// Sets Next to point to itself, effectively isolating this node.
    public void Clear()
    {
        // Simply set Next to this, breaking connection to other nodes
        // The removed nodes will form their own circular chain with no external references
        Next = this;
        
        // No need to traverse & break chain
        // The GC will collect them as they're now unreachable
    }
}