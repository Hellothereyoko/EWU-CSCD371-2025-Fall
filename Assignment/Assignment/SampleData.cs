

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
            // FIX: Call shared CsvParser method
            .Select(CsvParser.ParseStateFromCsvRow)
            .Distinct()
            .OrderBy(state => state);
    }

// ... (methods between)

    public IEnumerable<IPerson> People
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

    

   
}