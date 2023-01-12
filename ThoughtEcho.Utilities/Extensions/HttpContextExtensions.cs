using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace ThoughtEcho.Utilities.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetHeaderInfo(this HttpContext context, string key)
        {
            context.Request.Headers.TryGetValue(key, out StringValues value);
            return value.ToString();
        }

        public static string GetCookiesInfo(this HttpContext context, string key)
        {
            return context.Request.Cookies[key];
        }
    }
}
