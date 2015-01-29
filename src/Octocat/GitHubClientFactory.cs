using Octokit;

namespace Octocat
{
    public class GitHubClientFactory
    {
        public IGitHubClient CreateClient(string userName, string password)
        {
            return new GitHubClient(new ProductHeaderValue("Octocat"))
                {
                    Credentials = GetCredentials(userName, password)
                };
        }

        private Credentials GetCredentials(string userName, string password)
        {
            return new Credentials(userName, password);
        }
    }
}