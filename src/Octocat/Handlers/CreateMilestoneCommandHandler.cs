using System.Threading.Tasks;
using Octokit;

namespace Octocat.Handlers
{
    public class CreateMilestoneCommandHandler : ICommandHandler
    {
        private readonly IGitHubClient _client;

        public CreateMilestoneCommandHandler(IGitHubClient client)
        {
            _client = client;
        }

        public Task<bool> CanHandle(Command command)
        {
            return Task.Run(
                () =>
                command.Verb == "create"
                && command.Noun == "milestone");
        }

        public async Task Handle(Command command)
        {
            await _client.Issue.Milestone.Create(
                command.Organization,
                command.Repository,
                new NewMilestone(command.Target));
        }
    }
}