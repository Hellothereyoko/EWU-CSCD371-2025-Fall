using System;
namespace CanHazFunny;

class Program
{
    static void Main(string[] args)
    {

        //Ask if they wanna hear a joke
        Console.Clear();
        Console.Write("Wanna hear a joke? (y/n): ");

        //Get their answer
        string answer = Console.ReadLine() ?? "n";
        
        //If they say no, exit program gracefully
        if (answer.ToLower() != "y")
        {
            Console.WriteLine("Aww, come on! You're no fun!");
            System.Environment.Exit(0);
            return;
        }

        //Feel free to use your own setup here - this is just provided as an example
        //new Jester(new SomeReallyCoolOutputClass(), new SomeJokeServiceClass()).TellJoke();
        Console.Clear();
        new Jester(new ConsoleOutputService(), new JokeService()).TellJoke();
        return;

    }
}
