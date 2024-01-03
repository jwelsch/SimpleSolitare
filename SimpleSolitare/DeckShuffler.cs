namespace SimpleSolitare
{
    public interface IDeckShuffler
    {
        void Shuffle(IDeck deck);
    }

    public class DeckShuffler : IDeckShuffler
    {
        private readonly IRngFactory _rng;

        public DeckShuffler(IRngFactory rng)
        {
            _rng = rng;
        }

        public void Shuffle(IDeck deck)
        {
            var iterations = deck.Count * 21;
            var max = deck.Count - 1;

            for (var i = 0; i < iterations; i++)
            {
                var slot1 = _rng.GetInt(0, max);
                var slot2 = _rng.GetInt(0, max);

                var card = deck[slot1];
                deck[slot1] = deck[slot2];
                deck[slot2] = card;
            }
        }
    }
}
