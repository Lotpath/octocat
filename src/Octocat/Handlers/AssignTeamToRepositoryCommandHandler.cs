using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace Octocat.Handlers
{
    public class AssignTeamToRepositoryCommandHandler : ICommandHandler
    {
        private readonly IGitHubClient _client;
        private Team _team;

        public AssignTeamToRepositoryCommandHandler(IGitHubClient client)
        {
            _client = client;
        }

        public async Task<bool> CanHandle(Command command)
        {
            if (command.Verb != "assign")
            {
                return false;
            }

            var teams = await _client.Organization.Team.GetAll(command.Organization);
            _team = teams.SingleOrDefault(x => x.Name == command.Noun);
            
            return _team != null;
        }

        public async Task Handle(Command command)
        {
            await _client.Organization.Team.AddRepository(_team.Id, command.Organization, command.Repository);
        }

        public override string ToString()
        {
            return "assign [team] [organization]/[repository] (assigns the specified team to the specified repository)";
        }
    }
}