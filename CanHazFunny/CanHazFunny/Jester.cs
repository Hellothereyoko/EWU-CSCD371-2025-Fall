namespace CanHazFunny;


public class Jester(IOutputService outputService, IJokeService jokeService)

{
   

    //getJoke method that returns a string
    internal string getJoke()
    {
        return jokeService.GetJoke();
    }


    //tellJoke method that writes the joke to the console
    internal void TellJoke()
    {
        outputService.Write(getJoke());
        return;

    }


}