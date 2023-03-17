class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Server farm starting...");

        // Create a ThreadPool with 4 worker threads
        ThreadPool.SetMinThreads(4, 4);
        ThreadPool.SetMaxThreads(4, 4);

        // Start simulating requests
        for (int i = 0; i < 10; i++)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessRequest), i);
        }

        // Wait for all requests to complete
        while (ThreadPool.PendingWorkItemCount > 0)
        {
            Console.WriteLine("Waiting for requests to complete...");
            Thread.Sleep(1000);
        }

        Console.WriteLine("All requests completed");
        Console.ReadLine();
    }

    static void ProcessRequest(object state)
    {
        int requestId = (int)state;
        Console.WriteLine("Request {0} starting on thread {1}", requestId, Environment.CurrentManagedThreadId);

        // Simulate some work
        Thread.Sleep(5000);

        Console.WriteLine("Request {0} completed on thread {1}", requestId, Environment.CurrentManagedThreadId);
    }
}

