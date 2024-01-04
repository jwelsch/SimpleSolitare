namespace SimpleSolitare
{
    public interface IGameRunnerResult
    {
        IGameResult[] Losses { get; }

        IGameResult[] Wins { get; }

        int TotalGames { get; }
    }

    public class GameRunnerResult : IGameRunnerResult
    {
        public IGameResult[] Losses { get; }

        public IGameResult[] Wins { get; }

        public int TotalGames => Losses.Length + Wins.Length;

        public GameRunnerResult(IGameResult[] losses, IGameResult[] wins)
        {
            Losses = losses;
            Wins = wins;
        }
    }
}
