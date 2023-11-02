using JWTAuthentication.Repository;

namespace JWTAuthentication.Middleware
{
    public class TokenRevocationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenRevocationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITokenRepository tokenrepo)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var path = context.Request.Path;
            var host = context.Request.Host;
            var scheme = context.Request.Scheme;
            var mainpaths = scheme + "://" + host;
            if (mainpaths + path == mainpaths + "/api/Auth/Login" || mainpaths + path == mainpaths + "/api/Auth/Logout")
            {
                await _next(context);
                return;
            }
            if (!tokenrepo.IsTokenRevoked(token))
            {
                context.Response.StatusCode = 401;
                return;
            }

            await _next(context);
        }
    }

}
