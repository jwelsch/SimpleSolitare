namespace SimpleSolitare
{
    public interface IGameStatistics
    {
        int WinCount { get; }

        int LossCount { get; }

        int TotalGames { get; }

        double WinPercent { get; }

        double LossPercent { get; }

        int MeanLossCardCount { get; }

        int MedianLossCardCount { get; }

        int ModeLossCardCount { get; }

        double VarianceLossCardCount { get; }

        double StandardDeviationLossCardCount { get; }
    }

    public class GameStatistics : IGameStatistics
    {
        private readonly IList<int> _lossCardCounts;
        private readonly long _lossTotalCardCount;

        public int WinCount { get; }

        public int LossCount => _lossCardCounts.Count;

        public int LeastLossCardCount { get; private set; }

        public int GreatestLossCardCount { get; private set; }

        public int TotalGames { get; private set; }

        public double WinPercent { get; private set; }

        public double LossPercent { get; private set; }

        public int MeanLossCardCount { get; private set; }

        public int MedianLossCardCount { get; private set; }

        public int ModeLossCardCount { get; private set; }

        public double VarianceLossCardCount { get; private set; }

        public double StandardDeviationLossCardCount { get; private set; }

        public GameStatistics(int winCount, IList<int> lossCardCounts, long lossCardTotalCount)
        {
            WinCount = winCount;
            _lossCardCounts = lossCardCounts;
            _lossTotalCardCount = lossCardTotalCount;

            CalculateStatistics();
        }

        private void CalculateStatistics()
        {
            var half = _lossCardCounts.Count / 2;
            MedianLossCardCount = _lossCardCounts.Count % 2 == 0
                                    ? (_lossCardCounts[half] + _lossCardCounts[half + 1]) / 2
                                    : _lossCardCounts[half + 1];

            TotalGames = WinCount + LossCount;
            WinPercent = (double)WinCount / (double)TotalGames;
            LossPercent = (double)LossCount / (double)TotalGames;
            var meanLossCardCount = (double)_lossTotalCardCount / (double)LossCount;

            var orderedLossCardCount = _lossCardCounts.OrderBy(i => i).ToArray();
            LeastLossCardCount = orderedLossCardCount[0];
            GreatestLossCardCount = orderedLossCardCount[^1];

            var modeBuckets = new Dictionary<int, int>();
            double varianceSum = 0.0;

            for (var i = 0; i < _lossCardCounts.Count; i++)
            {
                var cardCount = _lossCardCounts[i];

                varianceSum += Math.Pow(cardCount - meanLossCardCount, 2.0);

                if (modeBuckets.TryGetValue(cardCount, out var value))
                {
                    modeBuckets[i] = value + 1;
                }
                else
                {
                    modeBuckets.Add(cardCount, 1);
                }
            }

            VarianceLossCardCount = varianceSum / _lossCardCounts.Count;
            StandardDeviationLossCardCount = Math.Sqrt(VarianceLossCardCount);

            ModeLossCardCount = modeBuckets.Max(i => i.Value);
            MeanLossCardCount = (int)meanLossCardCount;
        }
    }
}
