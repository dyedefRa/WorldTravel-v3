using Hangfire.Dashboard;

namespace WorldTravel.Web.Helpers
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (httpContext.User.IsInRole("admin"))
                return true;

            return false;

            //if (httpContext is { User: { Identity: { IsAuthenticated: true } } })
            //{
            //    return true;
            //}
            //return false;
        }
    }
}
