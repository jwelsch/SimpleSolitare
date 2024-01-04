using Microsoft.Extensions.Logging;

namespace SimpleSolitare.Wraps
{
    public interface ILoggerWrap
    {
        void LogInformation(string? message, params object?[] args);

        void LogDebug(string? message, params object?[] args);

        void LogError(string? message, params object?[] args);
    }

    public interface ILoggerWrap<T> : ILoggerWrap
    {
    }

    public class LoggerWrap : ILoggerWrap
    {
        private readonly ILogger _logger;

        public LoggerWrap(ILogger logger)
        {
            _logger = logger;
        }

        public void LogInformation(string? message, params object?[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogDebug(string? message, params object?[] args)
        {
            _logger.LogDebug(message, args);
        }

        public void LogError(string? message, params object?[] args)
        {
            _logger.LogError(message, args);
        }
    }

    public class LoggerWrap<T> : ILoggerWrap<T>
    {
        private readonly ILogger _logger;

        public LoggerWrap(ILogger logger)
        {
            _logger = logger;
        }

        public void LogInformation(string? message, params object?[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogDebug(string? message, params object?[] args)
        {
            _logger.LogDebug(message, args);
        }

        public void LogError(string? message, params object?[] args)
        {
            _logger.LogError(message, args);
        }
    }
}