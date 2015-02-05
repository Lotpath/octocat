namespace Octocat
{
    public class Interpreter
    {
        public Command Interpret(string input)
        {
            var command = new Command();

            if (string.IsNullOrWhiteSpace(input))
            {
                return command;
            }

            var inputParts = input.Split(' ');

            command.Verb = inputParts[0];

            if (inputParts.Length > 1)
            {
                command.Noun = inputParts[1];
                if (inputParts.Length > 2)
                {
                    var repositoryParts = inputParts[2].Split('/');
                    command.Organization = repositoryParts[0];
                    if (repositoryParts.Length > 1)
                    {
                        command.Repository = repositoryParts[1];
                    }
                }
                if (inputParts.Length > 3)
                {
                    command.Target = inputParts[3];
                }
            }

            return command;
        }
    }
}