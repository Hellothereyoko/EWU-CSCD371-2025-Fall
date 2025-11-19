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

    // 1. Implements CsvRows (skips header)
    public IEnumerable<string> CsvRows
    {
        get { return File.ReadLines(_csvFilePath).Skip(1); }
    }

    // 2. Returns unique and sorted states using CsvParser
    public IEnumerable<string> GetUniqueSortedListOfStatesGivenCsvRows()
    {
        return CsvRows
            .Select(CsvParser.ParseStateFromCsvRow) // Calls shared parser
            .Distinct()
            .OrderBy(state => state);
    }

    // 3. Aggregates unique and sorted states into a comma-separated string
    public string GetAggregateSortedListOfStatesUsingCsvRows()
    {
        var states = GetUniqueSortedListOfStatesGivenCsvRows().ToArray();
        return string.Join(", ", states);
    }

    // 4. Returns Person objects sorted by State, City, and Zip using CsvParser
    public IEnumerable<IPerson> People
    {
        get
        {
            return CsvRows
                .Select(CsvParser.ParsePersonFromCsvRow) // Calls shared parser
                .OrderBy(p => p.Address.State)
                .ThenBy(p => p.Address.City)
                .ThenBy(p => p.Address.Zip);
        }
    }

    // 5. Filters the People collection by email
    public IEnumerable<(string FirstName, string LastName)> FilterByEmailAddress(Predicate<string> filter)
    {
        return People
            .Where(p => filter(p.EmailAddress))
            .Select(p => (p.FirstName, p.LastName));
    }

    // 6. Aggregates the states from the provided people collection
    public string GetAggregateListOfStatesGivenPeopleCollection(IEnumerable<IPerson> people)
    {
        var distinctStates = people
            .Select(p => p.Address.State)
            .Distinct()
            .OrderBy(s => s)
            .ToList();

        if (!distinctStates.Any()) return string.Empty;

        return distinctStates.Aggregate((current, next) => $"{current}, {next}");
    }
}