namespace SimpleSolitare
{
    public interface IStatistician
    {
        void AddWin();

        void AddLoss(int cardCount);

        IGameStatistics GetStatistics();

        int WinCount { get; }

        int LossCount { get; }

        int TotalGames { get; }
    }

    public class Statistician : IStatistician
    {
        private readonly object _lossLock = new();

        private int _winCount;
        private List<int> _lossCardCounts = new();
        private long _lossCardTotalCount;

        public int WinCount => _winCount;

        public int LossCount => _lossCardCounts.Count;

        public int TotalGames => WinCount + LossCount;

        public void AddWin()
        {
            Interlocked.Increment(ref _winCount);
        }

        public void AddLoss(int cardCount)
        {
            lock (_lossLock)
            {
                _lossCardCounts.Add(cardCount);
                _lossCardTotalCount += cardCount;
            }
        }

        public IGameStatistics GetStatistics()
        {
            return new GameStatistics(_winCount, _lossCardCounts, _lossCardTotalCount);
        }
    }
}
