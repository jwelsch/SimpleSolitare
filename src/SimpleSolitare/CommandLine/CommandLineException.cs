using CommandLine;

namespace SimpleSolitare.CommandLine
{
    [Serializable]
    public class CommandLineException : Exception
    {
        public IReadOnlyList<Error> Errors { get; }

        public CommandLineException(IEnumerable<Error> errors)
        {
            Errors = new List<Error>(errors).AsReadOnly();
        }

        public CommandLineException(IEnumerable<Error> errors, string message)
            : base(message)
        {
            Errors = new List<Error>(errors).AsReadOnly();
        }

        public CommandLineException(IEnumerable<Error> errors, string message, Exception inner)
            : base(message, inner)
        {
            Errors = new List<Error>(errors).AsReadOnly();
        }
    }
}
