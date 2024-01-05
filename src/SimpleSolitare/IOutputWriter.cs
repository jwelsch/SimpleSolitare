namespace SimpleSolitare
{
    public interface IOutputWriter
    {
        void WriteLine(string? text);

        void WriteLine(object? obj);
    }
}
