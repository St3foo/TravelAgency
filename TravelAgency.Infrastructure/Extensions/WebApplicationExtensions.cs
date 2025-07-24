using Microsoft.AspNetCore.Builder;
using TravelAgency.Infrastructure.Middlewares;

namespace TravelAgency.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UserAdminRedirection(this IApplicationBuilder app)
        {
            app.UseMiddleware<AdminRedirectionMiddleware>();

            return app;
        }
    }
}
