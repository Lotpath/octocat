using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace Octocat.Handlers
{
    public class ConfigureLabelsCommandHandler : ICommandHandler
    {
        private readonly IGitHubClient _client;

        public ConfigureLabelsCommandHandler(IGitHubClient client)
        {
            _client = client;
        }


        public Task<bool> CanHandle(Command command)
        {
            return Task.Run(
                () =>
                command.Verb == "configure"
                && command.Noun == "labels");
        }

        public async Task Handle(Command command)
        {
            var labelsToCreate = new List<NewLabel>();

            labelsToCreate.Add(new NewLabel("wontfix", "000000"));
            labelsToCreate.Add(new NewLabel("feature", "fbca04"));
            labelsToCreate.Add(new NewLabel("defect", "fc2929"));
            labelsToCreate.Add(new NewLabel("chore", "c0c0c0"));
            labelsToCreate.Add(new NewLabel("ready", "009800"));

            labelsToCreate.Add(new NewLabel("0 points", "bfe5bf"));
            labelsToCreate.Add(new NewLabel("1 point", "bfe5bf"));
            labelsToCreate.Add(new NewLabel("2 points", "bfe5bf"));
            labelsToCreate.Add(new NewLabel("3 points", "bfe5bf"));
            labelsToCreate.Add(new NewLabel("5 points", "bfe5bf"));
            labelsToCreate.Add(new NewLabel("8 points", "bfe5bf"));
            labelsToCreate.Add(new NewLabel("13 points", "bfe5bf"));

            foreach (var label in labelsToCreate)
            {
                await _client.Issue.Labels.Create(command.Organization, command.Repository, label);
            }
        }

        public override string ToString()
        {
            return "configure labels [organization]/[repository] (configures default labels for the specified repository)";
        }
    }
}