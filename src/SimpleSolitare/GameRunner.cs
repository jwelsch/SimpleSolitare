using SimpleSolitare.Models;

namespace SimpleSolitare
{
    public interface IGameRunner
    {
        void StartGames(IPlayer player, int gameCount, Action<object?, IGameResult> callback, object? callbackContext, CancellationToken cancellationToken);

        bool IsRunning { get; }

        void WaitForCompletion();
    }

    public class GameRunner : IGameRunner
    {
        private readonly IDeckProvider _deckProvider;

        private Thread? _worker;

        public bool IsRunning => _worker?.IsAlive ?? false;

        public GameRunner(IDeckProvider deckProvider)
        {
            _deckProvider = deckProvider;
        }

        public void WaitForCompletion()
        {
            _worker?.Join();
        }

        public void StartGames(IPlayer player, int gameCount, Action<object?, IGameResult> callback, object? callbackContext, CancellationToken cancellationToken)
        {
            var threadStart = new ThreadStart(() =>
            {
                PlayGames(player, gameCount, callback, callbackContext, cancellationToken);
            });

            _worker = new Thread(threadStart);

            var results = new IGameResult[gameCount];

            _worker.Start();
        }

        private void PlayGames(IPlayer player, int gameCount, Action<object?, IGameResult> callback, object? callbackContext, CancellationToken cancellationToken)
        {
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
                    callback(callbackContext, game.Result);
                }
            });
        }
    }
}
