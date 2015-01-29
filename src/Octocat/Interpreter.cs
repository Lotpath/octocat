using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octocat.Handlers;
using Octokit;

namespace Octocat
{
    public class Interpreter
    {
        private readonly IGitHubClient _client;
        private readonly IConsole _console;
        private readonly IList<ICommandHandler> _handlers = new List<ICommandHandler>();

        public Interpreter(IGitHubClient client, IConsole console)
        {
            _client = client;
            _console = console;
            ConfigureHandlers();
        }

        private void ConfigureHandlers()
        {
            _handlers.Add(new ListRepositoriesCommandHandler(_client, _console));
            _handlers.Add(new CreateRepositoryCommandHandler(_client));
            _handlers.Add(new DeleteLabelsCommandHandler(_client));
            _handlers.Add(new ConfigureLabelsCommandHandler(_client));

            _handlers.Add(new ExitApplicationCommandHandler());
            _handlers.Add(new DisplayHelpCommandHandler(_console, _handlers));
            _handlers.Add(new DefaultCommandHandler(_console));
        }

        private Command ParseCommand(string input)
        {
            var command = new Command();

            var inputParts = input.Split(' ');

            command.Verb = inputParts[0];

            if (inputParts.Length > 1)
            {
                command.Noun = inputParts[1];
                if (inputParts.Length > 2)
                {
                    var repositoryParts = inputParts[2].Split('/');
                    command.Organization = repositoryParts[0];
                    if (repositoryParts.Length > 1)
                    {
                        command.Repository = repositoryParts[1];
                    }
                }
            }

            return command;
        }

        public async Task Interpret(string input)
        {
            var command = ParseCommand(input);
            var handler = _handlers.First(x => x.CanHandle(command));
            await handler.Handle(command);
        }
    }
}