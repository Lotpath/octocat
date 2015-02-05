using System.Threading.Tasks;

namespace Octocat
{
    class Program
    {
        static void Main()
        {
            var repl = new Bootstrapper().Bootstrap();

            Task.WaitAll(repl.Start());
        }
    }
}
