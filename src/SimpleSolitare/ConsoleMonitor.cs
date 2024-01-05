namespace SimpleSolitare
{
    public interface IConsoleMonitor : IInputMonitor
    {
    }

    public class ConsoleMonitor : IConsoleMonitor
    {
        private bool _stopRequested;
        private Thread? _worker;

        public bool IsRunning => _worker?.IsAlive ?? false;

        public event EventHandler<EventArgs>? CancelRequested;

        public void StartMonitoring(CancellationToken cancellationToken)
        {
            var threadStart = new ThreadStart(() =>
            {
                Monitor(cancellationToken);
            });

            _worker = new Thread(threadStart);

            _worker.Start();
        }

        public void StopMonitoring()
        {
            _stopRequested = true;
        }

        private void Monitor(CancellationToken cancellationToken)
        {
            Console.CancelKeyPress += OnCancelKeyPressed;

            while (!cancellationToken.IsCancellationRequested
                && !_stopRequested)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.X)
                    {
                        RaiseCancelRequested();
                    }
                }

                Thread.Sleep(100);
            }

            Console.CancelKeyPress -= OnCancelKeyPressed;
        }

        private void OnCancelKeyPressed(object? sender, EventArgs e)
        {
            RaiseCancelRequested();
        }

        private void RaiseCancelRequested()
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
