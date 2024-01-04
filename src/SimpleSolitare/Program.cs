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

        private static void Main(string[] args)
        {
            var serviceProvider = RegisterAppServices();

            using var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactoryWrap>();
            var logger = loggerFactory.CreateLogger<Program>();

            var commandLineProcessor = serviceProvider.GetRequiredService<ICommandLineProcessor>();
            var deckProvider = serviceProvider.GetRequiredService<IDeckProvider>();
            var player = serviceProvider.GetRequiredService<IPlayer>();
            var runner = serviceProvider.GetRequiredService<IGameRunner>();
            var gameResultWriter = serviceProvider.GetRequiredService<IGameResultWriter>();

            var commandLineArguments = commandLineProcessor.Process(args) ?? throw new Exception($"Command line arguments object was null.");

            RunGames(logger, deckProvider, player, runner, gameResultWriter, commandLineArguments);
        }

        private static void RunGames(ILoggerWrap logger, IDeckProvider deckProvider, IPlayer player, IGameRunner gameRunner, IGameResultWriter gameResultWriter, ICommandLineArguments commandLineArguments)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var games = ConfigureGames(deckProvider, player, commandLineArguments.GameCount);

            Console.CancelKeyPress += (o, e) =>
            {
                cancellationTokenSource.Cancel();
            };

            logger.LogInformation($"Starting {commandLineArguments.GameCount} games. Press 'X' to exit.");

            gameRunner.StartGames(games, GameCallback, cancellationTokenSource.Token);

            while (!cancellationTokenSource.IsCancellationRequested
                && gameRunner.IsRunning)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.X)
                    {
                        cancellationTokenSource.Cancel();
                        break;
                    }
                }

                Thread.Sleep(100);
            }

            if (gameRunner.Result == null)
            {
                logger.LogError($"Game runner result was null.");
                return;
            }

            logger.LogInformation($"\r\nFinished {gameRunner.Result.TotalGames} games. Lost {gameRunner.Result.Losses.Length}. Won {gameRunner.Result.Wins.Length}.");

            WriteWins(gameResultWriter, gameRunner.Result.Wins, commandLineArguments);
        }

        private static IGame[] ConfigureGames(IDeckProvider deckProvider, IPlayer player, int gameCount)
        {
            var games = new List<Game>();

            for (var i = 0; i < gameCount; i++)
            {
                var deck = deckProvider.GetShuffledDeck();
                var game = new Game(i + 1, player, deck);

                games.Add(game);
            }

            return games.ToArray();
        }

        private static void GameCallback(IGameResult result)
        {
            if (result.Outcome == GameOutcome.Win)
            {

                Console.WriteLine($"Game {result.GameId} won.");
            }
            else if (result.Outcome == GameOutcome.Loss)
            {
                Console.WriteLine($"Game {result.GameId} lost after {result.CardCount} cards.");
            }
        }

        private static void WriteWins(IGameResultWriter gameResultWriter, IGameResult[] wins, ICommandLineArguments commandLineArguments)
        {
            if (commandLineArguments.WinOutputPath == null
                || wins.Length <= 0)
            {
                return;
            }

            gameResultWriter.Open(commandLineArguments.WinOutputPath);

            try
            {
                for (var i = 0; i < wins.Length; i++)
                {
                    gameResultWriter.Write(wins[i]);
                }
            }
            finally
            {
                gameResultWriter.Close();
            }
        }
    }
}
