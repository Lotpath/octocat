namespace Octocat
{
    public interface IConsole
    {
        void WriteLine(string output);
        string ReadLine();
        void Clear();
    }
}