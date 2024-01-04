namespace SimpleSolitare.Wraps
{
    public interface IStreamWriterWrapFactory
    {
        IStreamWriterWrap Create(string path);
    }

    public class StreamWriterWrapFactory : IStreamWriterWrapFactory
    {
        public IStreamWriterWrap Create(string path)
        {
            return new StreamWriterWrap(new StreamWriter(path));
        }
    }
}
