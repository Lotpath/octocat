using System.Threading.Tasks;
using Octokit;

namespace Octocat
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = CreateClient(args);
            var console = new ConsoleImpl();

            var interpreter = new Interpreter(client, console);

            var repl = new Repl(console, interpreter);

            Task.WaitAll(repl.Start());
        }

        private static IGitHubClient CreateClient(string[] args)
        {
            return new GitHubClientBootstrapper().CreateClient(args);
        }
    }
}
