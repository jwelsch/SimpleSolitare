using Microsoft.Extensions.Logging;

namespace SimpleSolitare.Wraps
{
    public interface ILoggerFactoryWrap : IDisposable
    {
        ILoggerWrap<T> CreateLogger<T>();
    }

    public class LoggerFactoryWrap : ILoggerFactoryWrap
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggerFactoryWrap(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public ILoggerWrap<T> CreateLogger<T>()
        {
            return new LoggerWrap<T>(_loggerFactory.CreateLogger<T>());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            _loggerFactory.Dispose();
        }
    }
}