﻿using System.Threading.Tasks;

namespace Octocat.Handlers
{
    public class DefaultCommandHandler : ICommandHandler
    {
        private readonly IConsole _console;

        public DefaultCommandHandler(IConsole console)
        {
            _console = console;
        }

        public Task<bool> CanHandle(Command command)
        {
            return Task.Run(() => true);
        }

        public Task Handle(Command command)
        {
            _console.WriteLine("Unknown command");
            return Task.Delay(0);
        }

        public override string ToString()
        {
            return "";
        }
    }
}