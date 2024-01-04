using SimpleSolitare.Wraps;

namespace SimpleSolitare
{
    public interface IGameResultWriter : IDisposable
    {
        void Open(string filePath);

        void Close();

        void Write(IGameResult result);
    }

    public class GameResultWriter : IGameResultWriter
    {
        private readonly IStreamWriterWrapFactory _streamWriterWrapFactory;

        private IStreamWriterWrap? _streamWriter;

        public GameResultWriter(IStreamWriterWrapFactory streamWriterWrapFactory)
        {
            _streamWriterWrapFactory = streamWriterWrapFactory;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            Close();
        }

        public void Open(string filePath)
        {
            if (_streamWriter != null)
            {
                throw new InvalidOperationException("The writer is already open.");
            }

            _streamWriter = _streamWriterWrapFactory.Create(filePath);
        }

        public void Close()
        {
            if (_streamWriter == null)
            {
                throw new InvalidOperationException("The writer is already closed.");
            }

            _streamWriter.Dispose();
            _streamWriter = null;
        }

        public void Write(IGameResult result)
        {
            if (_streamWriter == null)
            {
                throw new InvalidOperationException("The writer is not open.");
            }

            _streamWriter.WriteLine($"Win {result.GameId}:");

            for (var i = 0; i < result.Deck.Count; i++)
            {
                var count = (i + 1) % 10;
                _streamWriter.WriteLine($"  {(count == 0 ? 10 : count):D2}: {result.Deck[i]}");
            }

            _streamWriter.WriteLine();
        }
    }
}
