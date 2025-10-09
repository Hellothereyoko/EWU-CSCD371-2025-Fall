using System.Net.Http;


namespace CanHazFunny;

public class JokeService : IJokeService
{
    private HttpClient HttpClient { get; } = new();

    public string GetJoke()
    {
        string joke = HttpClient.GetStringAsync("https://geek-jokes.sameerkumar.website/api").Result;

        if (joke.Contains("Chuck Norris"))
        {
            joke = "No Chuck Norris Jokes!";
            return joke;
        }
        return joke;
    }
}
