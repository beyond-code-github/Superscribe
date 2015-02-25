using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Superscribe.Extensions
{
    public static class EnvironmentExtensions
    {
        public static Stream GetRequestBody(this IDictionary<string, object> environment)
        {
            return (Stream)environment["owin.RequestBody"];
        }

        public static Task WriteResponse(this IDictionary<string, object> environment, string text)
        {
            var data = Encoding.UTF8.GetBytes(text);
            return ((Stream)environment["owin.ResponseBody"]).WriteAsync(data, 0, data == null ? 0 : data.Length, CancellationToken.None);
        }

        public static Stream GetResponse(this IDictionary<string, object> environment)
        {
            return ((Stream)environment["owin.ResponseBody"]);
        }

        public static void SetResponseStatusCode(this IDictionary<string, object> environment, int statusCode)
        {
            environment["owin.ResponseStatusCode"] = statusCode;
        }

        public static bool TryGetHeaderValues(this IDictionary<string, object> environment, string key, out string[] values)
        {
            values = null;

            if (!environment.ContainsKey("owin.RequestHeaders"))
            {
                return false;
            }

            var headers = environment["owin.RequestHeaders"] as IDictionary<string, string[]>;
            if (headers == null)
            {
                return false;
            }

            if (!headers.ContainsKey(key))
            {
                return false;
            }

            values = headers[key];
            return true;
        }

        public static bool TrySetHeaderValues(this IDictionary<string, object> environment, string key, string[] values)
        {
            if (!environment.ContainsKey("owin.ResponseHeaders"))
            {
                return false;
            }

            var headers = environment["owin.ResponseHeaders"] as IDictionary<string, string[]>;
            if (headers == null)
            {
                return false;
            }

            if (headers.ContainsKey(key))
            {
                headers[key] = values;
                return true;
            }

            headers.Add(key, values);
            return true;
        }

        public static void SetResponseContentType(this IDictionary<string, object> environment, string contentType)
        {
            environment.TrySetHeaderValues("content-type", new[] { contentType });
        }
    }
}
