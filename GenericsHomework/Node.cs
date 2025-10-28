using System;
using System.Collections;
using System.Collections.Generic;

namespace GenericsHomework;

public class Node<T> : System.Collections.Generic.ICollection<T>, IEnumerable<T>, IEnumerable
{
    public T Value { get; set; } //Get-Set T-Value

    public Node<T> Next { get; private set; } //Make next a private init obj
    public int Count
    {
        get
        {
            int count = 1; // Start with 1 for the current node
            Node<T> current = this.Next;
            // Traverse the circular list until we loop back to the starting node
            while (current != this)
            {
                count++;
                current = current.Next;
            }
            return count;
        }
    }

    public bool IsReadOnly => false; //Collection is mutable

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

    public void Add(T item)
    {
        Append(item);
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

    public bool Contains(T item)
    {
        return Exists(item);
    }

    public bool Remove(T item)
    {
        // Case 1: If the item to remove is in the first node (this)
        if (EqualityComparer<T>.Default.Equals(Value, item))
        {
            if (Next == this)
            {
                // Special case: only one node in the list
                // Cannot remove the only node as it would break the circular structure
                return false;
            }

            // Copy the next node's value to this node and remove the next node instead
            Value = Next.Value;
            Next = Next.Next;
            return true;
        }

        // Case 2: Look for the item in the rest of the list
        Node<T> current = this;
        while (current.Next != this)
        {
            if (EqualityComparer<T>.Default.Equals(current.Next.Value, item))
            {
                // Remove the found node by updating the Next reference
                current.Next = current.Next.Next;
                return true;
            }
            current = current.Next;
        }

        // Item not found
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        if (arrayIndex < 0 || arrayIndex >= array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        }
        Node<T> current = this;
        int index = arrayIndex;
        do
        {
            if (index >= array.Length)
            {
                throw new ArgumentException("The array is not large enough to hold the elements.", nameof(array));
            }
            array[index] = current.Value;
            index++;
            current = current.Next;
        } while (current != this);
    }

    public IEnumerator<T> GetEnumerator()
    {
        Node<T> current = this;
        do
        {
            yield return current.Value;
            current = current.Next;
        } while (current != this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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