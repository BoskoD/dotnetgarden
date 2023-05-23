// https://en.wikipedia.org/wiki/Dekker%27s_algorithm
namespace DekkerAlgorithm
{
    internal class Program
    {
        private static bool[] flag = new bool[2];
        private static int turn = 0;

        static void Main(string[] args)
        {
            Thread thread1 = new(Thread1Code);
            Thread thread2 = new(Thread2Code);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }

        static void Thread1Code()
        {
            while (true)
            {
                flag[0] = true;
                while (flag[1])
                {
                    if (turn != 0)
                    {
                        flag[0] = false;
                        while (turn != 0) ;
                        flag[0] = true;
                    }
                }

                // Critical Section
                Console.WriteLine("Thread 1 is inside the critical section.");
                Thread.Sleep(1000);

                turn = 1;
                flag[0] = false;
            }
        }

        static void Thread2Code()
        {
            while (true)
            {
                flag[1] = true;
                while (flag[0])
                {
                    if (turn != 1)
                    {
                        flag[1] = false;
                        while (turn != 1) ;
                        flag[1] = true;
                    }
                }

                // Critical Section
                Console.WriteLine("Thread 2 is inside the critical section.");
                Thread.Sleep(1000);

                turn = 0;
                flag[1] = false;
            }
        }
    }
}