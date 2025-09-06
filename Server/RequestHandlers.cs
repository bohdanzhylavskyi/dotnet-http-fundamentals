using System.Net;

namespace Server
{
    internal static class RequestHandlers
    {
        public static void GetMyName(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Bohdan");
        }

        public static void Information(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Information", 100);
        }

        public static void Success(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Success", 201);
        }

        public static void Redirection(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Redirection", 301);
        }

        public static void ClientError(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "ClientError", 401);
        }

        public static void ServerError(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Server Error", 500);
        }

        public static void MyNameByHeader(HttpListenerContext context)
        {
            context.Response.AddHeader("X-MyName", "Bohdan");

            HttpUtils.WriteStringResponse(context, "");
        }

        public static void MyNameByCookies(HttpListenerContext context)
        {
            var cookie = new Cookie();

            cookie.Name = "MyName";
            cookie.Value = "Bohdan";

            context.Response.Cookies.Add(cookie);

            HttpUtils.WriteStringResponse(context, "");
        }

    }
}
