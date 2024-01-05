namespace SimpleSolitare
{
    public interface IGameRunner
    {
        void StartGames(IPlayer player, int gameCount, Action<object?, IGameResult> callback, object? callbackContext, CancellationToken cancellationToken);

        bool IsRunning { get; }

        IGameRunnerResult? WaitForResult();
    }

    public class GameRunner : IGameRunner
    {
        private readonly IDeckProvider _deckProvider;

        private Thread? _worker;
        private IGameRunnerResult? _result;

        public bool IsRunning => _worker?.IsAlive ?? false;

        public GameRunner(IDeckProvider deckProvider)
        {
            _deckProvider = deckProvider;
        }

        public IGameRunnerResult? WaitForResult()
        {
            _worker?.Join();

            return _result;
        }

        public void StartGames(IPlayer player, int gameCount, Action<object?, IGameResult> callback, object? callbackContext, CancellationToken cancellationToken)
        {
            var threadStart = new ThreadStart(() =>
            {
                _result = PlayGames(player, gameCount, callback, callbackContext, cancellationToken);
            });

            _worker = new Thread(threadStart);

            var results = new IGameResult[gameCount];

            _worker.Start();
        }

        private IGameRunnerResult PlayGames(IPlayer player, int gameCount, Action<object?, IGameResult> callback, object? callbackContext, CancellationToken cancellationToken)
        {
            var losses = 0;
            var wins = 0;

            var startTime = DateTime.Now;

            Parallel.For(0, gameCount, i =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var game = new Game(i + 1, player, _deckProvider.GetShuffledDeck());

                game.Play();

                if (game.Result != null)
                {
                    if (game.Result.Outcome == GameOutcome.Loss)
                    {
                        Interlocked.Increment(ref losses);
                    }
                    else if (game.Result.Outcome == GameOutcome.Win)
                    {
                        Interlocked.Increment(ref wins);
                    }

                    callback(callbackContext, game.Result);
                }
            });

            var duration = DateTime.Now - startTime;

            return new GameRunnerResult(losses, wins, duration);
        }
    }
}
