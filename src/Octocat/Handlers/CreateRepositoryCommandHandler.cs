using System.Threading.Tasks;
using Octokit;

namespace Octocat.Handlers
{
    public class CreateRepositoryCommandHandler : ICommandHandler
    {
        private readonly IGitHubClient _client;

        public CreateRepositoryCommandHandler(IGitHubClient client)
        {
            _client = client;
        }

        public bool CanHandle(Command command)
        {
            return command.Verb == "create" && command.Noun == "repository";
        }

        public async Task Handle(Command command)
        {
            await _client.Repository.Create(command.Organization, new NewRepository
                {
                    Name = command.Repository,
                    Private = true
                });
        }

        public override string ToString()
        {
            return "create repository [organization]/[repository] (creates a new private repository)";
        }
    }
}