using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assignment; // Ensure this is here to see SampleData/Node

namespace Assignment.Tests;

[TestClass]
public class SampleDataTests
{
    private const string TestFileName = "People.csv";
    
    // FIX 1: Use 'null!' to satisfy the compiler warning safely
    private SampleData _sampleData = null!;

    [TestInitialize]
    public void Setup()
    {
        // Create dummy CSV object
        var lines = new[]
        {
            "FirstName,LastName,Email,Street,City,State,Zip",
            "John,Doe,john@test.com,123 Main,Seattle,WA,98101",
            "Jane,Smith,jane@test.com,456 Oak,Portland,OR,97201",
            "Bob,Jones,bob@example.com,789 Pine,Spokane,WA,99201"
        };

        File.WriteAllLines(TestFileName, lines);
        
        // Ensure you are NOT using 'var' here (which would create a local variable)
        _sampleData = new SampleData(TestFileName);
    }

    [TestCleanup] 
    public void Cleanup()
    {
        if (File.Exists(TestFileName)) File.Delete(TestFileName);
    }

    // ... Your tests remain the same ...
    [TestMethod]
    public void CsvRows_SkipsHeader_And_ReturnsCorrectCount()
    {
        var rows = _sampleData.CsvRows.ToList();
        Assert.AreEqual(3, rows.Count());
        Assert.IsFalse(rows.Any(r => r.StartsWith("FirstName")));
    }

    [TestMethod]
    public void GetUniqueSortedListOfStates_ReturnsSortedUniqueList()
    {
        var states = _sampleData.GetUniqueSortedListOfStatesGivenCsvRows().ToList();
        Assert.AreEqual("OR", states[0]);
        Assert.AreEqual("WA", states[1]);
        Assert.AreEqual(2, states.Count);
        
        var isSorted = states.Zip(states.Skip(1), (a, b) => string.Compare(a, b) < 0).All(x => x);
        Assert.IsTrue(isSorted, "List was not sorted alphabetically.");
    }

    [TestMethod]
    public void GetAggregateSortedListOfStates_ReturnsCommaSeparatedString()
    {
        var result = _sampleData.GetAggregateSortedListOfStatesUsingCsvRows();
        Assert.AreEqual("OR, WA", result);
    }

    [TestMethod]
    public void People_ReturnsObjects_SortedByStateCityZip()
    {
        var people = _sampleData.People.ToList();
        Assert.AreEqual(3, people.Count);
        Assert.AreEqual("OR", people[0].Address.State);
        Assert.AreEqual("WA", people[1].Address.State);
        Assert.AreEqual("Jane", people[0].FirstName);
    }

    [TestMethod]
    public void FilterByEmailAddress_ReturnsMatches()
    {
        var result = _sampleData.FilterByEmailAddress(email => email.Contains("@test.com")).ToList();
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.Any(x => x.FirstName == "John"));
        Assert.IsTrue(result.Any(x => x.FirstName == "Jane"));
        Assert.IsFalse(result.Any(x => x.FirstName == "Bob"));
    }

    [TestMethod]
    public void GetAggregateListOfStatesGivenPeopleCollection_UsesAggregate()
    {
        var people = _sampleData.People;
        var result = _sampleData.GetAggregateListOfStatesGivenPeopleCollection(people);
        Assert.AreEqual("OR, WA", result);
    }

    [TestMethod]
    public void Node_IteratesCircleOnce()
    {
        var node1 = new Node<int>(1);
        var node2 = new Node<int>(2);
        var node3 = new Node<int>(3);

        node1.Next = node2;
        node2.Next = node3;
        node3.Next = node1; 

        var result = node1.ToList();

        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(1, result[0]);
        Assert.AreEqual(3, result[2]);
    }
}