using ChatService.Filters;

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



            return services;
        }
    }
}
