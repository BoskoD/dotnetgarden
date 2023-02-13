using System.Diagnostics;
using System.Numerics;

namespace DifferenceExample
{
    internal class Program
    {
        static List<Employee> GetData()
        {
            List<Employee> employeeList = new();
            Random random = new(1000);

            for (int i = 0; i < 1000; i++)
            {
                employeeList.Add(new Employee() { Name = "Employee" + i, Salary = GetRandomNumber(random, 1000, 5000) });
            }
            return employeeList;
        }
        static double GetRandomNumber(Random random, double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        static void Main()
        {
            List<Employee> employeeList = GetData();

            Stopwatch stopWatch = new();

            //Testing LINQ With Expensive Computation
            stopWatch.Start();
            var linqResult = employeeList.Where(e => e.ExpensiveComputation());
            int empCount = linqResult.Count();
            stopWatch.Stop();
            Console.WriteLine(string.Format($"LINQ in Expensive Computation is {stopWatch.Elapsed.TotalMilliseconds} to get {empCount} Employees"));
            stopWatch.Reset();

            // Testing PLINQ With Expensive Computation
            stopWatch.Start();
            linqResult = employeeList.AsParallel().Where(e => e.ExpensiveComputation());
            empCount = linqResult.Count();
            stopWatch.Stop();
            Console.WriteLine(string.Format($"PLINQ in Expensive Computation is {stopWatch.Elapsed.TotalMilliseconds} to get {empCount} Employees"));
            stopWatch.Reset();
            Console.WriteLine();


            // Testing LINQ With NON Expensive Computation
            stopWatch.Start();
            linqResult = employeeList.Where(e => e.NonExpensiveComputation());
            empCount = linqResult.Count();
            stopWatch.Stop();
            Console.WriteLine(string.Format($"Time taken by LINQ in Non Expensive Computation is {stopWatch.Elapsed.TotalMilliseconds} to get {empCount} Employees"));
            stopWatch.Reset();

            // Testing PLINQ With NON Expensive Computation
            stopWatch.Start();
            linqResult = employeeList.AsParallel().Where(e => e.NonExpensiveComputation());
            empCount = linqResult.Count();
            stopWatch.Stop();
            Console.WriteLine(string.Format($"Time taken by PLINQ in Non Expensive Computation is {stopWatch.Elapsed.TotalMilliseconds} to get {empCount} Employees"));
            stopWatch.Reset();
            Console.ReadKey();
        }
    }

    public class Employee
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public double Salary { get; set; }

        public bool ExpensiveComputation()
        {
            Thread.Sleep(10);
            return (Salary > 2000 && Salary < 3000);
        }

        public bool NonExpensiveComputation()
        {
            return (Salary > 2000 && Salary < 3000);
        }
    }
}