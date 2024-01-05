using SimpleSolitare.CommandLine;

namespace SimpleSolitare
{
    public interface ICallbackContext
    {
        ICommandLineArguments Arguments { get; }

        IGameResultWriter? ResultWriter { get; }
    }

    public class CallbackContext : ICallbackContext
    {
        public ICommandLineArguments Arguments { get; }

        public IGameResultWriter? ResultWriter { get; }

        public CallbackContext(ICommandLineArguments arguments, IGameResultWriter? resultWriter)
        {
            Arguments = arguments;
            ResultWriter = resultWriter;
        }
    }
}
