using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSolitare.DependencyInjection;

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

            var gameCount = 100;

            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

            var deckProvider = serviceProvider.GetRequiredService<IDeckProvider>();
            var player = serviceProvider.GetRequiredService<IPlayer>();

            var runner = serviceProvider.GetRequiredService<IGameRunner>();

            RunGames(logger, deckProvider, player, runner, gameCount);
        }

        private static void RunGames(ILogger logger, IDeckProvider deckProvider, IPlayer player, IGameRunner gameRunner, int gameCount)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var games = ConfigureGames(deckProvider, player, gameCount);

            Console.CancelKeyPress += (o, e) =>
            {
                cancellationTokenSource.Cancel();
            };

            logger.LogInformation($"Starting {gameCount} games.");
            logger.LogInformation($"Press 'X' to exit.");

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

            logger.LogInformation($"Finished {gameCount} games.");
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
    }
}
