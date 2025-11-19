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

    public IEnumerable<string> CsvRows
    {
        get { return File.ReadLines(_csvFilePath).Skip(1); }
    }

    public IEnumerable<string> GetUniqueSortedListOfStatesGivenCsvRows()
    {
        return CsvRows
            .Select(ParseStateFromCsvRow)
            .Distinct()
            .OrderBy(state => state);
    }

    public string GetAggregateSortedListOfStatesUsingCsvRows()
    {
        var states = GetUniqueSortedListOfStatesGivenCsvRows().ToArray();
        return string.Join(", ", states);
    }

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

    public IEnumerable<(string FirstName, string LastName)> FilterByEmailAddress(Predicate<string> filter)
    {
        return People
            .Where(p => filter(p.EmailAddress))
            .Select(p => (p.FirstName, p.LastName));
    }

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

    private static string ParseStateFromCsvRow(string row)
    {
        var parts = row.Split(',');
        return parts.Length > 5 ? parts[5].Trim() : string.Empty;
    }

    private static IPerson ParsePersonFromCsvRow(string row)
    {
        var parts = row.Split(',');
        
        var address = new Address(
            parts.Length > 3 ? parts[3].Trim() : "",
            parts.Length > 4 ? parts[4].Trim() : "",
            parts.Length > 5 ? parts[5].Trim() : "",
            parts.Length > 6 ? parts[6].Trim() : ""
        );

        // FIXED: Passing Address as 3rd arg, Email as 4th arg
        return new Person(
            parts.Length > 0 ? parts[0].Trim() : "",
            parts.Length > 1 ? parts[1].Trim() : "",
            address,                                  
            parts.Length > 2 ? parts[2].Trim() : ""   
        );
    }
}