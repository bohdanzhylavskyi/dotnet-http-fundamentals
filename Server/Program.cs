using System.Net;

namespace Server
{
    public delegate void RequestHandler(HttpListenerContext context);

    internal class Program
    {
        private static Dictionary<string, RequestHandler> Handlers = new();

        static void Main(string[] args)
        {
            SetupHandlers();

            HttpListener listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:8888/");
            listener.Start();

            while (true)
            {
                HttpListenerContext context = listener.GetContext();

                HandleRequest(context);
            }
           
            listener.Stop();
        }

        private static void HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            string path = request.Url.AbsolutePath;

            Handlers.TryGetValue(path, out RequestHandler handler);

            if (handler != null)
            {
                handler(context);

                return;
            }

            NotFound(context);
        }

        private static void NotFound(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Not Found");
        }

        private static void SetupHandlers()
        {
            Handlers.Add("/MyName", RequestHandlers.GetMyName);
            Handlers.Add("/Information", RequestHandlers.Information);
            Handlers.Add("/Success", RequestHandlers.Success);
            Handlers.Add("/Redirection", RequestHandlers.Redirection);
            Handlers.Add("/ClientError", RequestHandlers.ClientError);
            Handlers.Add("/ServerError", RequestHandlers.ServerError);
            Handlers.Add("/MyNameByHeader", RequestHandlers.MyNameByHeader);
            Handlers.Add("/MyNameByCookies", RequestHandlers.MyNameByCookies);
        }
    }
}
