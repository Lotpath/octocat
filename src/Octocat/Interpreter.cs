using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Octocat.Handlers;
using Octokit;

namespace Octocat
{
    public class Interpreter
    {
        private readonly IGitHubClient _client;
        private readonly IConsole _console;
        private readonly List<ICommandHandler> _handlers = new List<ICommandHandler>();

        public Interpreter(IGitHubClient client, IConsole console)
        {
            _client = client;
            _console = console;
            ConfigureHandlers();
        }

        private void ConfigureHandlers()
        {
            var manuallyConfiguredTypes = new List<Type>
                {
                    typeof(DisplayHelpCommandHandler),
                    typeof(ExitApplicationCommandHandler),
                    typeof(DefaultCommandHandler)
                };

            var handlerTypes = typeof(ICommandHandler)
                .Assembly
                .GetTypes()
                .Where(t => typeof(ICommandHandler).IsAssignableFrom(t)
                            && !t.IsInterface
                            && !t.IsAbstract
                            && !manuallyConfiguredTypes.Contains(t));

            _handlers.AddRange(handlerTypes.Select(BuildHandler));

            _handlers.Insert(0, new DisplayHelpCommandHandler(_console, _handlers));
            _handlers.Insert(0, new ExitApplicationCommandHandler());
            _handlers.Add(new DefaultCommandHandler(_console));
        }

        private ICommandHandler BuildHandler(Type handlerType)
        {
            var constructors = handlerType
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .Select(x => new
                    {
                        Constructor = x,
                        Parameters = x.GetParameters().ToList()
                    })
                .ToList();

            // get the greediest constructor (though there will probably be only one)
            var max = constructors.Max(x => x.Parameters.Count);

            var ctor = constructors
                .SingleOrDefault(x => x.Parameters.Count == max);

            var parameters = new List<object>();

            // todo: dependency injection could be improved but this suffices for now
            // if more dependencies are eventually needed for command handlers
            // then this should probably be rewritten rather than expanded with 
            // more branching
            foreach (var p in ctor.Parameters)
            {
                if (p.ParameterType == typeof(IGitHubClient))
                {
                    parameters.Add(_client);
                }
                if (p.ParameterType == typeof(IConsole))
                {
                    parameters.Add(_console);
                }
            }

            var handler = (ICommandHandler)Activator.CreateInstance(handlerType, parameters.ToArray());
            return handler;
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
                if (inputParts.Length > 3)
                {
                    command.Target = inputParts[3];
                }
            }

            return command;
        }

        public async Task Interpret(string input)
        {
            var command = ParseCommand(input);
            ICommandHandler handler = null;
            foreach (var item in _handlers)
            {
                if (await item.CanHandle(command))
                {
                    handler = item;
                    break;
                }
            }
            await handler.Handle(command);
        }
    }
}