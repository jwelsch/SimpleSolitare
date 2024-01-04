using System.Collections.Concurrent;

namespace SimpleSolitare
{
    public interface IGameRunner
    {
        void StartGames(IGame[] games, Action<IGameResult> callback, CancellationToken cancellationToken);

        bool IsRunning { get; }

        IGameRunnerResult? Result { get; }
    }

    public class GameRunner : IGameRunner
    {
        private volatile bool _isRunning;

        public bool IsRunning => _isRunning;

        public IGameRunnerResult? Result { get; private set; }

        public void StartGames(IGame[] games, Action<IGameResult> callback, CancellationToken cancellationToken)
        {
            var threadStart = new ThreadStart(() =>
            {
                Result = PlayGames(games, callback, cancellationToken);
                _isRunning = false;
            });

            var worker = new Thread(threadStart);

            var results = new IGameResult[games.Length];

            _isRunning = true;

            worker.Start();
        }

        private IGameRunnerResult PlayGames(IGame[] games, Action<IGameResult> callback, CancellationToken cancellationToken)
        {
            var losses = new ConcurrentBag<IGameResult>();
            var wins = new ConcurrentBag<IGameResult>();

            var parallelResult = Parallel.ForEach(games, game =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

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

            return new GameRunnerResult(losses.ToArray(), wins.ToArray());
        }
    }
}
