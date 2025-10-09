
using System;
namespace CanHazFunny
{
    public interface IOutputService
    {
        void Write(string message);
    }


    class ConsoleOutputService : IOutputService
    {
        public void Write(string message)
        {
            
            Console.WriteLine(message);
        }
    }
}
