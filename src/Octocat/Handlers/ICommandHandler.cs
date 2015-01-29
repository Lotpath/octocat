using System.Threading.Tasks;

namespace Octocat.Handlers
{
    public interface ICommandHandler
    {
        bool CanHandle(Command command);
        Task Handle(Command command);
    }
}