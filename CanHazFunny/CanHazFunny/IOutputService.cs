using System;
using System.IO;

namespace CanHazFunny; // FIX 1: Converted to file-scoped namespace

public interface IOutputService
{
    void Write(string message);
}

public class ConsoleOutputService : IOutputService
{
    // FIX 2: Converted the public field 'output' to a private backing field.
    private string? _output; 

    // FIX 2: Added a public property to access the field (Resolves CA1051).
    public string? Output
    {
        get => _output;
        set => _output = value;
    }

    void IOutputService.Write(string message)
    {
        // FIX 3: Updated to use the corrected property name 'Output'
        Output = ("Response: " + message);

        Console.WriteLine(Output);
    }
}