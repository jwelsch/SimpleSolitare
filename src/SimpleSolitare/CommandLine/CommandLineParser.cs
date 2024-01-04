using CommandLine;

namespace SimpleSolitare.CommandLine
{
    public interface ICommandLineProcessor
    {
        ICommandLineArguments? Process(string[] args);
    }

    public class CommandLineProcessor : ICommandLineProcessor
    {
        public ICommandLineArguments Process(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException($"No arguments were found.", nameof(args));
            }

            var result = Parser.Default.ParseArguments<CommandLineArguments>(args)
                                       .MapResult<CommandLineArguments, ICommandLineArguments>(
                                          (CommandLineArguments a) => a,
                                          errs => throw new CommandLineException(errs));
            return result;
        }
    }
}
