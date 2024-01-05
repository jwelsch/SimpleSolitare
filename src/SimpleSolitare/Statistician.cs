namespace SimpleSolitare
{
    public interface IStatistician
    {
        void AddWin();

        void AddLoss(int cardCount);

        IGameStatistics GetStatistics();
    }

    public class Statistician : IStatistician
    {
        private readonly object _lossLock = new();

        private int _winCount;
        private int _lossCount;
        private long _lossCardCount;
        private int _leastLossCardCount = int.MaxValue;
        private int _greatestLossCardCount;

        public void AddWin()
        {
            Interlocked.Increment(ref _winCount);
        }

        public void AddLoss(int cardCount)
        {
            lock (_lossLock)
            {
                _lossCount++;
                _lossCardCount += cardCount;

                if (cardCount > _leastLossCardCount)
                {
                    _leastLossCardCount = cardCount;
                }

                if (cardCount > _greatestLossCardCount)
                {
                    _greatestLossCardCount = cardCount;
                }
            }
        }

        public IGameStatistics GetStatistics()
        {
            return new GameStatistics(_winCount, _lossCount, _lossCardCount, _leastLossCardCount, _greatestLossCardCount);
        }
    }
}
