using System.Collections;
using System.Collections.Generic;

namespace Assignment;

public class Node<T> : IEnumerable<T>
{
    public T Data { get; }
    public Node<T> Next { get; set; }

    public Node(T data)
    {
        Data = data;
        Next = this; // Circular reference by default
    }

    // Task 7: Implements IEnumerable to return all items in the "circle"
    public IEnumerator<T> GetEnumerator()
    {
        yield return Data;
        var current = Next;
        // Stops after one full rotation
        while (current != null && current != this) 
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Task 7: Returns remaining items with a maximum number of items returned
    public IEnumerable<T> ChildItems(int maximum)
    {
        var current = Next;
        int count = 0;
        
        // FIX: Added current != this check to prevent infinite loop on circular lists
        while (current != null && current != this && count < maximum) 
        {
            yield return current.Data;
            current = current.Next;
            count++;
        }
    }
}