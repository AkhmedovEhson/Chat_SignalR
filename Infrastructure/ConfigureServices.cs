using Application.Common.Interfaces;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata.Ecma335;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(ops =>
            {
                ops.UseNpgsql("Server=localhost;Port=5434;Userid=postgres;Password=postgres;Pooling=false;" +
                    "MinPoolSize=1;MaxPoolSize=20;Timeout=15;SslMode=Disable;Database=chat;");
            });

            services.AddScoped<IApplicationDbContext>(service => service.GetRequiredService<ApplicationDbContext>());
            return services;
        }

    }
}