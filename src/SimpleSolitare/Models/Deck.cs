using System.Collections;
using System.Text;

namespace SimpleSolitare.Models
{
    public interface IDeck : IList<ICard>
    {
    }

    public class Deck : IDeck
    {
        private readonly IList<ICard> _cards;

        public Deck()
            : this(new List<ICard>())
        {
        }

        public Deck(IList<ICard> cards)
        {
            _cards = new List<ICard>();

            for (var i = 0; i < cards.Count; i++)
            {
                _cards.Add(cards[i]);
            }
        }

        public ICard this[int index]
        {
            get => _cards[index];
            set => _cards[index] = value;
        }

        public int Count => _cards.Count;

        public bool IsReadOnly => _cards.IsReadOnly;

        public void Add(ICard card)
        {
            _cards.Add(card);
        }

        public void Clear()
        {
            _cards.Clear();
        }

        public bool Contains(ICard card)
        {
            return _cards.Contains(card);
        }

        public void CopyTo(ICard[] array, int arrayIndex)
        {
            _cards.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ICard> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        public int IndexOf(ICard card)
        {
            return _cards.IndexOf(card);
        }

        public void Insert(int index, ICard card)
        {
            _cards.Insert(index, card);
        }

        public bool Remove(ICard card)
        {
            return _cards.Remove(card);
        }

        public void RemoveAt(int index)
        {
            _cards.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        public override string ToString()
        {
            var sample = Count;
            var builder = new StringBuilder();
            builder.Append('(');
            for (var i = 0; i < sample; i++)
            {
                builder.Append(this[i]);
                if (i < sample - 1)
                {
                    builder.Append(", ");
                }
            }
            builder.Append(')');
            return builder.ToString();
        }
    }
}
