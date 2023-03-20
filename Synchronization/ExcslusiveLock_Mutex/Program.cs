namespace ExcslusiveLock_Mutex
{
    internal class Program
    {
        private static Mutex? mutex = null;

        static void Main(string[] args)
        {
            const string applicationName = "Deadlock";
            mutex = new Mutex(true, applicationName, out bool isCreatedNew);

            if (!isCreatedNew)
            {
                Console.WriteLine("Another instance is already running. Press any key to exit...");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("This is the only instance running. Press any key to exit...");
                Console.ReadKey();
            }
            mutex.ReleaseMutex();
        }
    }
}