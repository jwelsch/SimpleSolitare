using System.Text;

namespace SimpleSolitare
{
    public interface IGame
    {
        int Id { get; }

        IPlayer Player { get; }

        IDeck Deck { get; }

        IGameResult? Result { get; }

        void Play();
    }

    public class Game : IGame
    {
        public int Id { get; }

        public IPlayer Player { get; }

        public IDeck Deck { get; }

        public IGameResult? Result { get; private set; }

        public Game(int id, IPlayer player, IDeck deck)
        {
            Id = id;
            Player = player;
            Deck = deck;
        }

        public void Play()
        {
            Result = Player.Play(Id, Deck);
        }

        public override string ToString()
        {
            return $"Game {Id} {Deck}";
        }
    }
}
