namespace GenericsHomework;

public class Node<T>
{
    public T Value { get; set; }

    public Node<T> Next { get; private set; }

    public Node(T value)
    {
        Value = value;
        Next = this;
    }

    public override string ToString()
    {
        return Value?.ToString() ?? String.Empty;
    }

    public void Append(T value)
    {
        Node<T> newNode = new Node<T>(value);
        Next = newNode;
    }
}
