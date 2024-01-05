namespace SimpleSolitare
{
    public interface IInputMonitor
    {
        void StartMonitoring(CancellationToken cancellationToken);

        void StopMonitoring();

        bool IsRunning { get; }

        event EventHandler<EventArgs>? CancelRequested;
    }
}
