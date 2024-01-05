namespace SimpleSolitare
{
    public interface IGameManagerResult
    {
        TimeSpan TotalDuration { get; }

        IGameStatistics Statistics { get; }
    }

    public class GameManagerResult : IGameManagerResult
    {
        public TimeSpan TotalDuration { get; }

        public IGameStatistics Statistics { get; }

        public GameManagerResult(TimeSpan totalDuration, IGameStatistics statistics)
        {
            TotalDuration = totalDuration;
            Statistics = statistics;
        }
    }
}
