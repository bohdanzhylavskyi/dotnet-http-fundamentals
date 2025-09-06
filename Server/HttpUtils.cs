using System.Net;

namespace Server
{
    internal static class HttpUtils
    {
        public static void WriteStringResponse(HttpListenerContext context, string responseString, int statusCode = 200)
        {
            HttpListenerResponse response = context.Response;

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.StatusCode = statusCode;
            response.ContentLength64 = buffer.Length;
            
            System.IO.Stream output = response.OutputStream;
            
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}
