using ChatService.Filters;
using Serilog;

namespace ChatService
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services)
        {
            services.AddSignalR();

            services.AddMvc(o =>
            {
                o.Filters.Add(new HubExceptionFilter());
            });


            services.AddCors(cors =>
            {
                cors.AddDefaultPolicy(p =>
                {
                    p.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                            .CreateLogger();

            return services;
        }
    }
}
