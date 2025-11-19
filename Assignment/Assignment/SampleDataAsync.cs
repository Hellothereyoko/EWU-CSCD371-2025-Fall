using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment;

public class SampleDataAsync : IAsyncSampleData
{
    private readonly string _csvFilePath;

    public SampleDataAsync(string csvFilePath)
    {
        _csvFilePath = csvFilePath;
    }

    // 1. Async CSV Rows
    public IAsyncEnumerable<string> CsvRows => ReadLinesAsync(_csvFilePath);

    private async IAsyncEnumerable<string> ReadLinesAsync(string path)
    {
        using var reader = new StreamReader(path);
       
        // Skip header
        await reader.ReadLineAsync();
        
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (line != null)
            {
                yield return line;
            }
        }
    }

    // 2. Async Unique States
    public async IAsyncEnumerable<string> GetUniqueSortedListOfStatesGivenCsvRows()
    {
        var states = new HashSet<string>();
        
        await foreach (var row in CsvRows)
        {
            // Reusing logic would technically require extracting ParseState to a static helper class
            // For now, inline or duplicate minimal logic for clarity
            var parts = row.Split(',');
            if (parts.Length > 5) states.Add(parts[5].Trim());
        }

        foreach (var state in states.OrderBy(s => s))
        {
            yield return state;
        }
    }

    // 3. Aggregate String (Async source, sync return string)
    public string GetAggregateSortedListOfStatesUsingCsvRows()
    {
        // Note: The interface defines this as returning string (sync), 
        // but we must consume async data. This usually implies blocking 
        // or that the interface method itself should have been Task<string>.
        // Assuming we must implement the signature as provided:
        var states = GetUniqueSortedListOfStatesGivenCsvRows().ToEnumerable().ToList();
        return string.Join(", ", states);
    }

    // 4. People Async
    public IAsyncEnumerable<IPerson> People
    {
        get
        {
            return CsvRows
                .Select(row => ParsePersonHelper(row))
                .OrderBy(p => p.Address.State)
                .ThenBy(p => p.Address.City)
                .ThenBy(p => p.Address.Zip);
        }
    }

    // 5. Filter Async
    public IAsyncEnumerable<(string FirstName, string LastName)> FilterByEmailAddress(Predicate<string> filter)
    {
        return People
            .Where(p => filter(p.EmailAddress))
            .Select(p => (p.FirstName, p.LastName));
    }

    // 6. Aggregate People
    public string GetAggregateListOfStatesGivenPeopleCollection(IAsyncEnumerable<IPerson> people)
    {
        // Consuming async enumerable synchronously to match interface signature
        var distinctStates = people.ToEnumerable()
            .Select(p => p.Address.State)
            .Distinct()
            .OrderBy(s => s)
            .ToList();

        if (!distinctStates.Any()) return string.Empty;
        return distinctStates.Aggregate((current, next) => $"{current}, {next}");
    }

    // Static helper to share logic between Sync and Async classes
    private static IPerson ParsePersonHelper(string row)
    {
        var parts = row.Split(',');
        return new Person(
            parts[0], parts[1], parts[2],
            new Address(parts[3], parts[4], parts[5], parts[6])
        );
    }
}