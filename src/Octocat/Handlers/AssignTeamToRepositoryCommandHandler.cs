using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace Octocat.Handlers
{
    public class AssignTeamToRepositoryCommandHandler : ICommandHandler
    {
        private readonly IGitHubClient _client;

        public AssignTeamToRepositoryCommandHandler(IGitHubClient client)
        {
            _client = client;
        }

        public Task<bool> CanHandle(Command command)
        {
            return Task.Run(
                () =>
                command.Verb == "assign"
                && command.Noun == "team");
        }

        public async Task Handle(Command command)
        {
            var teams = await _client.Organization.Team.GetAll(command.Organization);
            var team = teams.SingleOrDefault(x => x.Name == command.Target);
            await _client.Organization.Team.AddRepository(team.Id, command.Organization, command.Repository);
        }

        public override string ToString()
        {
            return "assign team [organization]/[repository] [team-name] (assigns the specified team to the specified repository)";
        }
    }
}