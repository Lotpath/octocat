using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace Octocat.Handlers
{
    public class ListRepositoriesCommandHandler : ICommandHandler
    {
        private readonly IGitHubClient _client;
        private readonly IConsole _console;

        public ListRepositoriesCommandHandler(IGitHubClient client, IConsole console)
        {
            _client = client;
            _console = console;
        }

        public bool CanHandle(Command command)
        {
            return command.Verb == "list" && command.Noun == "repositories";
        }

        public async Task Handle(Command command)
        {
            var repositories = default(IReadOnlyList<Repository>);

            if (!string.IsNullOrEmpty(command.Organization))
            {
                repositories = await _client.Repository.GetAllForOrg(command.Organization);
            }
            else
            {
                repositories = await _client.Repository.GetAllForCurrent();
            }

            foreach (var repository in repositories)
            {
                _console.WriteLine(repository.FullName);
            }
        }

        public override string ToString()
        {
            return "list repositories [organization] (lists all repositories for the current user (or, optionally, the specified organization)";
        }
    }
}