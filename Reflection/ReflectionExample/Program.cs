using System.Reflection;

namespace ReflectionExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AnalyzeReflection("<path.dll>");
        }

        private static void AnalyzeReflection(string dllPath)
        {
            if (!System.IO.File.Exists(dllPath))
            {
                Console.WriteLine($"The file '{dllPath}' does not exist.");
                return;
            }

            Assembly assembly = Assembly.LoadFrom(dllPath);

            try
            {
                var classTypes = assembly.GetTypes();

                foreach (var type in classTypes)
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        Console.WriteLine($"Class name: {type.Name}");

                        var methods = type.GetMethods(BindingFlags.Public);
                        foreach (var method in methods)
                        {
                            Console.WriteLine($"\tMethod: {method.Name}");

                            var parameters = method.GetParameters();
                            foreach (var parameter in parameters)
                            {
                                Console.WriteLine($"\t\tParameter: {parameter.Name} - Class that declares this parameter: {parameter.GetType().DeclaringType}");
                            }
                        }

                        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                        foreach (var property in properties)
                        {
                            Console.WriteLine($"\tProperty: {property.Name} - Class that declares this type: {property.DeclaringType}");
                        }

                        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                        foreach (var field in fields)
                        {
                            Console.WriteLine($"\tField: {field.Name} - Field type: {field.FieldType} - Class that declares this field: {field.GetType().DeclaringType}");
                        }

                        var attributes = type.GetCustomAttributes(true);
                        foreach (var attribute in attributes)
                        {
                            Console.WriteLine($"\tAttribute: {attribute.GetType().Name}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}