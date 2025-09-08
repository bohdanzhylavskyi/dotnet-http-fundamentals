using System.Net;

namespace Server
{
    public static class RequestHandlers
    {
        public static void GetMyName(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Bohdan");
        }

        public static void InformationStatusCode(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Information", 101);
        }

        public static void SuccessStatusCode(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Success", 201);
        }

        public static void RedirectionStatusCode(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Redirection", 301);
        }

        public static void ClientErrorStatusCode(HttpListenerContext context)
        {
            HttpUtils.WriteStringResponse(context, "Client Error", 401);
        }

        public static void ServerErrorStatusCode(HttpListenerContext context)
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
