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
                    return new GameResult(id, GameOutcome.Loss, deck, count);
                }

                count = (count + 1) % 10;
                count = count == 0 ? 10 : count;
            }

            return new GameResult(id, GameOutcome.Win, deck, count);
        }
    }
}
