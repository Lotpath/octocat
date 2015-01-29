using System;
using Octokit;

namespace Octocat
{
    public class GitHubClientBootstrapper
    {
        public IGitHubClient CreateClient(string[] args)
        {
            var credentials = GetCredentials(args);

            var client = new GitHubClientFactory().CreateClient(credentials.UserName, credentials.Password);

            return client;
        }

        private Credentials GetCredentials(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                return new Credentials
                    {
                        UserName = Environment.GetEnvironmentVariable("OCTOCAT_USERNAME"),
                        Password = Environment.GetEnvironmentVariable("OCTOCAT_PASSWORD"),
                    };
            }

            return new Credentials
                {
                    UserName = args[0],
                    Password = args[1]
                };
        }

        private class Credentials
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}