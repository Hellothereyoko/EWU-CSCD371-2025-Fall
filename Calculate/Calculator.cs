namespace Calculate;

public class Calculator
{
    public static int Add(int a, int b, out int result) => result = a + b;

    public static int Subtract(int a, int b, out int result) => result = a - b;

    public static int Multiple(int a, int b, out int result) => result = a * b;

    public static double Divide(int a, int b, out double result)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Denominator cannot be zero.");
        }
        return result = a / b;
    }
}
