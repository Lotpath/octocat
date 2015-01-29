using System.Threading.Tasks;

namespace Octocat.Handlers
{
    public class ExitApplicationCommandHandler : ICommandHandler
    {
        public bool CanHandle(Command command)
        {
            return command.Verb == "q";
        }

        public Task Handle(Command command)
        {
            throw new ExitApplicationException();
        }

        public override string ToString()
        {
            return "'q' (exits the application)";
        }
    }
}