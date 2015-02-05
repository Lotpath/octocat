using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Octocat.Handlers;
using Octokit;

namespace Octocat
{
    public class CommandProcessor
    {
        private readonly Interpreter _interpreter;
        private readonly IGitHubClient _client;
        private readonly IConsole _console;
        private readonly List<ICommandHandler> _handlers = new List<ICommandHandler>(); 
        private readonly List<Type> _handlerTypes = new List<Type>(); 

        public CommandProcessor(Interpreter interpreter, IGitHubClient client, IConsole console)
        {
            _interpreter = interpreter;
            _client = client;
            _console = console;

            RegisterHandlerTypes();
            ConfigureHandlers();
        }

        private void RegisterHandlerTypes()
        {
            var manuallyConfiguredTypes = new List<Type>
                {
                    typeof(DisplayHelpCommandHandler),
                    typeof(ExitApplicationCommandHandler),
                    typeof(ClearConsoleCommandHandler),
                    typeof(DefaultCommandHandler)
                };

            var handlerTypes = typeof(ICommandHandler)
                .Assembly
                .GetTypes()
                .Where(t => typeof(ICommandHandler).IsAssignableFrom(t)
                            && !t.IsInterface
                            && !t.IsAbstract
                            && !manuallyConfiguredTypes.Contains(t))
                .ToList();

            _handlerTypes.AddRange(handlerTypes);

            _handlerTypes.Insert(0, typeof(DisplayHelpCommandHandler));
            _handlerTypes.Insert(0, typeof(ExitApplicationCommandHandler));
            _handlerTypes.Add(typeof(ClearConsoleCommandHandler));
            _handlerTypes.Add(typeof(DefaultCommandHandler));
        }

        public async Task Process(string input)
        {
            var command = _interpreter.Interpret(input);
            await Execute(command);
        }

        private async Task Execute(Command command)
        {
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

        private void ConfigureHandlers()
        {
            _handlers.AddRange(_handlerTypes.Select(BuildHandler));
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

            // get the greediest constructor (though there should probably be only one)
            var max = constructors.Max(x => x.Parameters.Count);

            var ctor = constructors
                .SingleOrDefault(x => x.Parameters.Count == max);

            var parameters = new List<object>();

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
                if (p.ParameterType == typeof (IEnumerable<string>))
                {
                    parameters.Add(_handlers.Select(x => x.ToString()));
                }
            }

            var handler = (ICommandHandler)Activator.CreateInstance(handlerType, parameters.ToArray());
            return handler;
        }
    }
}