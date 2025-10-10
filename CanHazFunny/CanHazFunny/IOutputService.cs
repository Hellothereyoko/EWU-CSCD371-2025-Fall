
using System;
using System.IO;
namespace CanHazFunny
{
    public interface IOutputService
    {
        void Write(string message);
    }



    public class ConsoleOutputService : IOutputService
    {
        public string? output;
    
        void IOutputService.Write(string message)
        {
            output = ("Response: " + message);

            Console.WriteLine(output);

        }
    }
}
