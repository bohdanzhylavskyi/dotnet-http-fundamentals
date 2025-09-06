using Microsoft.Extensions.Configuration;
using System.Net;

namespace Server
{
    public delegate void RequestHandler(HttpListenerContext context);

    internal class Program
    {
        private static Dictionary<string, RequestHandler> _requestHandlers = new();

        static void Main(string[] args)
        {
            SetupRequestHandlers();

            var serverUrl = ResolveServerUrl();

            HttpListener listener = new HttpListener();

            listener.Prefixes.Add(serverUrl);
            listener.Start();

            Console.WriteLine($"Server is listening on url '{serverUrl}'");

            while (true)
            {
                HttpListenerContext context = listener.GetContext();

                HandleRequest(context);
            }
           
            listener.Stop();
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

        private static void HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            string path = request.Url.AbsolutePath;

            _requestHandlers.TryGetValue(path, out RequestHandler handler);

            if (handler != null)
            {
                handler(context);

                return;
            }

            NotFound(context);
        }

        private static void NotFound(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Not Found", 404);
        }

        private static void SetupRequestHandlers()
        {
            _requestHandlers.Add("/MyName", RequestHandlers.GetMyName);

            _requestHandlers.Add("/Information", RequestHandlers.InformationStatusCode);
            _requestHandlers.Add("/Success", RequestHandlers.SuccessStatusCode);
            _requestHandlers.Add("/Redirection", RequestHandlers.RedirectionStatusCode);
            _requestHandlers.Add("/ClientError", RequestHandlers.ClientErrorStatusCode);
            _requestHandlers.Add("/ServerError", RequestHandlers.ServerErrorStatusCode);
            
            _requestHandlers.Add("/MyNameByHeader", RequestHandlers.MyNameByHeader);
            
            _requestHandlers.Add("/MyNameByCookies", RequestHandlers.MyNameByCookies);
        }
    }
}
