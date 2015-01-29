using System;
using System.Threading.Tasks;
using Octokit;
using Xunit;

namespace Octocat.Tests
{
    public class GitHubClientFactoryTester
    {
        private GitHubClientFactory _factory;

        public GitHubClientFactoryTester()
        {
            _factory = new GitHubClientFactory();
        }

        [Fact]
        public async Task can_create_client_with_valid_credentials_and_fetch_organizations()
        {
            var client = _factory.CreateClient("lotpath-buildadmin", "~L0tp@th!");

            var orgs = await client.Organization.GetAllForCurrent();

            Assert.True(orgs.Count > 0);
        }

        [Fact]
        public async Task exception_creating_client_with_invalid_credentials()
        {
            var client = _factory.CreateClient("foo", "bar");
            var ex = default(Exception);

            try
            {
                await client.Organization.GetAllForCurrent();
            }
            catch (Exception e)
            {
                ex = e; 
            }

            Assert.IsType<AuthorizationException>(ex);
        }
    }
}