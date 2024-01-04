using CommandLine;

namespace SimpleSolitare.CommandLine
{
    public interface ICommandLineArguments
    {
        int GameCount { get; }

        string? WinOutputPath { get; }
    }

    public class CommandLineArguments : ICommandLineArguments
    {
        [Option("count", Required = true, HelpText = "The number of games to play.")]
        public int GameCount { get; set; }

        [Option("win-output-path", Required = false, HelpText = "The path of the file to write the win data to.")]
        public string? WinOutputPath { get; set; }
    }
}
