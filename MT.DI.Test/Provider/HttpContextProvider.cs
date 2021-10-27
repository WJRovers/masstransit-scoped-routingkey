using System.Linq;
using Microsoft.AspNetCore.Http;

namespace MT.DI.Test.Provider
{
    public class HttpContextProvider : IContextProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetHeader(string key)
        {
            var httpContext = _contextAccessor.HttpContext;

            if (httpContext == null)
                return default;

            if (!httpContext.Request.Headers.TryGetValue(key, out var values))
                return default;

            return values.FirstOrDefault();
        }
    }
}