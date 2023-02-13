namespace CustomAttributeExamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var classAttribute = (MyCustomAttribute)Attribute.GetCustomAttribute(typeof(MyClass), typeof(MyCustomAttribute));
            Console.WriteLine($"Class attribute: {classAttribute.Description}");

            var methodAttribute = (MyCustomAttribute)Attribute.GetCustomAttribute(typeof(MyClass).GetMethod("MyMethod"), typeof(MyCustomAttribute));
            Console.WriteLine($"Method attribute: {methodAttribute.Description}");

            Console.ReadLine();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MyCustomAttribute : Attribute
    {
        public string Description { get; set; }

        public MyCustomAttribute(string description)
        {
            Description = description;
        }
    }

    [MyCustom("Class-level attribute")]
    public class MyClass
    {
        [MyCustom("Method-level attribute")]
        public static void MyMethod()
        {
            Console.WriteLine("Hello, world!");
        }
    }
}