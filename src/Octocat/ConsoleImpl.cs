using System;

namespace Octocat
{
    public class ConsoleImpl : IConsole
    {
        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}