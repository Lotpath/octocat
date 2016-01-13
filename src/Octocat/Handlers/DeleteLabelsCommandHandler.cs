using System.Threading.Tasks;
using Octokit;

namespace Octocat.Handlers
{
    public class DeleteLabelsCommandHandler : ICommandHandler
    {
        private readonly IGitHubClient _client;

        public DeleteLabelsCommandHandler(IGitHubClient client)
        {
            _client = client;
        }

        public Task<bool> CanHandle(Command command)
        {
            return Task.Run(
                () =>
                command.Verb == "delete"
                && command.Noun == "labels");
        }

        public async Task Handle(Command command)
        {
            var labels = await _client.Issue.Labels.GetAllForRepository(command.Organization, command.Repository);

            foreach (var label in labels)
            {
                await _client.Issue.Labels.Delete(command.Organization, command.Repository, label.Name);
            }
        }

        public override string ToString()
        {
            return "delete labels [organization]/[repository] (deletes all labels for the specified repository)";
        }
    }
}