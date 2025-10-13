using System;

namespace CanHazFunny;


public class Jester

{
    private readonly IOutput _outputService; // FIX: Renamed interface from IOutputService
    private readonly IJokeService _jokeService;

    // FIX: Renamed interface from IOutputService
    public Jester(IOutput outputService, IJokeService jokeService)
    {
        _outputService = outputService ?? throw new ArgumentNullException(nameof(outputService));
        _jokeService = jokeService ?? throw new ArgumentNullException(nameof(jokeService));
    }

    // FIX: Changed from getJoke() to GetJoke() (PascalCase fix)
    public string GetJoke()
    {
        return _jokeService.GetJoke();
    }

    //tellJoke method that writes the joke to the console
    public void TellJoke()
    {
        string joke = _jokeService.GetJoke();
        // FIX: Changed method call from Write to WriteLine
        _outputService.WriteLine(joke); 

    }


}