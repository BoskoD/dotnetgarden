// TPL equivalent
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter a list of integers separated by spaces:");
        string input = Console.ReadLine();
        string[] numbers = input.Split(' ');

        Console.WriteLine("Creating tasks...");
        Task[] tasks = new Task[numbers.Length];
        for (int i = 0; i < numbers.Length; i++)
        {
            int number = int.Parse(numbers[i]);
            tasks[i] = Task.Run(() => PrintNumber(number));
        }

        Console.WriteLine("Waiting for tasks to complete...");
        await Task.WhenAll(tasks);

        Console.WriteLine("All tasks completed.");
        Console.ReadKey();
    }

    static void PrintNumber(int number)
    {
        // Do some work with the number parameter...

        Console.WriteLine($"Task {Task.CurrentId} completed for number {number}.");
    }
}