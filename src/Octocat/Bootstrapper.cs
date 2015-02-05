using System;
using System.Linq;
using Octokit;

namespace Octocat
{
    public class Bootstrapper
    {
        public Repl Bootstrap()
        {
            var args = Environment.GetCommandLineArgs().ToList();
            
            args.RemoveAt(0);
            
            var client = CreateClient(args.ToArray());

            var console = new ConsoleImpl();

            var interpreter = new Interpreter();

            var processor = new CommandProcessor(interpreter, client, console);

            return new Repl(console, processor);
        }

        private IGitHubClient CreateClient(string[] args)
        {
            return new GitHubClientBootstrapper().CreateClient(args);
        }
    }
}