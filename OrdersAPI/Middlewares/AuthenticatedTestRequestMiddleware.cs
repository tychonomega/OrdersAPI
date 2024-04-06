using System.Security.Claims;

namespace App.Middlewares
{
    public class AuthenticatedTestRequestMiddleware
    {
        public const string TestingAccessTokenAuthentication = "TestingAccessTokenAuthentication";
        public const string TestingHeader = "X-Integration-Testing";
        public const string TestingHeaderValue = "abcde-12345";

        private readonly RequestDelegate _next;

        public AuthenticatedTestRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Contains("X-Integration-Testing"))
            {

                if (context.Request.Headers.Keys.Contains("Authorization"))
                {
                    var token = context.Request.Headers["Authorization"].First();

                    var claimsIdentity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Authentication, token)
                }, TestingAccessTokenAuthentication);

                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    context.User = claimsPrincipal;
                }
            }


            await _next(context);
        }
    }

    public static class AuthenticatedTestRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticatedTestRequest(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticatedTestRequestMiddleware>();
        }
    }

}
