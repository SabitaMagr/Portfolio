using System.IdentityModel.Tokens.Jwt;

namespace Portfolio.Middleware
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceFactory;
        public JwtTokenMiddleware(RequestDelegate next, IServiceScopeFactory serviceFactory)
        {
            _next = next;
            _serviceFactory = serviceFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    var IdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                    var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Full_name")?.Value;

                    if (!string.IsNullOrEmpty(IdClaim) && int.TryParse(IdClaim, out var userId))
                    {
                        context.Items["userId"] = userId;
                    }
                    if (!string.IsNullOrEmpty(nameClaim))
                    {
                        context.Items["Full_name"] = nameClaim;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }
            await _next(context);
        }
    }
}
