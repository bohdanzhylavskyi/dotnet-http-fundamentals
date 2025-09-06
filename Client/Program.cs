using Microsoft.Extensions.Configuration;
using System.Net;

namespace Client
{
    internal class Program
    {
        static readonly HttpClient client = new HttpClient();
        static readonly string serverUrl = ResolveServerUrl();

        static async Task Main(string[] args)
        {
            var tasks = new List<Func<Task>>()
            {
                ExecuteTask1,
                ExecuteTask2,
                ExecuteTask3,
                ExecuteTask4,
            };

            for (var i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"Task #{i + 1}");

                await tasks[i]();

                Console.WriteLine("\n----------------\n");
            }
        }

        private static async Task ExecuteTask1()
        {
            var url = CombineUrl(serverUrl, "MyName");

            using HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Url: {url}\nResponse={responseBody}");
        }

        private static async Task ExecuteTask2()
        {
            var pathes = new List<string>()
            {
                //"Information",
                "Success",
                "Redirection",
                "ClientError",
                "ServerError",
            };

            foreach (var path in pathes)
            {
                var url = CombineUrl(serverUrl, path);

                using HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Url: {url}, Status Code: {response.StatusCode} ({(int) response.StatusCode})");
            }
        }

        private static async Task ExecuteTask3()
        {
            var url = CombineUrl(serverUrl, "MyNameByHeader");

            using HttpResponseMessage response = await client.GetAsync(url);

            response.Headers.TryGetValues("X-MyName", out var headerValue);

            Console.WriteLine($"Url: {url}\nResponse 'X-MyName' Header Value: {string.Join(", ", headerValue)}");
        }

        private static async Task ExecuteTask4()
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true
            };

            var client = new HttpClient(handler);

            var url = CombineUrl(serverUrl, "MyNameByCookies");

            using HttpResponseMessage response = await client.GetAsync(url);

            var cookies = cookieContainer.GetCookies(new Uri(serverUrl));
            var cookiesList = cookies.Select(c => $"{c.Name} = {c.Value}").ToList();

            Console.WriteLine($"Url: {url}");
            Console.WriteLine($"Cookies: {string.Join(", ", cookiesList)}");
        }

        private static string CombineUrl(string originUrl, string path)
        {
            return new Uri(new Uri(originUrl), path).ToString();
        }

        private static string ResolveServerUrl()
        {
            var folder = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            return config.GetSection("ServerUrl").Value;
        }
    }
}
