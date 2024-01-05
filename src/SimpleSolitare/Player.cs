namespace SimpleSolitare
{
    public interface IPlayer
    {
        IGameResult Play(int id, IDeck deck);
    }

    public class Player : IPlayer
    {
        public IGameResult Play(int id, IDeck deck)
        {
            var count = 1;

            for (var i = 0; i < deck.Count; i++)
            {
                if (count == (int)deck[i].Value)
                {
                    return new GameResult(id, GameOutcome.Loss, deck, i + 1);
                }

                count = (count + 1) % 10;

                if (count == 0)
                {
                    count = 10;
                }
            }

            return new GameResult(id, GameOutcome.Win, deck, count);
        }
    }
}
