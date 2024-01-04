using System.Collections.Concurrent;

namespace SimpleSolitare
{
    public interface IGameRunner
    {
        void StartGames(IGame[] games, Action<IGameResult> callback, CancellationToken cancellationToken);

        bool IsRunning { get; }
    }

    public class GameRunner : IGameRunner
    {
        private volatile bool _isRunning;

        public bool IsRunning => _isRunning;

        public void StartGames(IGame[] games, Action<IGameResult> callback, CancellationToken cancellationToken)
        {
            var threadStart = new ParameterizedThreadStart(results =>
            {
                results = PlayGames(games, callback, cancellationToken);
            });

            var worker = new Thread(threadStart);

            var results = new IGameResult[games.Length];

            _isRunning = true;

            worker.Start(results);
        }

        private IGameResult[] PlayGames(IGame[] games, Action<IGameResult> callback, CancellationToken cancellationToken)
        {
            var results = new ConcurrentBag<IGameResult>();

            var parallelResult = Parallel.ForEach(games, game =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                game.Play();

                results.Add(game.Result!);

                callback(game.Result!);
            });

            var gameResults = results.ToArray();

            _isRunning = false;

            return gameResults;
        }
    }
}
