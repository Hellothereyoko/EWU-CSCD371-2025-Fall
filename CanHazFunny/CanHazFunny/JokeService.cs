using System.Net.Http;


namespace CanHazFunny;

public class JokeService : IJokeService
{
    private HttpClient HttpClient { get; } = new();

    public string GetJoke()
    {
        /*//Could use a bool to repeat this function until a joke w/o "Chuck Norris" is present

        string joke = HttpClient.GetStringAsync("https://geek-jokes.sameerkumar.website/api").Result;


        if (joke.Contains("Chuck Norris") || joke.Contains("Chuck") || joke.Contains("Norris"))
        {

            joke = "No Chuck Norris Jokes!";
            return joke;
        }
        else
            return joke;*/

        while (true)
        {
            string joke = HttpClient.GetStringAsync("https://geek-jokes.sameerkumar.website/api").Result;
            
            if (!joke.Contains("Chuck Norris"))
            {
                return joke;
            }
            //If jokes contain "Chuck Norris", loop again to get another joke
        }   
    }
}
