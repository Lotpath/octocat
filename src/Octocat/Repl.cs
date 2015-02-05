using System;
using System.Threading.Tasks;

namespace Octocat
{
    public class Repl
    {
        private readonly IConsole _console;
        private readonly CommandProcessor _processor;

        public Repl(IConsole console, CommandProcessor processor)
        {
            _console = console;
            _processor = processor;
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
                    await _processor.Process(input);
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

            _console.WriteLine("Octocat sez 'I can haz cheezeburger?!?'");
        }
    }
}