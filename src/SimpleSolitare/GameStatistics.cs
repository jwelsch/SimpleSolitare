namespace SimpleSolitare
{
    public interface IGameStatistics
    {
        int WinCount { get; }

        int LossCount { get; }

        long LossCardCount { get; }

        int TotalGames { get; }

        double WinPercent { get; }

        double LossPercent { get; }

        double MeanLossCardCount { get; }
    }

    public class GameStatistics : IGameStatistics
    {
        public int WinCount { get; }

        public int LossCount { get; }

        public long LossCardCount { get; }

        public int LeastLossCardCount { get; }

        public int GreatestLossCardCount { get; }

        public int TotalGames { get; private set; }

        public double WinPercent { get; private set; }

        public double LossPercent { get; private set; }

        public double MeanLossCardCount { get; private set; }

        public GameStatistics(int winCount, int lossCount, long lossCardCount, int leastLossCardCount, int greatestLossCardCount)
        {
            WinCount = winCount;
            LossCount = lossCount;
            LossCardCount = lossCardCount;
            LeastLossCardCount = leastLossCardCount;
            GreatestLossCardCount = greatestLossCardCount;

            CalculateStatistics();
        }

        private void CalculateStatistics()
        {
            TotalGames = WinCount + LossCount;
            WinPercent = ((double)WinCount / (double)TotalGames);
            LossPercent = ((double)LossCount / (double)TotalGames);
            MeanLossCardCount = ((double)LossCardCount / (double)LossCount);
        }
    }
}
