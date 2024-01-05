namespace SimpleSolitare
{
    public interface IOutputWriter
    {
        void Write(string? value);

        void Write(object? value);

        void WriteLine(string? text);

        void WriteLine(object? obj);
    }
}
