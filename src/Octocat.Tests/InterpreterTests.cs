using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FakeItEasy;
using Octokit;
using Xunit;

namespace Octocat.Tests
{
    public class InterpreterTests
    {
        private readonly IGitHubClient _client;
        private readonly IConsole _console;
        private readonly Interpreter _interpreter;

        public InterpreterTests()
        {
            _client = A.Fake<IGitHubClient>();
            _console = A.Fake<IConsole>();
            _interpreter = new Interpreter(_client, _console);
        }

        [Fact]
        public async Task can_create_a_new_repository()
        {
            await _interpreter.Interpret("create repository org/repo");

            A.CallTo(() =>
                     _client
                         .Repository
                         .Create("org", A<NewRepository>.That.Matches(x =>
                             x.Name == "repo" && x.Private == true)))
             .MustHaveHappened();
        }

        [Fact]
        public async Task can_list_repositories_for_the_current_user()
        {
            await _interpreter.Interpret("list repositories");

            A.CallTo(() => _client.Repository.GetAllForCurrent()).MustHaveHappened();
        }

        [Fact]
        public async Task can_assign_a_team_to_a_repository()
        {
            A.CallTo(() => _client.Organization.Team.GetAll("org"))
             .Returns(new ReadOnlyCollection<Team>(new List<Team>
                 {
                     new Team {Id = 99, Name = "devs"}
                 }));

            await _interpreter.Interpret("assign team org/repo devs");

            A.CallTo(() => _client.Organization.Team.AddRepository(99, "org", "repo"))
                .MustHaveHappened();
        }

        [Fact]
        public async Task can_create_a_new_milestone()
        {
            await _interpreter.Interpret("create milestone org/repo 0.1.0");

            A.CallTo(() =>
                     _client.Issue.Milestone.Create("org", "repo", A<NewMilestone>.That.Matches(x => x.Title == "0.1.0")))
             .MustHaveHappened();
        }

        [Fact]
        public async Task can_list_repositories_for_an_organization()
        {
            await _interpreter.Interpret("list repositories org");

            A.CallTo(() => _client.Repository.GetAllForOrg("org")).MustHaveHappened();
        }

        [Fact]
        public async Task can_delete_labels_for_a_repository()
        {
            A.CallTo(() =>
                     _client.Issue.Labels.GetForRepository("org", "repo"))
             .Returns(new ReadOnlyCollection<Label>(new List<Label>
                 {
                     new Label {Name = "foo", Color = "000000"},
                     new Label {Name = "bar", Color = "000000"},
                 }));

            await _interpreter.Interpret("delete labels org/repo");

            A.CallTo(() =>
                     _client
                         .Issue.Labels.Delete("org", "repo", "foo"))
             .MustHaveHappened();

            A.CallTo(() =>
                     _client
                         .Issue.Labels.Delete("org", "repo", "bar"))
             .MustHaveHappened();
        }

        [Fact]
        public async Task can_configure_labels_for_a_repository()
        {
            await _interpreter.Interpret("configure labels org/repo");

            A.CallTo(() =>
                     _client
                         .Issue.Labels.Create("org", "repo", A<NewLabel>._))
             .MustHaveHappened();
        }

        [Fact]
        public async Task can_display_help()
        {
            await _interpreter.Interpret("?");

            A.CallTo(() => _console.WriteLine(A<string>._))
                .MustHaveHappened(Repeated.AtLeast.Times(6));
        }

        [Fact]
        public async Task can_exit_application()
        {
            var exception = default(Exception);

            try
            {
                await _interpreter.Interpret("q");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsType<ExitApplicationException>(exception);
        }
    }
}