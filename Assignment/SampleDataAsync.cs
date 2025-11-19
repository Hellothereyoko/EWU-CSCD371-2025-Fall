using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // This now includes System.Linq.Async extension methods
using System.Threading.Tasks;

namespace Assignment;

public class SampleDataAsync : IAsyncSampleData
{
    private readonly string _csvFilePath;

    public SampleDataAsync(string csvFilePath)
    {
        _csvFilePath = csvFilePath;
    }

    public IAsyncEnumerable<string> CsvRows => ReadLinesAsync(_csvFilePath);

    private async IAsyncEnumerable<string> ReadLinesAsync(string path)
    {
        using var reader = new StreamReader(path);
        await reader.ReadLineAsync(); // Skip header
        
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (line != null) yield return line;
        }
    }

    public async IAsyncEnumerable<string> GetUniqueSortedListOfStatesGivenCsvRows()
    {
        var states = new HashSet<string>();
        
        await foreach (var row in CsvRows)
        {
            // FIX: Use shared CsvParser instead of inline logic
            states.Add(CsvParser.ParseStateFromCsvRow(row)); 
        }

        foreach (var state in states.OrderBy(s => s))
        {
            yield return state;
        }
    }

// ... (methods between)

    public IAsyncEnumerable<IPerson> People
    {
        get
        {
            return CsvRows
                // FIX: Call shared CsvParser method
                .Select(CsvParser.ParsePersonFromCsvRow)
                .OrderBy(p => p.Address.State)
                .ThenBy(p => p.Address.City)
                .ThenBy(p => p.Address.Zip);
        }
    }

    public string GetAggregateSortedListOfStatesUsingCsvRows()
    {
        // ToEnumerable() requires System.Linq.Async package
        var states = GetUniqueSortedListOfStatesGivenCsvRows().ToEnumerable().ToList();
        return string.Join(", ", states);
    }


    public IAsyncEnumerable<(string FirstName, string LastName)> FilterByEmailAddress(Predicate<string> filter)
    {
        return People
            .Where(p => filter(p.EmailAddress))
            .Select(p => (p.FirstName, p.LastName));
    }

    public string GetAggregateListOfStatesGivenPeopleCollection(IAsyncEnumerable<IPerson> people)
    {
        var distinctStates = people.ToEnumerable()
            .Select(p => p.Address.State)
            .Distinct()
            .OrderBy(s => s)
            .ToList();

        if (!distinctStates.Any()) return string.Empty;
        return distinctStates.Aggregate((current, next) => $"{current}, {next}");
    }

}
