
using System;

namespace CanHazFunny;


public class Jester

{
    private readonly IOutputService _outputService;
    private readonly IJokeService _jokeService;

    public Jester(IOutputService outputService, IJokeService jokeService)
    {
        _outputService = outputService ?? throw new ArgumentNullException(nameof(outputService));
        _jokeService = jokeService ?? throw new ArgumentNullException(nameof(jokeService));
    }

    public string getJoke()
    {
        return _jokeService.GetJoke();
    }

    //tellJoke method that writes the joke to the console
    public void TellJoke()
    {
        string joke = _jokeService.GetJoke();
        _outputService.Write(joke);

    }


}