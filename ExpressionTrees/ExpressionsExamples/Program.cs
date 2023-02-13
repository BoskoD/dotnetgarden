using System.Linq.Expressions;

namespace ExpressionsExamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            int threshold = 3;
            FilterArray(numbers, threshold); // filter numbers > 3
            Addition(threshold); // bump threshold to 10
            RaiseToPower(threshold); // raising threshold to power of 10
        }

        private static void Addition(int input)
        {
            // Create a parameter expression.
            ParameterExpression paramExpr = Expression.Parameter(typeof(int), "x");

            // Create a constant expression.
            ConstantExpression constExpr = Expression.Constant(10);

            // Create a binary expression that adds the parameter and the constant.
            BinaryExpression addExpr = Expression.Add(paramExpr, constExpr);

            // Compile the expression tree into a lambda expression.
            Func<int, int> lambdaExpr = Expression.Lambda<Func<int, int>>(addExpr, paramExpr).Compile();

            // Call the lambda expression with an argument.
            int result = lambdaExpr(input);
            Console.WriteLine($"Threshold updated to {result}");
        }

        private static void FilterArray(int[] input, int threshold)
        {
            // Create an expression tree that represents a filter for numbers greater than 3
            ParameterExpression pe = Expression.Parameter(typeof(int), "num");
            ConstantExpression constant = Expression.Constant(threshold);
            BinaryExpression greaterThan = Expression.GreaterThan(pe, constant);
            Expression<Func<int, bool>> expression = Expression.Lambda<Func<int, bool>>(greaterThan, pe);

            // Use the expression tree to dynamically filter the numbers array
            var result = input.Where(expression.Compile());

            // Print the result
            Console.WriteLine("Numbers greater than 3:");
            foreach (var num in result)
            {
                Console.WriteLine(num);
            }
        }

        //Expression Tree with Method Call
        private static void RaiseToPower(int numToRaise)
        {
            // Create a parameter expression.
            ParameterExpression paramExpr = Expression.Parameter(typeof(int), "x");

            // Convert the parameter expression to a double type.
            UnaryExpression paramExprConverted = Expression.Convert(paramExpr, typeof(double));

            // Create a constant expression.
            ConstantExpression constExpr = Expression.Constant(10.0, typeof(double));

            // Create a method call expression that calls the Math.Pow method.
            MethodCallExpression powExpr = Expression.Call(typeof(Math), "Pow", null, paramExprConverted, constExpr);

            // Compile the expression tree into a lambda expression.
            Func<int, double> lambdaExpr = Expression.Lambda<Func<int, double>>(powExpr, paramExpr).Compile();

            // Call the lambda expression with an argument.
            double result = lambdaExpr(numToRaise);

            Console.WriteLine(result);
        }
    }
}