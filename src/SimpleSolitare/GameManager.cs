using SimpleSolitare.CommandLine;

namespace SimpleSolitare
{
    public interface IGameManager
    {
        IGameRunnerResult? RunGames(ICommandLineArguments commandLineArguments, object? gameCompletedContext);
    }

    public class GameManager : IGameManager
    {
        private readonly object _callbackSync = new();
        private readonly IInputMonitor _inputMonitor;
        private readonly IOutputWriter _outputWriter;
        private readonly IPlayer _player;
        private readonly IGameRunner _gameRunner;

        private CancellationTokenSource? _cancellationTokenSource;

        public GameManager(IInputMonitor inputMonitor, IOutputWriter outputWriter, IPlayer player, IGameRunner gameRunner)
        {
            _inputMonitor = inputMonitor;
            _outputWriter = outputWriter;
            _player = player;
            _gameRunner = gameRunner;
        }

        public IGameRunnerResult? RunGames(ICommandLineArguments commandLineArguments, object? gameCompletedContext)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _inputMonitor.CancelRequested += OnCancelRequested;
            _inputMonitor.StartMonitoring(_cancellationTokenSource.Token);

            try
            {
                _gameRunner.StartGames(_player, commandLineArguments.GameCount, GameCallback, gameCompletedContext, _cancellationTokenSource.Token);

                return _gameRunner.WaitForResult();
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
                _outputWriter.WriteLine($"Game {result.GameId} won.");

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
                _outputWriter.WriteLine($"Game {result.GameId} lost after {result.CardCount} cards.");
            }
        }
    }
}
