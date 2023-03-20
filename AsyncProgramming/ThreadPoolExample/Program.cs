/*Task: Write a C# console application that simulates a task scheduler using the thread pool in .NET. The application should do the following:

1) Accept a list of integers from the user.
2) Create a task for each integer in the list.
3) Use the thread pool to execute each task concurrently.
4) Each task should simply print out its integer value along with the name of the thread that executed it.
5) After all tasks have completed, the application should print out a message indicating that all tasks have completed.*/

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter a list of integers separated by spaces:");
        string input = Console.ReadLine();
        string[] numbers = input.Split(' ');

        Console.WriteLine("Creating tasks...");
        ManualResetEvent[] handles = new ManualResetEvent[numbers.Length];
        for (int i = 0; i < numbers.Length; i++)
        {
            int number = int.Parse(numbers[i]);
            handles[i] = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback(PrintNumber), new object[] { number, handles[i] });
        }

        Console.WriteLine("Waiting for tasks to complete...");
        WaitHandle.WaitAll(handles);

        Console.WriteLine("All tasks completed.");
        Console.ReadKey();
    }
    static void PrintNumber(object state)
    {
        int number = (int)((object[])state)[0];
        ManualResetEvent handle = (ManualResetEvent)((object[])state)[1];
        Console.WriteLine($"Task {number} executed on thread {Environment.CurrentManagedThreadId}.");
        handle.Set();
    }
}

