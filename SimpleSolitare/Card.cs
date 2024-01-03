namespace SimpleSolitare
{
    public enum Suit
    {
        Club,
        Diamond,
        Heart,
        Spade
    }

    public enum FaceValue
    {
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    public interface ICard
    {
        Suit Suit { get; }

        FaceValue Value { get; }
    }

    public class Card : ICard
    {
        public Suit Suit { get; }

        public FaceValue Value { get; }

        public Card(Suit suit, FaceValue value)
        {
            Suit = suit;
            Value = value;
        }

        public bool Equals(Card card)
        {
            return card.Suit == Suit && card.Value == Value;
        }

        public override string ToString()
        {
            return $"{Value} of {Suit}s";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null
                || obj is not Card card)
            {
                return false;
            }

            return Equals(card);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Suit.GetHashCode() + Value.GetHashCode();
        }

        public static Card operator >(Card left, Card right)
        {
             return left.Value > right.Value ? left : right;
        }

        public static Card operator <(Card left, Card right)
        {
            return left.Value < right.Value ? left : right;
        }

        public static Card operator >=(Card left, Card right)
        {
            return left.Value >= right.Value ? left : right;
        }

        public static Card operator <=(Card left, Card right)
        {
            return left.Value <= right.Value ? left : right;
        }

        public static Card operator ==(Card left, Card right)
        {
            return left.Equals(right) ? left : right;
        }

        public static Card operator !=(Card left, Card right)
        {
            return !left.Equals(right) ? left : right;
        }
    }
}
