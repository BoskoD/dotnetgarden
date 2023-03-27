namespace ExclusiveLock_LockStatement
{
    class Program
    {
        static readonly object _locker = new();

        static void ExclusiveResource()
        {
            List<int?> list = new();

            Console.WriteLine($"Lock requested by task {Task.CurrentId}");

            lock (_locker)
            {
                list.Add(Task.CurrentId);
                Console.WriteLine($"Lock acquired by task {Task.CurrentId}");
                Task.Delay(TimeSpan.FromSeconds(3)).Wait(); // Add Wait to block the task during delay
            }
        }

        static async Task Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(ExclusiveResource);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            await Task.WhenAll(tasks);
        }
    }
}