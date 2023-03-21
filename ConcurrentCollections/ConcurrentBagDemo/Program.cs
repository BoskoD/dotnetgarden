using System.Collections.Concurrent;

namespace ConcurrentBagDemo
{
    internal class Program
    {
        static void Main()
        {
            // Add to ConcurrentBag concurrently
            ConcurrentBag<int> cb = new();
            List<Task> bagAddTasks = new();
            for (int i = 0; i < 500; i++)
            {
                var numberToAdd = i;
                bagAddTasks.Add(Task.Run(() => cb.Add(numberToAdd)));
            }

            // Wait for all tasks to complete
            Task.WaitAll(bagAddTasks.ToArray());

            // Consume the items in the bag
            List<Task> bagConsumeTasks = new();
            int itemsInBag = 0;

            while (!cb.IsEmpty)
            {
                bagConsumeTasks.Add(Task.Run(() =>
                {
                    if (cb.TryTake(out int item))
                    {
                        Console.WriteLine(item);
                        Interlocked.Increment(ref itemsInBag);
                    }
                }));
            }
            Task.WaitAll(bagConsumeTasks.ToArray());

            Console.WriteLine($"There were {itemsInBag} items in the bag");

            // Checks the bag for an item
            // The bag should be empty and this should not print anything
            if (cb.TryPeek(out int unexpectedItem))
                Console.WriteLine("Found an item in the bag when it should be empty");
        }

    }
}