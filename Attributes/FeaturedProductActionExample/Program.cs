using System.Reflection;

namespace FeaturedProductActionExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Get all methods in the ApiExample class
            var methods = typeof(ApiExample).GetMethods();

            // Iterate over the methods and check if they are marked as dangerous or safe
            foreach (var method in methods)
            {
                if (method.IsDefined(typeof(DangerousAttribute), false))
                {
                    Console.WriteLine($"{method.Name} is dangerous.");
                }
                else if (method.IsDefined(typeof(SafeAttribute), false))
                {
                    Console.WriteLine($"{method.Name} is safe.");
                }
                else
                {
                    Console.WriteLine($"{method.Name} has no safety attribute.");
                }
            }
            Console.ReadLine();
        }
    }

    // Define the custom attributes
    [AttributeUsage(AttributeTargets.Method)]
    public class DangerousAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class SafeAttribute : Attribute
    {
    }

    public class ApiExample
    {
        [Dangerous]
        public static void DoDangerousApiCall()
        {
            Console.WriteLine("Performing dangerous API call...");
        }

        [Safe]
        public static void DoSafeApiCall()
        {
            Console.WriteLine("Performing safe API call...");
        }
    }
}