using System.Net.Http;
using System.Text.Json;


namespace CanHazFunny;

public class JokeService : IJokeService
{
    private HttpClient HttpClient { get; } = new();

    public string GetJoke()
    {

        while (true)
        {
            try
            {
                //Request JSON instead of plain text
                string json = HttpClient.GetStringAsync("https://geek-jokes.sameerkumar.website/api?format=json").Result;

                //Parse JSON - turns the JSON text into a readable object
                using var doc = JsonDocument.Parse(json);

                //Gets the value of the key "joke" and turns that value into C# string
                string joke = doc.RootElement.GetProperty("joke").GetString() ?? "No joke found";

                //If jokes contain "Chuck Norris", loop again to get another joke
                if (!joke.Contains("Chuck Norris"))
                {
                    return joke;
                }
                
            }
            catch
            {
                return "Error retrieving joke. Please try again later";
            }



        }   
    }
}
