namespace SimpleSolitare
{
    public interface IConsoleWriter : IOutputWriter
    {
    }

    public class ConsoleWriter : IConsoleWriter
    {
        public void WriteLine(string? text)
        {
            Console.WriteLine(text);
        }

        public void WriteLine(object? obj)
        {
            Console.WriteLine(obj);
        }
    }
}
