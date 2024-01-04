namespace SimpleSolitare
{
    public enum GameOutcome
    {
        Unbegun,
        Loss,
        Win
    }

    public interface IGameResult
    {
        int GameId { get; }

        GameOutcome Outcome { get; }

        IDeck Deck { get; }

        int CardCount { get; }
    }

    public class GameResult : IGameResult
    {
        public int GameId { get; }

        public GameOutcome Outcome { get; }

        public IDeck Deck { get; }

        public int CardCount { get; }

        public GameResult(int gameId, GameOutcome outcome, IDeck deck, int cardCount)
        {
            GameId = gameId;
            Outcome = outcome;
            Deck = deck;
            CardCount = cardCount;
        }
    }
}
