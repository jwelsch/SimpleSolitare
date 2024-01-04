namespace SimpleSolitare
{
    public interface IDeckProvider
    {
        IDeck GetReferenceDeck();

        IDeck GetShuffledDeck();
    }

    public class DeckProvider : IDeckProvider
    {
        private readonly IDeckShuffler _deckShuffler;

        public static readonly Deck Reference = new(new List<ICard>
        {
            new Card(Suit.Club, FaceValue.Ace),
            new Card(Suit.Club, FaceValue.Two),
            new Card(Suit.Club, FaceValue.Three),
            new Card(Suit.Club, FaceValue.Four),
            new Card(Suit.Club, FaceValue.Five),
            new Card(Suit.Club, FaceValue.Six),
            new Card(Suit.Club, FaceValue.Seven),
            new Card(Suit.Club, FaceValue.Eight),
            new Card(Suit.Club, FaceValue.Nine),
            new Card(Suit.Club, FaceValue.Ten),
            new Card(Suit.Club, FaceValue.Jack),
            new Card(Suit.Club, FaceValue.Queen),
            new Card(Suit.Club, FaceValue.King),
            new Card(Suit.Diamond, FaceValue.Ace),
            new Card(Suit.Diamond, FaceValue.Two),
            new Card(Suit.Diamond, FaceValue.Three),
            new Card(Suit.Diamond, FaceValue.Four),
            new Card(Suit.Diamond, FaceValue.Five),
            new Card(Suit.Diamond, FaceValue.Six),
            new Card(Suit.Diamond, FaceValue.Seven),
            new Card(Suit.Diamond, FaceValue.Eight),
            new Card(Suit.Diamond, FaceValue.Nine),
            new Card(Suit.Diamond, FaceValue.Ten),
            new Card(Suit.Diamond, FaceValue.Jack),
            new Card(Suit.Diamond, FaceValue.Queen),
            new Card(Suit.Diamond, FaceValue.King),
            new Card(Suit.Heart, FaceValue.Ace),
            new Card(Suit.Heart, FaceValue.Two),
            new Card(Suit.Heart, FaceValue.Three),
            new Card(Suit.Heart, FaceValue.Four),
            new Card(Suit.Heart, FaceValue.Five),
            new Card(Suit.Heart, FaceValue.Six),
            new Card(Suit.Heart, FaceValue.Seven),
            new Card(Suit.Heart, FaceValue.Eight),
            new Card(Suit.Heart, FaceValue.Nine),
            new Card(Suit.Heart, FaceValue.Ten),
            new Card(Suit.Heart, FaceValue.Jack),
            new Card(Suit.Heart, FaceValue.Queen),
            new Card(Suit.Heart, FaceValue.King),
            new Card(Suit.Spade, FaceValue.Ace),
            new Card(Suit.Spade, FaceValue.Two),
            new Card(Suit.Spade, FaceValue.Three),
            new Card(Suit.Spade, FaceValue.Four),
            new Card(Suit.Spade, FaceValue.Five),
            new Card(Suit.Spade, FaceValue.Six),
            new Card(Suit.Spade, FaceValue.Seven),
            new Card(Suit.Spade, FaceValue.Eight),
            new Card(Suit.Spade, FaceValue.Nine),
            new Card(Suit.Spade, FaceValue.Ten),
            new Card(Suit.Spade, FaceValue.Jack),
            new Card(Suit.Spade, FaceValue.Queen),
            new Card(Suit.Spade, FaceValue.King)
        });

        public DeckProvider(IDeckShuffler deckShuffler)
        {
            _deckShuffler = deckShuffler;
        }

        public IDeck GetReferenceDeck()
        {
            return Reference;
        }

        public IDeck GetShuffledDeck()
        {
            return _deckShuffler.ShuffleNew(Reference);
        }
    }
}
