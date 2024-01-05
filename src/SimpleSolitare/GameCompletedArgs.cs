namespace SimpleSolitare
{
    public class GameCompletedArgs : EventArgs
    {
        public IGameResult Result { get; }

        public object? Context { get; }

        public GameCompletedArgs(IGameResult result, object? context)
        {
            Result = result;
            Context = context;
        }
    }
}
