class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Server farm starting...");

        // Create a TaskFactory with worker threads based on the number of processors
        int workerThreads = Environment.ProcessorCount;
        TaskFactory factory = new(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, new LimitedConcurrencyLevelTaskScheduler(workerThreads));
        Task[] tasks = new Task[10];

        // Start simulating requests
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = factory.StartNew(async () => await ProcessRequestAsync(i));
        }

        // Wait for all requests to complete
        await Task.WhenAll(tasks);

        Console.WriteLine("All requests completed");
        Console.ReadLine();
    }

    static async Task ProcessRequestAsync(int requestId)
    {
        Console.WriteLine($"Request {requestId} starting on thread {Environment.CurrentManagedThreadId}");

        // Simulate some work
        await Task.Delay(5000);

        Console.WriteLine($"Request {requestId} completed on thread {Environment.CurrentManagedThreadId}");
    }

    // A custom task scheduler that limits the concurrency to a specified number of threads
    class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        private readonly object _lock = new();
        private readonly int _maxDegreeOfParallelism;
        private int _runningTasks = 0;
        private readonly List<Task> _tasks = new();

        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism));
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        protected override void QueueTask(Task task)
        {
            lock (_lock)
            {
                if (_runningTasks < _maxDegreeOfParallelism)
                {
                    _runningTasks++;
                    TryExecuteTask(task);
                }
                else
                {
                    _tasks.Add(task);
                }
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (Thread.CurrentThread.ManagedThreadId == Environment.CurrentManagedThreadId)
            {
                return TryExecuteTask(task);
            }
            else
            {
                return false;
            }
        }

        protected override bool TryDequeue(Task task)
        {
            lock (_lock)
            {
                return _tasks.Remove(task);
            }
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_lock)
            {
                return _tasks.ToList();
            }
        }
    }
}