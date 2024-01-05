using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSolitare.CommandLine;
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
            try
            {
                var serviceProvider = RegisterAppServices();

                var commandLineProcessor = serviceProvider.GetRequiredService<ICommandLineProcessor>();
                var deckProvider = serviceProvider.GetRequiredService<IDeckProvider>();
                var player = serviceProvider.GetRequiredService<IPlayer>();
                var runner = serviceProvider.GetRequiredService<IGameRunner>();
                var gameResultWriter = serviceProvider.GetRequiredService<IGameResultWriter>();

                var commandLineArguments = commandLineProcessor.Process(args) ?? throw new Exception($"Command line arguments object was null.");

                RunGames(deckProvider, player, runner, gameResultWriter, commandLineArguments);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
                Console.WriteLine($"Caught exception:");
                Console.WriteLine(ex);
            }
        }

        private static void RunGames(IDeckProvider deckProvider, IPlayer player, IGameRunner gameRunner, IGameResultWriter gameResultWriter, ICommandLineArguments commandLineArguments)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            Console.CancelKeyPress += (o, e) =>
            {
                cancellationTokenSource.Cancel();
            };

            Console.WriteLine($"Starting {commandLineArguments.GameCount} games. Press 'X' to exit.");

            gameRunner.StartGames(player, commandLineArguments.GameCount, GameCallback, cancellationTokenSource.Token);

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
                Console.WriteLine($"Game runner result was null.");
                return;
            }

            Console.WriteLine($"\r\nFinished {gameRunner.Result.TotalGames} games. Lost {gameRunner.Result.Losses.Length}. Won {gameRunner.Result.Wins.Length}.");

            WriteWins(gameResultWriter, gameRunner.Result.Wins, commandLineArguments);
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
            if (commandLineArguments.WinOutputPath == null)
            {
                return;
            }

            if (wins.Length < 1)
            {
                Console.WriteLine("\r\nNo wins to write.");
                return;
            }

            gameResultWriter.Open(commandLineArguments.WinOutputPath);

            Console.WriteLine($"\r\nWriting wins to '{commandLineArguments.WinOutputPath}'...");

            try
            {
                for (var i = 0; i < wins.Length; i++)
                {
                    gameResultWriter.Write(wins[i]);
                }

                Console.WriteLine($"Done writing wins.");
            }
            finally
            {
                gameResultWriter.Close();
            }
        }
    }
}
