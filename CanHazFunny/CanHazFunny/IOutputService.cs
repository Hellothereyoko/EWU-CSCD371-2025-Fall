
using System;
namespace CanHazFunny
{
    public interface IOutputService
    {
        void Write(string message);
    }



    class ConsoleOutputService : IOutputService 
    {
    
        void IOutputService.Write(string message)
        {
            Console.WriteLine("Response: " + message);
        }
    }
}
