using Microsoft.Extensions.DependencyInjection;

namespace Http.HttpClientExamples
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var tasks = new Task[] { CallHttpClient(), CallHttpClientFactory() };
            await Task.WhenAll(tasks);
            Console.ReadKey();
        }

        private static async Task CallHttpClient()
        {
            // Create a new instance of HttpClient without using IHttpClientFactory
            var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine("HttpClient Result: " + responseContent);
        }

        /* For most cases, the best approach for making requests to external API is by using IHttpClientFactory. 
           Microsoft’s recommendation is to use IHttpClientFactory or use a static 
           or singleton HttpClient with PooledConnectionLifetime configured with the desired interval. */
        public static async Task CallHttpClientFactory()
        {
            // Create a new instance of IHttpClientFactory
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var httpClientFromFactory = httpClientFactory.CreateClient();
            var httpResponseFromFactory = await httpClientFromFactory.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
            var responseContentFromFactory = await httpResponseFromFactory.Content.ReadAsStringAsync();
            Console.WriteLine("HttpFactory Result: " + responseContentFromFactory);
        }
    }
}