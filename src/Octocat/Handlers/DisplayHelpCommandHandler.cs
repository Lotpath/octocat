using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octocat.Handlers
{
    public class DisplayHelpCommandHandler : ICommandHandler
    {
        private readonly IConsole _console;
        private readonly IList<string> _commandDescriptions; 

        public DisplayHelpCommandHandler(IConsole console, IEnumerable<ICommandHandler> commands)
        {
            _console = console;
            _commandDescriptions = commands.Select(x => x.ToString()).ToList();
        }

        public bool CanHandle(Command command)
        {
            return command.Verb == "?" || command.Verb == "help";
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
    }
}