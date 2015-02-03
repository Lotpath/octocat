using System.Threading.Tasks;

namespace Octocat.Handlers
{
    public interface ICommandHandler
    {
        Task<bool> CanHandle(Command command);
        Task Handle(Command command);
    }
}