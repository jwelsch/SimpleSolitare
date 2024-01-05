namespace SimpleSolitare
{
    public interface IGameRunnerResult
    {
        IGameResult[] Losses { get; }

        IGameResult[] Wins { get; }

        int TotalGames { get; }

        TimeSpan TotalDuration { get; }
    }

    public class GameRunnerResult : IGameRunnerResult
    {
        public IGameResult[] Losses { get; }

        public IGameResult[] Wins { get; }

        public int TotalGames => Losses.Length + Wins.Length;

        public TimeSpan TotalDuration { get; }

        public GameRunnerResult(IGameResult[] losses, IGameResult[] wins, TimeSpan totalDuration)
        {
            Losses = losses;
            Wins = wins;
            TotalDuration = totalDuration;
        }
    }
}
