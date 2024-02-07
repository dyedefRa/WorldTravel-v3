using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WorldTravel
{
    public class WorldTravelWebTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<WorldTravelWebTestModule>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.InitializeApplication();
        }
    }
}