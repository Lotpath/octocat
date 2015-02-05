using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octocat.Handlers
{
    public class DisplayHelpCommandHandler : ICommandHandler
    {
        private readonly IConsole _console;
        private readonly IEnumerable<string> _commandDescriptions;

        public DisplayHelpCommandHandler(IConsole console, IEnumerable<string> commandDescriptions)
        {
            _console = console;
            _commandDescriptions = commandDescriptions;
        }

        public Task<bool> CanHandle(Command command)
        {
            return Task.Run(
                () =>
                command.Verb == "?"
                | command.Verb == "help");
        }

        public Task Handle(Command command)
        {
            _console.WriteLine("Commands:");

            foreach (var desc in _commandDescriptions)
            {
                _console.WriteLine(desc);
            }

            return Task.Delay(0);
        }

        public override string ToString()
        {
            return "? or help to display help";
        }
    }
}