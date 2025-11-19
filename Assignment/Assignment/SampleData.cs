using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assignment;

public class SampleData : ISampleData
{
    private readonly string _csvFilePath;

    public SampleData(string csvFilePath)
    {
        _csvFilePath = csvFilePath;
    }

    // 1. Implement CsvRows (Skipping header, handling file reading)
    public IEnumerable<string> CsvRows
    {
        get
        {
            // File.ReadLines lazily evaluates and handles IDisposable for the stream
            return File.ReadLines(_csvFilePath).Skip(1); 
        }
    }

    // 2. Unique Sorted List of States
    public IEnumerable<string> GetUniqueSortedListOfStatesGivenCsvRows()
    {
        return CsvRows
            .Select(ParseStateFromCsvRow)
            .Distinct()
            .OrderBy(state => state);
    }

    // 3. Aggregate Sorted List of States (String)
    public string GetAggregateSortedListOfStatesUsingCsvRows()
    {
        var states = GetUniqueSortedListOfStatesGivenCsvRows().ToArray();
        return string.Join(", ", states);
    }

    // 4. People Collection
    public IEnumerable<IPerson> People
    {
        get
        {
            return CsvRows
                .Select(ParsePersonFromCsvRow)
                .OrderBy(p => p.Address.State)
                .ThenBy(p => p.Address.City)
                .ThenBy(p => p.Address.Zip);
        }
    }

    // 5. Filter By Email
    public IEnumerable<(string FirstName, string LastName)> FilterByEmailAddress(Predicate<string> filter)
    {
        return People
            .Where(p => filter(p.EmailAddress))
            .Select(p => (p.FirstName, p.LastName));
    }

    // 6. Aggregate using .Aggregate() LINQ method
    public string GetAggregateListOfStatesGivenPeopleCollection(IEnumerable<IPerson> people)
    {
        var distinctStates = people
            .Select(p => p.Address.State)
            .Distinct()
            .OrderBy(s => s) // Ensure consistent ordering
            .ToList();

        if (!distinctStates.Any()) return string.Empty;

        // Using Aggregate as explicitly requested
        return distinctStates.Aggregate((current, next) => $"{current}, {next}");
    }

    // Helpers to parse CSV
    private static string ParseStateFromCsvRow(string row)
    {
        var parts = row.Split(',');
        // Assuming Index 5 is State (First, Last, Email, Street, City, State, Zip)
        return parts.Length > 5 ? parts[5].Trim() : string.Empty;
    }

    private static IPerson ParsePersonFromCsvRow(string row)
    {
        var parts = row.Split(',');
        // Simple parsing logic; in production use a CSV library
        var address = new Address(
            parts.Length > 3 ? parts[3].Trim() : "",
            parts.Length > 4 ? parts[4].Trim() : "",
            parts.Length > 5 ? parts[5].Trim() : "",
            parts.Length > 6 ? parts[6].Trim() : ""
        );

        return new Person(
            parts.Length > 0 ? parts[0].Trim() : "",
            parts.Length > 1 ? parts[1].Trim() : "",
            parts.Length > 2 ? parts[2].Trim() : "",
            address
        );
    }
}