using System.Collections.Concurrent;

namespace SimpleSolitare
{
    public interface IGameRunner
    {
        void StartGames(IPlayer player, int gameCount, Action<IGameResult> callback, CancellationToken cancellationToken);

        bool IsRunning { get; }

        IGameRunnerResult? Result { get; }
    }

    public class GameRunner : IGameRunner
    {
        private readonly IDeckProvider _deckProvider;

        private volatile bool _isRunning;

        public bool IsRunning => _isRunning;

        public GameRunner(IDeckProvider deckProvider)
        {
            _deckProvider = deckProvider;
        }

        public IGameRunnerResult? Result { get; private set; }

        public void StartGames(IPlayer player, int gameCount, Action<IGameResult> callback, CancellationToken cancellationToken)
        {
            var threadStart = new ThreadStart(() =>
            {
                Result = PlayGames(player, gameCount, callback, cancellationToken);
                _isRunning = false;
            });

            var worker = new Thread(threadStart);

            var results = new IGameResult[gameCount];

            _isRunning = true;

            worker.Start();
        }

        private IGameRunnerResult PlayGames(IPlayer player, int gameCount, Action<IGameResult> callback, CancellationToken cancellationToken)
        {
            var losses = new ConcurrentBag<IGameResult>();
            var wins = new ConcurrentBag<IGameResult>();

            var startTime = DateTime.Now;

            var parallelResult = Parallel.For(0, gameCount, i =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var game = new Game(i + 1, player, _deckProvider.GetShuffledDeck());

                game.Play();

                if (game.Result == null)
                {

                }
                else if (game.Result.Outcome == GameOutcome.Loss)
                {
                    losses.Add(game.Result);
                }
                else if (game.Result.Outcome == GameOutcome.Win)
                {
                    wins.Add(game.Result);
                }

                callback(game.Result!);
            });

            var duration = DateTime.Now - startTime;

            return new GameRunnerResult(losses.ToArray(), wins.ToArray(), duration);
        }
    }
}
