namespace NonExclusiveLock_Semaphore
{
    internal class Program
    {
        public static Semaphore semaphore = new(3, 5);

        public static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread threadObject = new(DoSomeWork)
                {
                    Name = "Thread: " + i
                };
                threadObject.Start();
            }
            Console.ReadLine();
        }

        private static void DoSomeWork()
        {
            Console.WriteLine("{0} is waiting to enter the critical section.", Thread.CurrentThread.Name);
            semaphore.WaitOne();
            Console.WriteLine("{0} is inside the critical section now...", Thread.CurrentThread.Name);
            Thread.Sleep(1000);
            Console.WriteLine("{0} is releasing the critical section...", Thread.CurrentThread.Name);
            semaphore.Release();
        }
    }
}