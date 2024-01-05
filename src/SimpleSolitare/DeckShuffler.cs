using SimpleSolitare.Models;

namespace SimpleSolitare
{
    public interface IDeckShuffler
    {
        void ShuffleExisting(IDeck deck);

        IDeck ShuffleNew(IDeck deck);
    }

    public class DeckShuffler : IDeckShuffler
    {
        private readonly IRngFactory _rng;

        public DeckShuffler(IRngFactory rng)
        {
            _rng = rng;
        }

        public void ShuffleExisting(IDeck deck)
        {
            var iterations = deck.Count * 23;
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

        public IDeck ShuffleNew(IDeck deck)
        {
            var shuffled = new Deck(deck);

            ShuffleExisting(shuffled);

            return shuffled;
        }
    }
}
