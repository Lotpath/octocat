using System.Threading.Tasks;

namespace Octocat.Handlers
{
    public class ClearConsoleCommandHandler : ICommandHandler
    {
        private readonly IConsole _console;

        public ClearConsoleCommandHandler(IConsole console)
        {
            _console = console;
        }

        public Task<bool> CanHandle(Command command)
        {
            return Task.Run(() => command.Verb == "clear");
        }

        public async Task Handle(Command command)
        {
            _console.Clear();
            await Task.Delay(0);
        }

        public override string ToString()
        {
            return "clear to clear the console";
        }
    }
}