using System;
using System.Threading.Tasks; // You may need this depending on other code, but including System is enough for StringComparison
// using System.Globalization; // Not needed if using StringComparison.OrdinalIgnoreCase

namespace CanHazFunny;

// FIX 1: CA1852 - Added 'sealed' keyword to the class declaration.
sealed class Program
{
    static void Main(string[] args)
    {

        //Ask if they wanna hear a joke
        Console.Clear();
        Console.Write("Wanna hear a joke? (y/n): ");

        //Get their answer
        string answer = Console.ReadLine() ?? "n";
        
        //If they say no, exit program gracefully
        // FIX 2: CA1304, CA1311, CA1862 - Replaced .ToLower() != "y" with String.Equals for culture-insensitive comparison.
        if (!string.Equals(answer, "y", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Aww, come on! You're no fun!");
            System.Environment.Exit(0);
            return;
        }

        //Feel free to use your own setup here - this is just provided as an example
        //new Jester(new SomeReallyCoolOutputClass(), new SomeJokeServiceClass()).TellJoke();
        Console.Clear();
        new Jester(new ConsoleOutput(), new JokeService()).TellJoke();
        return;

    }
}