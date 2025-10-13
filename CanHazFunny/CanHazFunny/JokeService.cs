using System.Net.Http;
using System.Text.Json;
using System;

namespace CanHazFunny;

public class JokeService : IJokeService
{
    private HttpClient HttpClient { get; } = new();

    public string GetJoke()
    {
        try
        {
            // NEW: No filtering or looping here. Just get one joke.
            return GetJokeFromApi();
        }
        // Catch specific network/JSON exceptions (Covers the required branch)
        catch (Exception ex) when (ex is HttpRequestException || ex is JsonException) 
        {
            return "Error retrieving joke. Please try again later";
        }
    }

    protected virtual string GetJokeFromApi()
    {
        // This is the production code that makes the actual network call
        string json = HttpClient.GetStringAsync("https://geek-jokes.sameerkumar.website/api?format=json").Result;

        using var doc = JsonDocument.Parse(json);

        return doc.RootElement.GetProperty("joke").GetString() ?? "No joke found";
    }
}