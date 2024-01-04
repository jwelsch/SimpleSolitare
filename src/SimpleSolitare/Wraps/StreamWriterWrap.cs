namespace SimpleSolitare.Wraps
{
    public interface IStreamWriterWrap : IDisposable
    {
        void Close();

        void Write(object value);

        void Write(string value);

        void Write(char value);

        void Write(int value);

        void Write(long value);

        void Write(double value);

        void Write(decimal value);

        void WriteLine();

        void WriteLine(object value);

        void WriteLine(string value);

        void WriteLine(char value);

        void WriteLine(int value);

        void WriteLine(long value);

        void WriteLine(double value);

        void WriteLine(decimal value);
    }

    public class StreamWriterWrap : IStreamWriterWrap
    {
        private readonly StreamWriter _streamWriter;

        public StreamWriterWrap(StreamWriter streamWriter)
        {
            _streamWriter = streamWriter;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            _streamWriter.Dispose();
        }

        public void Close() => _streamWriter.Close();

        public void Write(object value) => _streamWriter.Write(value);

        public void Write(string value) => _streamWriter.Write(value);

        public void Write(char value) => _streamWriter.Write(value);

        public void Write(int value) => _streamWriter.Write(value);

        public void Write(long value) => _streamWriter.Write(value);

        public void Write(double value) => _streamWriter.Write(value);

        public void Write(decimal value) => _streamWriter.Write(value);

        public void WriteLine() => _streamWriter.WriteLine();

        public void WriteLine(object value) => _streamWriter.WriteLine(value);

        public void WriteLine(string value) => _streamWriter.WriteLine(value);

        public void WriteLine(char value) => _streamWriter.WriteLine(value);

        public void WriteLine(int value) => _streamWriter.WriteLine(value);

        public void WriteLine(long value) => _streamWriter.WriteLine(value);

        public void WriteLine(double value) => _streamWriter.WriteLine(value);

        public void WriteLine(decimal value) => _streamWriter.WriteLine(value);
    }
}
