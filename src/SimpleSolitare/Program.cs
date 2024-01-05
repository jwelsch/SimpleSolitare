using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSolitare.CommandLine;
using SimpleSolitare.DependencyInjection;
using SimpleSolitare.Wraps;

namespace SimpleSolitare
{
    internal class Program
    {
        #region Dependency injection

        private static IServiceProvider RegisterAppServices()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);
            RegisterModules(services);

            return services.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure =>
            {
                configure.AddConsole(o => o.FormatterName = nameof(CustomConsoleFormatter))
                         .AddConsoleFormatter<CustomConsoleFormatter, CustomConsoleOptions>();
                //configure.AddDebug();
            });
        }

        private static void RegisterModules(IServiceCollection services)
        {
            services.RegisterModule<ApplicationModule>();
        }

        #endregion

        private static readonly object _callbackSync = new();

        private static void Main(string[] args)
        {
            IOutputWriter? outputWriter = null;

            try
            {
                var serviceProvider = RegisterAppServices();

                outputWriter = serviceProvider.GetRequiredService<IOutputWriter>();
                var commandLineProcessor = serviceProvider.GetRequiredService<ICommandLineProcessor>();
                var streamWriterWrapFactory = serviceProvider.GetRequiredService<IStreamWriterWrapFactory>();
                var gameManager = serviceProvider.GetRequiredService<IGameManager>();

                var commandLineArguments = commandLineProcessor.Process(args) ?? throw new Exception($"Command line arguments object was null.");

                using var resultWriter = CreateResultWriter(streamWriterWrapFactory, commandLineArguments.WinOutputPath);

                var callbackContext = new CallbackContext(commandLineArguments, resultWriter);

                RunGames(outputWriter, gameManager, commandLineArguments, callbackContext);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
                outputWriter?.WriteLine($"Caught exception:");
                outputWriter?.WriteLine(ex);
            }
        }

        private static IGameResultWriter? CreateResultWriter(IStreamWriterWrapFactory streamWriterWrapFactory, string? winOutputPath)
        {
            if (winOutputPath == null)
            {
                return null;
            }

            var writer = new GameResultWriter(streamWriterWrapFactory);

            writer.Open(winOutputPath);

            return writer;
        }

        private static void RunGames(IOutputWriter outputWriter, IGameManager gameManager, ICommandLineArguments commandLineArguments, ICallbackContext callbackContext)
        {
            outputWriter.WriteLine($"Starting {commandLineArguments.GameCount} games. Press 'X' to exit.");

            var result = gameManager.RunGames(commandLineArguments, callbackContext);

            if (result == null)
            {
                outputWriter.WriteLine($"\r\nGame runner result was null.");
                return;
            }

            outputWriter.WriteLine($"\r\nFinished {result.TotalGames} games in {result.TotalDuration.TotalMilliseconds}ms. Lost {result.Losses}. Won {result.Wins}.");
        }
    }
}
