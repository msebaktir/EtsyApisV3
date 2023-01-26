

using System.Security.Claims;
using dotnetEtsyApp.Models.Cache;

namespace dotnetEtsyApp.Middleware
{

    public static class RequestHandler
    {

        public static async Task Handle(HttpContext context, Func<Task> next)
        {
            if (string.IsNullOrEmpty(Global.DomainName))
            {
                var https = (context.Request.IsHttps || context.Request.Headers["X-Forwarded-Proto"] == Uri.UriSchemeHttps) ? "https://" : "http://";

                Global.DomainName = https + context.Request.Host.Value;


            }
            await next();
            var user = context.User;
            if (context.Response.StatusCode == 404 && !System.IO.Path.HasExtension(context.Request.Path.Value))
            {
                context.Request.Path = "/Home/Index";
                await next();
            }

        }
        public static async Task<string> GetCurrentUserId(HttpContext context)
        {
            var user = context.User;
            if (user.Identity.IsAuthenticated)
            {
                var userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                return userId;
            }
            else
            {
                return null;
            }
        }
    }
}