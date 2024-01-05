using SimpleSolitare.CommandLine;

namespace SimpleSolitare
{
    public interface IGameManager
    {
        IGameManagerResult? RunGames(ICommandLineArguments commandLineArguments, object? gameCompletedContext);
    }

    public class GameManager : IGameManager
    {
        private readonly object _callbackSync = new();
        private readonly IInputMonitor _inputMonitor;
        private readonly IOutputWriter _outputWriter;
        private readonly IPlayer _player;
        private readonly IGameRunner _gameRunner;
        private readonly IStatistician _statistician;

        private CancellationTokenSource? _cancellationTokenSource;
        private int _gameCount = 0;

        public GameManager(IInputMonitor inputMonitor, IOutputWriter outputWriter, IPlayer player, IGameRunner gameRunner, IStatistician statistician)
        {
            _inputMonitor = inputMonitor;
            _outputWriter = outputWriter;
            _player = player;
            _gameRunner = gameRunner;
            _statistician = statistician;
        }

        public IGameManagerResult? RunGames(ICommandLineArguments commandLineArguments, object? gameCompletedContext)
        {
            _outputWriter.WriteLine($"Starting {commandLineArguments.GameCount} games. Press 'X' to exit.\r\n");

            _cancellationTokenSource = new CancellationTokenSource();

            _inputMonitor.CancelRequested += OnCancelRequested;
            _inputMonitor.StartMonitoring(_cancellationTokenSource.Token);

            try
            {
                _gameCount = commandLineArguments.GameCount;

                var startTime = DateTime.Now;

                _gameRunner.StartGames(_player, commandLineArguments.GameCount, GameCallback, gameCompletedContext, _cancellationTokenSource.Token);

                _gameRunner.WaitForCompletion();

                var duration = DateTime.Now - startTime;
                var statistics = _statistician.GetStatistics();

                _outputWriter.WriteLine($"\r\n\r\nFinished {statistics.TotalGames} games in {duration.TotalMilliseconds}ms.");
                _outputWriter.WriteLine($"Statistics:");
                _outputWriter.WriteLine($"  Won: {statistics.WinCount} ({(statistics.WinPercent * 100.0):F1}%)");
                _outputWriter.WriteLine($"  Lost: {statistics.LossCount} ({(statistics.LossPercent * 100.0):F1}%)");
                _outputWriter.WriteLine($"  Mean number of cards until loss: {statistics.MeanLossCardCount}");
                _outputWriter.WriteLine($"  Median number of cards until loss: {statistics.MedianLossCardCount}");
                _outputWriter.WriteLine($"  Mode number of cards until loss: {statistics.ModeLossCardCount}");
                _outputWriter.WriteLine($"  Variance of cards until loss: {statistics.VarianceLossCardCount:F2}");
                _outputWriter.WriteLine($"  Standard deviation of cards until loss: {statistics.StandardDeviationLossCardCount:F2}");

                return new GameManagerResult(duration, _statistician.GetStatistics());
            }
            finally
            {
                _inputMonitor.CancelRequested -= OnCancelRequested;
                _inputMonitor.StopMonitoring();
            }
        }

        private void RequestCancel()
        {
            _cancellationTokenSource?.Cancel();
        }

        private void OnCancelRequested(object? sender, EventArgs e)
        {
            RequestCancel();
        }

        private void GameCallback(object? context, IGameResult result)
        {
            if (result.Outcome == GameOutcome.Win)
            {
                _statistician.AddWin();

                //_outputWriter.WriteLine($"Game {result.GameId} won.");

                if (context != null
                    && context is ICallbackContext callbackContext
                    && callbackContext.ResultWriter != null)
                {
                    lock (_callbackSync)
                    {
                        callbackContext.ResultWriter.Write(result);
                    }
                }
            }
            else if (result.Outcome == GameOutcome.Loss)
            {
                _statistician.AddLoss(result.CardCount);

                //_outputWriter.WriteLine($"Game {result.GameId} lost after {result.CardCount} cards.");
            }

            _outputWriter.Write($"\rGames played: {_statistician.TotalGames} ({((double)_statistician.TotalGames / (double)_gameCount) * 100.0:F1}%)");
        }
    }
}
