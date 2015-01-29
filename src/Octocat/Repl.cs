using System;
using System.Threading.Tasks;

namespace Octocat
{
    public class Repl
    {
        private readonly IConsole _console;
        private readonly Interpreter _interpreter;

        public Repl(IConsole console, Interpreter interpreter)
        {
            _console = console;
            _interpreter = interpreter;
        }

        public async Task Start()
        {
            _console.WriteLine("Octocat sez 'meow!'");
            _console.WriteLine("Enter '?' or 'help' for help.");

            do
            {
                var input = _console.ReadLine();

                try
                {
                    await _interpreter.Interpret(input);
                }
                catch (ExitApplicationException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _console.WriteLine(ex.ToString());
                }

            } while (true);
        }
    }
}