namespace SimpleSolitare
{
    public interface IGameRunnerResult
    {
        int Losses { get; }

        int Wins { get; }

        int TotalGames { get; }

        TimeSpan TotalDuration { get; }
    }

    public class GameRunnerResult : IGameRunnerResult
    {
        public int Losses { get; }

        public int Wins { get; }

        public int TotalGames => Losses + Wins;

        public TimeSpan TotalDuration { get; }

        public GameRunnerResult(int losses, int wins, TimeSpan totalDuration)
        {
            Losses = losses;
            Wins = wins;
            TotalDuration = totalDuration;
        }
    }
}
