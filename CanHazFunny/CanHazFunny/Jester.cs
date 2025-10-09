
using System;
using System.IO;
using System.Net.Http;
namespace CanHazFunny;


public class Jester

{

    //getJoke method that returns a string
    string getJoke()
    {
        var jokeService = new JokeService();
        return jokeService.GetJoke();
    }

    //tellJoke method that writes the joke to the console
    void tellJoke()
    {
        Console.WriteLine(getJoke());
    }


}