namespace Calculate;

public class Calculator
{
    public static int Add(int a, int b) => a + b;
    public static int Subtract(int a, int b) => a - b;
    public static int Multiple(int a, int b) => a * b;
    public static int Divide(int a, int b) => a / b;

    public IReadOnlyDictionary<char, Func<int, int, int>> MathematicalOperations { get; } =
        new Dictionary<char, Func<int, int, int>>
        {
            ['+'] = Add,
            ['-'] = Subtract,
            ['*'] = Multiple,
            ['/'] = Divide
        };

    public bool TryCalculate(string calculation, out int result)
    {
        result = 0;
        var parts = calculation.Split(' ');

        if (parts.Length != 3 ||
            !int.TryParse(parts[0], out int left) ||
            !int.TryParse(parts[2], out int right) ||
            parts[1].Length != 1 ||
            !MathematicalOperations.TryGetValue(parts[1][0], out var operation))
        {
            return false;
        }

        result = operation(left, right);
        return true;
    }
}
