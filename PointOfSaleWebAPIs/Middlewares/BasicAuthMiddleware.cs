using PointOfSale;
using System.Security.Claims;
using System.Text;

namespace PointOfSaleWebAPIs.Middlewares
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, POSDbContext dbContext)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Basic ".Length).Trim();
                var credentialBytes = Convert.FromBase64String(token);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                if (credentials.Length == 2)
                {
                    var username = credentials[0];
                    var password = credentials[1];

                    // Verify credentials against the database
                    var user = dbContext.Users.SingleOrDefault(u => u.name == username && u.password == password);

                    if (user != null)
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, username) };
                        var identity = new ClaimsIdentity(claims, "Basic");
                        context.User = new ClaimsPrincipal(identity);
                        await _next(context);
                        return;
                    }
                }
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }



}
