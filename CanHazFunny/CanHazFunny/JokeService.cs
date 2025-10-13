using System;
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
                // Call the new virtual method
                string joke = GetJokeFromApi(); 

                // If jokes contain "Chuck Norris", loop again to get another joke
                if (!joke.Contains("Chuck Norris"))
                {
                    return joke;
                }
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is JsonException) // Catch specific exceptions
            {
                return "Error retrieving joke. Please try again later";
            }
        }
    }

    // FIX: New virtual method to encapsulate non-testable code (Allows mocking)
    protected virtual string GetJokeFromApi()
    {
        //Request JSON instead of plain text
        string json = HttpClient.GetStringAsync("https://geek-jokes.sameerkumar.website/api?format=json").Result;

        //Parse JSON - turns the JSON text into a readable object
        using var doc = JsonDocument.Parse(json);

        //Gets the value of the key "joke" and turns that value into C# string
        return doc.RootElement.GetProperty("joke").GetString() ?? "No joke found";
    }
}